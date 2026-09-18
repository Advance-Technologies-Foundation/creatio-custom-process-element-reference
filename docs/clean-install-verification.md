# Package installation verification

## Version 0.1.0: fresh non-FSM installation

On 2026-09-17 a new, isolated Creatio 10.1.585.0 / .NET 8 / PostgreSQL instance was provisioned. FSM was confirmed off before and after installing the existing package archive through `clio push-pkg`. No workspace was linked.

Tested archive SHA-256: `49CCFDEAFB7D52DAA9A7ADF4FF6FB2311953E57EDB8A3EE314F6FE1CC89D9EFC`.

Before installation, none of the five task schema UIds existed in SysProcessUserTask. The normal installer logged execution of `UsrRegisterArithmeticPostgreSql` and `UsrRegisterFormatTextPostgreSql`, built the configuration, and restarted the application. After installation, each task had exactly one registration. Verification used read-only SQL; no manual registration INSERT was applied.

All ten live integration tests passed against the new instance. Browser verification showed Format text and a single Arithmetic toolbox entry, and the shipped selector process loaded its division page with dividend 10 and divisor 4. No separate compile, ClioGate installation, or source synchronization was needed.

This run proves PostgreSQL package installation for the stated build. SQL Server was not tested in this run. Full operation switching and output-reference guards were tested on the original development instance; this clean-install run verified the shipped division page rather than repeating the full UI suite.

## Version 0.2.0: retained-lab upgrade

Later on 2026-09-17, an exported-metadata regression was corrected: Text and Prefix now explicitly declare In, and FormattedText explicitly declares Out. Their task and parameter UIds are unchanged. IsError and unlimited-text ErrorMessage remain Out. All four arithmetic tasks already had explicit directions.

The corrected package was compressed as `UsrCustomProcessElement.gz` and installed using `clio push-pkg` into the same retained non-FSM PostgreSQL instance. The native installer compiled and restarted successfully. Archive SHA-256: `6439880B73DC676FE3822AE71C5681AF4A00D496B97D26D80D9D675F8F6FCD5F`.

Read-only database inspection confirmed the installed text task's five explicit directions and unlimited ErrorMessage type. Each of the five tasks still had exactly one registration. The retained FSM workspace passed 27 unit cases and 10 live process cases after the direction correction; the same test runner also passed 27 unit cases and all 10 live cases targeting the upgraded non-FSM instance. Unit tests run locally using the workspace's native generated sources and matching platform dependencies.

The environment-independent `scripts/Test-PackageContract.ps1` verifies all five exported task contracts. It fails against the original 0.1.0 text metadata and passes against 0.2.0. It also runs in GitHub CI, without platform binaries or credentials.

Browser verification after the upgrade opened the saved text process and showed its Text and Prefix process mappings. The text output selector offered only Error message and Formatted text from the task; Text and Prefix were excluded, and Boolean IsError was correctly filtered out for a text destination. The selector was cancelled without changing mappings.

This is upgrade evidence, not a second clean-install claim. Both retained instances were kept. SQL Server was validated subsequently as recorded below.

## Version 0.2.0: fresh SQL Server installation and forced upgrade

On 2026-09-18, Clio 8.1.0.131 deployed Creatio 10.1.784.0 / .NET Framework 4.8.9345.0 against SQL Server 2025 Express 17.0.4085.5. FSM remained off. The database was restored using Windows authentication. Initial readiness failed because the new IIS application-pool identity had no database user. Mapping that identity to the new lab database with db_owner membership enabled application startup and package DDL; no server-wide administrator role was granted. This prerequisite concerns this integrated-authentication deployment, not task registration.

The exact published v0.2.0 archive above was installed through `clio push-pkg`. Before installation, none of the five registrations existed. The native installation executed `UsrRegisterArithmeticMsSql` and `UsrRegisterFormatTextMsSql`, compiled the package and completed successfully. No manual registration INSERT, ClioGate installation, FSM, or separate compilation was used. All ten live process tests passed, including four arithmetic operations, divide-by-zero handling, text formatting, Unicode and blank-input errors.

A local validation copy changed only package version/timestamps and SQL-script timestamps to force native upgrade execution (version 0.2.1, not a published package release). The installer logged both MSSQL scripts as installed again. Readback confirmed exactly one row per task, with identical row IDs and captions. All ten live cases passed again. The installed text-task metadata preserved Text/Prefix as In, the three result parameters as Out, and unlimited-text ErrorMessage.

Browser checks showed one Arithmetic entry plus Format text under User actions. Opening the shipped selector displayed Dividend/Divisor. Switching with the native confirmation displayed the separate Add (First addend/Second addend), Subtract (Minuend/Subtrahend), and Multiply (Multiplicand/Multiplier) pages. These switches were not saved over the imported process. Save/reopen and downstream-reference guards retain their earlier PostgreSQL evidence; this run does not extend those claims to MSSQL.

The additional Clio-tooling probe from clio#1602 also installed its generated MSSQL registration and parameter page. Its test-only cross-package dependency initially assumed `Files/Bin/netstandard`; using the existing `StandalonePackageAssemblyPath` fixed .NET Framework compilation. All five probe integration cases passed. A timestamped upgrade forced its generated registration script to execute again and preserved its row ID and caption. This probe is separate from the unchanged published reference package.

The integration runner now uses the registered IsNetCore value, so .NET Framework requests use the native `/0/` service routes. Its `-IntegrationOnly` option avoids claiming that local .NET 8 unit-test binaries validate the .NET Framework target. Existing local unit evidence remains 27 cases. All environments were retained.
