# Third-party components

The example's original source is MIT licensed. Dependencies retain their own licenses.

- `packages/UsrCustomProcessElement/Files/Libs/ErrorOr.dll`: ErrorOr, assembly version 2.0.1.0, by Amichai Mantinband. [Upstream](https://github.com/amantinband/error-or); license in `licenses/erroror-LICENSE.txt`.
- `packages/UsrCustomProcessElement/Files/Libs/ATF.Repository.dll`: ATF.Repository, assembly version 2.0.1.0, Advance Technologies Foundation. [Upstream](https://github.com/Advance-Technologies-Foundation/repository); license in `licenses/atf-LICENSE.txt`.

NuGet test dependencies are declared in the project files and restored separately. Creatio platform and proprietary test assemblies are not included; obtain matching binaries through your licensed environment and Clio test scaffold. A Creatio installation is required to build native generated schema companions and execute the examples.
