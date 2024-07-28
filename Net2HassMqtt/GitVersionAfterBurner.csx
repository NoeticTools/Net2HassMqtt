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
using NoeticTools.GitVersionAfterBurner.Tools;
using NoeticTools.GitVersionAfterBurner.Tools.GitVersion;


new VersionInfoTransform(GitVersionAfterBurnerTask.Instance).Execute();

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
            var versionInfo = _runner.GitVersion.FileIO.Read();
            if (_runner.Logger.HasLoggedErrors)
            {
                return;
            }

            var buildNumber = GetBuildNumber();
            if (buildNumber.Split('.').Length != 2)
            {
                _runner.Logger.LogError("'BuildNumber' format is not consistent with the format recommended for projects that may build on GitHub ('<run_number>.<retry_number>'). Two dot delimited identifiers like '2356.1' or 'Computer1.2356' are required.");
            }

            var preRelease = new SemverPreReleaseIdentifiers(GetPreReleaseLabel(versionInfo)).Add(buildNumber);
            var metadata = new SemverMetadataIdentifiers(buildNumber).AddAdditional(versionInfo!.BranchName, versionInfo!.ShortSha);

            versionInfo!.Update(preRelease, metadata);

            _runner.Logger.LogImportantMessage($"Informational version: {versionInfo.InformationalVersion}");

            _runner.GitVersion.Validation.ValidateStrict(versionInfo);
            _runner.GitVersion.FileIO.Write(versionInfo);
        }
        catch (Exception exception)
        {
            _runner.Logger.LogErrorFromException(exception);
            throw;
        }
    }

    /// <summary>
    ///     Return a two part build number.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Returns a build number as two dot delimited identifiers.
    ///     </para>
    ///     <para>
    ///         Format:
    ///     </para>
    ///     <code>
    ///         &lt;identifier_1&gt;.&lt;identifier_1&gt;
    ///     </code>
    ///     <para>
    ///         Unconventional, but used to achieve consistent and sortable version format compatible with:
    ///     </para>
    ///     <list type="bullet">
    ///         <item>GitHub build system builds</item>
    ///         <item>Uncontrolled (dev box) builds</item>
    ///     </list>
    ///     <para>
    ///         GitHub currently does not provide a build number. One needs to be constructed as:
    ///     </para>
    ///     <code>
    ///         &lt;github_run_number&gt;.&lt;github_attempt_number&gt;
    ///     </code>
    ///     <para>
    ///         A build system that does provide a build number then uses the format:
    ///     </para>
    ///     <code>
    ///         &lt;build_number&gt;.&lt;0&gt;
    ///     </code>
    ///     <para>
    ///         Uncontrolled builds inherently do not have build numbers.
    ///         To get some short term numbering one can be created using a configuration file.
    ///         As this is only of any used on the one host a computer name is included.
    ///     </para>
    ///     <code>
    ///         &lt;temp_build_number&gt;.&lt;computer_name&gt;
    ///     </code>
    /// </remarks>
    private string GetBuildNumber()
    {
        var buildNumber = _runner.Inputs.BuildNumber;
        if (!string.IsNullOrWhiteSpace(buildNumber))
        {
            return buildNumber;
        }

        if (!_runner.Inputs.IsBuildSystemBuild)
        {
            return UncontrolledHost.GetAndBumpBuildNumber();
        }

        _runner.Logger.LogError("'BuildNumber' is required on build system.");
        return "";
    }

    private string GetPreReleaseLabel(GitVersionInfo versionInfo)
    {
        if (!_runner.Inputs.IsBuildSystemBuild)
        {
            return UncontrolledPreReleaseTag;
        }

        if (!versionInfo.IsInInitialDevelopment)
        {
            return versionInfo.PreReleaseLabel;
        }

        _runner.Logger.LogMessage("Initial development build.");
        return InitialDevPreReleaseTag;
    }
}