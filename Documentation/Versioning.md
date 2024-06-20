![](/Documentation/Images/Net2HassMqtt_banner_820x70.png)

# Versioning

## Contents

* [The Need](#the-need)
* [Offical (build system) builds](#offical-build-system-builds)
* [Uncontrolled builds](#uncontrolled-builds)

## Background

This project's versioning is designed to achive two main goals:

* [Semmantic Versioning](https://semver.org/) _release_ versioning.
* Clearly indicate any "uncontrolled" builds. That is, those not made on the build system.

Semmantic Versioning is a release versioning standard. 
It defines how the version bumps between releases and after a release. 
To be clear this project defines a release as:

> [!Important]
> A release is when an RC is pubished as a public release (NuGet).

So while there may be (an usually will be) many 1.2.3 builds (each with a unique build number), 
there will not be any more 1.2.3 builds after a release is made and the next release will be, my semver definition, either 1.2.4, 1.3.0, or 2.0.0.

> [!Important]
> Only builds made on the build system matter.
> Builds made on any uncontrolled host (like a dev box) cannot have automatic unique/traceable versioning.

Automatic unique/traceable versioning off uncontrolled boxes is not possible as:

* It may be a build of uncommitted code that may never be commited. Changes may be reverted and new changes made.
* There may be multiple multiple developers making builds from the same branch.
* A developer may rebuild the same code many times while making changes to their environment.
* ...

So, the project's approach is:

* A build, from an uncontrolled build host, will have, regardless of branch, a pre-release version with the pre-release meta-data "Uncontrolled" and a build number of "U".
* A build, from the build system, will have a unique, build number. The build number will be unique regardless of branch, rebuilds, or if building from a prior release.
* Use [Semmantic Versioning](https://semver.org/).

## Build numbers

The project is built on NuGet's build system which does not provide a build number but one can be constructed from a run count and a count of reruns.
This gives a rather unconventional build number format:

```
<run_count>-<rerun_count>
```

e.g:

```
1234-1
1234-2
```

Typically the rerun count will be "1".


## Early development builds

All builds prior to the first release (1.0.0) are considered unstable and have the pre-release tag `Alpha`
regardless of branch or if built on the build system or a dev box.

Formating:

```
[major].[minor].[patch]-Alpha			                    // Product version
[major].[minor].[patch]-Alpha                               // NuGet package version
[major].[minor].[patch]-Alpha+[BuildNumber].[GitSha1Short]  // informational version
```

## Offical (build system) builds

### Release branch builds

Each build on a release branch is a release candidate (RC).
When a RC build is chosen to be the release its commit is tagged. 
The released build may not be most recent build.

RC/release version formating:
````
[major].[minor].[patch]                               // Product version
[major].[minor].[patch]                               // Nuget package version
[major].[minor].[patch]+[BuildNumber].[GitSha1Short]  // Informational version
````

### Development/feature branch builds

Builds not on a relese branch are pre-release builds with the format:

````
[major].[minor].[patch]-Beta.[BuildNumber]                 // Product version
[major].[minor].[patch]-Beta.[BuildNumber]                 // NuGet package version
[major].[minor].[patch]-Beta+[BuildNumber].[GitSha1Short]  // informational version
````

### Uncontrolled builds

An uncontrolled build is any build not made on the build system. e.g: "on your box".
These are inherently uncontrolled builds and the build number will always have a pre-release version number with "-Uncontrolled" suffix and
the build number will always be "U".

Regardless of the branch the version format will be:

````
[major].[minor].[patch]-Uncontrolled.<n>                          // Product version
[major].[minor].[patch]-Uncontrolled.<n>                          // NuGet package version
[major].[minor].[patch]-Uncontrolled.<n>+U.<branch>.<short-sha>   // informational version
````

The `.n` pre-release suffix (e.g: .7) is a number that GitVersion adds.
It is not reliable and should be ignored.

## Examples

| Version                            | Build         | Branch      | Use                                   |
|:---------------------------------- |  :---:        |   :---:     |:-------------------------------------- |
| `1.2.3`                            | Controlled    | release/... | NuGet package version                  |
| `1.2.3+567-1.7f32d0`               | Controlled    | release/... | Informational version                  |
| `1.2.3-Beta.<n>`                   | Controlled    | feature/... | NuGet package version                  |
| `1.2.3-Beta.n+567-1.7f32d0`        | Controlled    | feature/... | Informational version                  |
| `1.2.3-Uncontrolled.<n>`           | Uncontrolled  | any         | NuGet package version                  |
| `1.2.3-Uncontrolled.<n>+U.feature-mybranch.7f32d0` | Uncontrolled | any | Informational version           |

The `.n` pre-release suffix (e.g: .7) is a number that GitVersion adds.
It is not reliable and should be ignored.