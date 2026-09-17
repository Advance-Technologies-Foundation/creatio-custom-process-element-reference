# Fresh non-FSM installation

On 2026-09-17 a new, isolated Creatio 10.1.585.0 / .NET 8 / PostgreSQL instance was provisioned. FSM was confirmed off before and after installing the existing package archive through `clio push-pkg`. No workspace was linked.

Tested archive SHA-256: `49CCFDEAFB7D52DAA9A7ADF4FF6FB2311953E57EDB8A3EE314F6FE1CC89D9EFC`.

Before installation, none of the five task schema UIds existed in SysProcessUserTask. The normal installer logged execution of `UsrRegisterArithmeticPostgreSql` and `UsrRegisterFormatTextPostgreSql`, built the configuration, and restarted the application. After installation, each task had exactly one registration. Verification used read-only SQL; no manual registration INSERT was applied.

All ten live integration tests passed against the new instance. Browser verification showed Format text and a single Arithmetic toolbox entry, and the shipped selector process loaded its division page with dividend 10 and divisor 4. No separate compile, ClioGate installation, or source synchronization was needed.

This proves PostgreSQL package installation for the stated build. SQL Server remains untested. Full operation switching and output-reference guards were tested on the original development instance; this clean-install run verified the shipped division page rather than repeating the full UI suite.
