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



const string initialDevPreReleaseTag = "experimental";
const string uncontrolledPreReleaseTag = "alpha";
const string controlledPreReleaseTag = "beta";

var runner = ScriptRunner.Instance!;
var gitVersionInfo = runner.GitVersion.FileIO.Read();
var host = runner.Host;

var prereleaseLabel = new SemverIdentifiers(!runner.Host.IsControlled ? uncontrolledPreReleaseTag :
                                            gitVersionInfo.Major == 0 ? initialDevPreReleaseTag :
                                            controlledPreReleaseTag);

var buildNumberAndContext = new SemverIdentifiers(host.BuildNumber, host.BuildContext);
var branchAndSha = new SemverIdentifiers(gitVersionInfo.BranchName, gitVersionInfo.ShortSha);

if (gitVersionInfo.IsARelease)
{
    gitVersionInfo.Update(SemverIdentifiers.None, buildNumberAndContext, buildNumberAndContext + branchAndSha);
}
else
{
    gitVersionInfo.Update(prereleaseLabel + buildNumberAndContext, SemverIdentifiers.None, branchAndSha);
}

runner.Logger.LogImportantMessage("Version: " + gitVersionInfo.InformationalVersion);

if (runner.Inputs.UpdateHostBuildLabel)
{
    host.SetBuildLabel(gitVersionInfo.FullSemVer);
}

runner.GitVersion.Validation.ValidateStrict(gitVersionInfo);
runner.GitVersion.FileIO.Write(gitVersionInfo);
