using System.Linq;
using NoeticTools.GitVersionAfterBurner;
using NoeticTools.GitVersionAfterBurner.Framework.BuildHosting;
using NoeticTools.GitVersionAfterBurner.Tools.GitVersion;


var runner = ScriptRunner.Instance!;
var bridge = new GitVersionBridge(runner.GitVersion.FileIO.Read());
var host = runner.Host;

bridge.ApplyExtendedSemverPrereleaseLabel(runner.Host.IsControlled);

if (bridge.IsARelease)
{
    bridge.NuGetVersion = bridge.NuGetVersion.WithoutPrerelease()
                                .WithoutMetadata();
    bridge.InformationalVersion = bridge.InformationalVersion.WithMetadata(host.BuildNumber, host.BuildContext, bridge.EscapedBranchName, bridge.ShortSha);
}
else
{
    bridge.WithPrerelease(bridge.PreReleaseLabel, host.BuildNumber, host.BuildContext);
    bridge.NuGetVersion = bridge.NuGetVersion.WithoutMetadata();
    bridge.InformationalVersion = bridge.InformationalVersion.WithMetadata(bridge.EscapedBranchName, bridge.ShortSha);
}

runner.GitVersion.Validation.ValidateStrict(bridge.Inner);
runner.GitVersion.FileIO.Write(bridge.Inner);


if (runner.Inputs.UpdateHostBuildLabel)
{
    var normalSemver = bridge.NormalSemver;
    var buildVersion = bridge.IsARelease ? normalSemver.WithMetadata(host.BuildNumber, host.BuildContext) :
        normalSemver.WithPrerelease(bridge.PreReleaseLabel, host.BuildNumber, host.BuildContext)
                    .WithoutMetadata();
    host.SetBuildLabel(buildVersion.ToString());
}

runner.Logger.LogImportantMessage("Version: " + bridge.InformationalVersion);