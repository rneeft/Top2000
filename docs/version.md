# Version
This repository is using [Semver](https://semver.org/) as its version strategy. The current version is set in the [`version.yml`](..\pipelines\version.yml) file. 
## Summary of Semver
It is advised to read the full [Semver](https://semver.org/) documentation but here follows a Summary of it.

A version in semver is build out of 4 parts: `Major` - `Minor` - `Patch` and a `version label`.

An increment in the `Major` section indicates a breaking change compare to the previous. 

An increment in the `Minor` section indicates a backwards compatible added feature. 

An increment in the `Patch` section indicated a bug fix.

The version label indicates the state of change like alpha, beta, prerelease. Since a version without a label indicate a release, this part of the version is optional. 

When a section of the version in incremented, the lower sections are reset to `0`. For example, given version 1.2.3, when adding a featues, the version must be 1.3.0.

## Top2000 version
Every part of a software system could get individual versions. However, considering the size of the Top2000 application only one version is applied to all parts of the system. 

I prefer counting from breaking changes in the client database schema in which case, this is the 6th version.