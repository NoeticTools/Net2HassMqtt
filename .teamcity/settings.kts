import jetbrains.buildServer.configs.kotlin.*
import jetbrains.buildServer.configs.kotlin.CustomChart.*
import jetbrains.buildServer.configs.kotlin.buildFeatures.perfmon
import jetbrains.buildServer.configs.kotlin.buildSteps.DotnetBuildStep
import jetbrains.buildServer.configs.kotlin.buildSteps.DotnetPackStep
import jetbrains.buildServer.configs.kotlin.buildSteps.dotnetBuild
import jetbrains.buildServer.configs.kotlin.buildSteps.dotnetPack
import jetbrains.buildServer.configs.kotlin.buildSteps.script
import jetbrains.buildServer.configs.kotlin.projectCustomChart
import jetbrains.buildServer.configs.kotlin.projectFeatures.githubIssues
import jetbrains.buildServer.configs.kotlin.triggers.vcs

/*
The settings script is an entry point for defining a TeamCity
project hierarchy. The script should contain a single call to the
project() function with a Project instance or an init function as
an argument.

VcsRoots, BuildTypes, Templates, and subprojects can be
registered inside the project using the vcsRoot(), buildType(),
template(), and subProject() methods respectively.

To debug settings scripts in command-line, run the

    mvnDebug org.jetbrains.teamcity:teamcity-configs-maven-plugin:generate

command and attach your debugger to the port 8000.

To debug in IntelliJ Idea, open the 'Maven Projects' tool window (View
-> Tool Windows -> Maven Projects), find the generate task node
(Plugins -> teamcity-configs -> teamcity-configs:generate), the
'Debug' option is available in the context menu for the task.
*/

version = "2025.03"

project {

    buildType(Build)

    params {
        param("teamcity.git.fetchAllHeads", "true")
    }

    features {
        githubIssues {
            id = "PROJECT_EXT_14"
            displayName = "NoeticTools/Net2HassMqtt"
            repositoryURL = "https://github.com/NoeticTools/Net2HassMqtt"
            authType = storedToken {
                tokenId = "tc_token_id:CID_3de2c2727993edab40e4371046ac9db7:-1:829efbaf-34e6-4ebd-a3e7-2c1dd979b30b"
            }
        }
        projectCustomChart {
            id = "PROJECT_EXT_2"
            title = "GitVersionAfterBurner"
            seriesTitle = "Build time"
            series = listOf(
                Serie(title = "Queue wait reason: All compatible agents are outdated waiting for upgrade", key = SeriesKey("queueWaitReason:All_compatible_agents_are_outdated_waiting_for_upgrade"), sourceBuildTypeId = "Net2HassMqtt_Build")
            )
            param("format", "durationSeconds")
        }
    }

    cleanup {
        baseRule {
            artifacts(builds = 20, days = 5)
        }
    }
}

object Build : BuildType({
    name = "Build (uses local nuget repo)"

    artifactRules = "+:Output/*.nupkg"
    publishArtifacts = PublishMode.SUCCESSFUL

    params {
        param("env.Git_Branch", "${DslContext.settingsRoot.paramRefs.buildVcsBranch}")
    }

    vcs {
        root(DslContext.settingsRoot)

        cleanCheckout = true
    }

    steps {
        script {
            name = "Flush local nuget caches"
            id = "Test_git_execute"
            scriptContent = """
                echo 'Flush local nuget caches'
                dotnet nuget locals --clear all
            """.trimIndent()
        }
        dotnetBuild {
            name = "Build"
            id = "Build"
            projects = "Net2HassMqtt.sln"
            configuration = "Release"
            args = "--property:Git2SemVer_UpdateHostBuildLabel=true --source:https://api.nuget.org/v3/index.json"
            logging = DotnetBuildStep.Verbosity.Normal
        }
        dotnetPack {
            name = "Pack Net2HassMqtt"
            id = "Pack"
            projects = "Net2HassMqtt/Net2HassMqtt.csproj"
            configuration = "Release"
            outputDir = "Output"
            skipBuild = true
            args = "--no-restore"
            logging = DotnetPackStep.Verbosity.Normal
        }
    }

    triggers {
        vcs {
        }
    }

    features {
        perfmon {
        }
    }
})
