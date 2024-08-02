/*
 * Build script to transform GitVersion generated information (GitVersion.json) to requirements.
 *
 * This script applies a Semver version scheme where:
 *
 *   * A pre-release 'experimental' is a controlled (build system) build for any 0.x.x build (initial development phase).
 *   * A release version is a controlled build from a release branch.
 *   * A pre-release 'beta' version is a controlled build from a non-release branch.
 *   * A pre-release 'alpha' version is an uncontrolled (dev box) build regardless of branch.
 */

using System;
using NoeticTools.GitVersionAfterBurner;
using NoeticTools.GitVersionAfterBurner.Framework.Semver;
using NoeticTools.GitVersionAfterBurner.Tools.GitVersion;


new VersionInfoTransform(ScriptRunner.Instance).Execute();

public class VersionInfoTransform
{
    private const string InitialDevPreReleaseTag = "experimental";
    private const string UncontrolledPreReleaseTag = "alpha";
    private readonly IScriptRunner _runner;

    public VersionInfoTransform(IScriptRunner runner)
    {
        _runner = runner;
    }

    public void Execute()
    {
        try
        {
            var gitVersionInfo = _runner.GitVersion.FileIO.Read();
            if (_runner.Logger.HasLoggedErrors)
            {
                return;
            }

            var host = _runner.Host;

            var preRelease = gitVersionInfo.IsARelease
                ? SemverIdentifiers.None
                : new SemverIdentifiers(GetPreReleaseLabel(gitVersionInfo), host.BuildNumber, host.BuildIdentifier);

            var shortMetadata = SemverIdentifiers.None;

            var informationalMetadata = gitVersionInfo.IsARelease
                ? new SemverIdentifiers(host.BuildNumber, host.BuildIdentifier, gitVersionInfo.BranchName, gitVersionInfo.ShortSha)
                : new SemverIdentifiers(gitVersionInfo.BranchName, gitVersionInfo.ShortSha);

            gitVersionInfo.Update(preRelease, shortMetadata, informationalMetadata);

            if (_runner.Inputs.UpdateHostBuildLabel)
            {
                host.SetBuildLabel(gitVersionInfo.FullSemVer);
            }

            _runner.GitVersion.Validation.ValidateStrict(gitVersionInfo);
            _runner.GitVersion.FileIO.Write(gitVersionInfo);
        }
        catch (Exception exception)
        {
            _runner.Logger.LogErrorFromException(exception);
            throw;
        }
    }

    private string GetPreReleaseLabel(GitVersionInfo versionInfo)
    {
        if (!_runner.Host.IsControlled)
        {
            return UncontrolledPreReleaseTag;
        }

        if (versionInfo.Major == 0)
        {
            return versionInfo.PreReleaseLabel;
        }

        _runner.Logger.LogMessage("Initial development build.");
        return InitialDevPreReleaseTag;
    }
}