# PostgreSQL installation verification

## Version 0.1.0: fresh non-FSM installation

On 2026-09-17 a new, isolated Creatio 10.1.585.0 / .NET 8 / PostgreSQL instance was provisioned. FSM was confirmed off before and after installing the existing package archive through `clio push-pkg`. No workspace was linked.

Tested archive SHA-256: `49CCFDEAFB7D52DAA9A7ADF4FF6FB2311953E57EDB8A3EE314F6FE1CC89D9EFC`.

Before installation, none of the five task schema UIds existed in SysProcessUserTask. The normal installer logged execution of `UsrRegisterArithmeticPostgreSql` and `UsrRegisterFormatTextPostgreSql`, built the configuration, and restarted the application. After installation, each task had exactly one registration. Verification used read-only SQL; no manual registration INSERT was applied.

All ten live integration tests passed against the new instance. Browser verification showed Format text and a single Arithmetic toolbox entry, and the shipped selector process loaded its division page with dividend 10 and divisor 4. No separate compile, ClioGate installation, or source synchronization was needed.

This proves PostgreSQL package installation for the stated build. SQL Server remains untested. Full operation switching and output-reference guards were tested on the original development instance; this clean-install run verified the shipped division page rather than repeating the full UI suite.

## Version 0.2.0: retained-lab upgrade

Later on 2026-09-17, an exported-metadata regression was corrected: Text and Prefix now explicitly declare In, and FormattedText explicitly declares Out. Their task and parameter UIds are unchanged. IsError and unlimited-text ErrorMessage remain Out. All four arithmetic tasks already had explicit directions.

The corrected package was compressed as `UsrCustomProcessElement.gz` and installed using `clio push-pkg` into the same retained non-FSM PostgreSQL instance. The native installer compiled and restarted successfully. Archive SHA-256: `6439880B73DC676FE3822AE71C5681AF4A00D496B97D26D80D9D675F8F6FCD5F`.

Read-only database inspection confirmed the installed text task's five explicit directions and unlimited ErrorMessage type. Each of the five tasks still had exactly one registration. The retained FSM workspace passed 27 unit cases and 10 live process cases after the direction correction; the same test runner also passed 27 unit cases and all 10 live cases targeting the upgraded non-FSM instance. Unit tests run locally using the workspace's native generated sources and matching platform dependencies.

The environment-independent `scripts/Test-PackageContract.ps1` verifies all five exported task contracts. It fails against the original 0.1.0 text metadata and passes against 0.2.0. It also runs in GitHub CI, without platform binaries or credentials.

Browser verification after the upgrade opened the saved text process and showed its Text and Prefix process mappings. The text output selector offered only Error message and Formatted text from the task; Text and Prefix were excluded, and Boolean IsError was correctly filtered out for a text destination. The selector was cancelled without changing mappings.

This is upgrade evidence, not a second clean-install claim. Both retained instances were kept. SQL Server runtime validation remains outstanding.
