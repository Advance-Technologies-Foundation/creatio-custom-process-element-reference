# One arithmetic element, four task implementations

This reference extends the text formatter with a selectable family of native process user tasks. The designer toolbox has one **Arithmetic** entry. Its operation selector changes the actual task schema and loads a different Classic parameter-page schema. Execution does not dispatch through an operation switch in C#.

| Operation | Backend task | Separate parameter page | Inputs |
|---|---|---|---|
| Add | UsrAddNumbersUserTask | UsrAddNumbersPropertiesPage | FirstAddend, SecondAddend |
| Subtract | UsrSubtractNumbersUserTask | UsrSubtractNumbersPropertiesPage | Minuend, Subtrahend |
| Multiply | UsrMultiplyNumbersUserTask | UsrMultiplyNumbersPropertiesPage | Multiplicand, Multiplier |
| Divide | UsrDivideNumbersUserTask | UsrDivideNumbersPropertiesPage | Dividend, Divisor |

Each task has two explicitly **In** numeric parameters and three explicitly **Out** parameters: `Result`, `IsError`, and unlimited-length `ErrorMessage`. Each page inherits `UsrArithmeticPropertiesPage`, which owns only the shared selector and operation-change behavior. The child page owns its MAPPING fields and operation-specific help.

## Runtime structure

Each task resets its outputs, resolves its own typed handler from `UsrCustomProcessElementApp.Instance`, and maps `ErrorOr<decimal>` into the three outputs. Expected errors complete normally, allowing the process to branch on `IsError`. Divide rejects a zero divisor. All handlers translate decimal overflow into a business error. Unexpected exceptions are logged and exposed through a generic error message.

The four interfaces and handlers live under `Files/src/cs/Arithmetic`. No task constructs its own handler. Unit tests inherit each real task, expose `InternalExecute` through `TestExecute`, set inherited input properties, and assert inherited output properties. They do not invoke protected methods through reflection.

Creatio's selected numeric parameter type is `Float2`; the generated server properties are decimal. The installed Clio process-model generator emits `System.Single` for that process parameter type. Integration tests use the generated models unchanged and exact binary-representable examples. This is not a claim of arbitrary decimal precision across that client model boundary.

## How switching works

1. The selected operation maps to a known registered user-task diagram type.
2. The page asks `parentSchema.canRemoveElements` whether replacing the element would break output references. If references prevent replacement, it keeps the current operation and explains the constraint.
3. A confirmation explains that switching replaces the task and clears its configured inputs. Cancel keeps the current operation.
4. The page broadcasts the native `ChangeElementType` message with the current element UId, `type: "userTask"`, and the chosen `userTaskType`.
5. The native designer replaces the element, reconnects its flows, selects it, and loads that task's parameter page through `FK11`.

This is a **design-time** selection saved into the process, not runtime routing. It intentionally follows the native file-processing task family. Operation-specific parameter identities are different; do not silently copy old input values into differently named parameters. Configure the operation before downstream output mappings.

The message must use `Terrasoft.MessageMode.BROADCAST`, matching the designer subscriber. A PTP declaration displays the selector and confirmation but does not perform the replacement. Initialize the selector's `Terrasoft.Collection` before loading choices.

## One toolbox entry

All four task schemas are registered in `SysProcessUserTask`, because the native designer must know each replacement type. All four remain active (`UsageType = General`); the secondary tasks are not marked obsolete.

`Files/descriptor.json` loads `src/js/arithmetic-designer-bootstrap.js`. This small Ext override extends `ProcessSchemaDesignerViewModelNew.getExcludedMenuItems` with the three alternate task names, preserving the platform's existing exclusions. The native file-processing family uses that same list for its alternate tasks. As a result, only Add's **Arithmetic** caption appears in the toolbox and replacement menu; the shared selector can still choose all four registered types.

This depends on a Classic designer implementation method and needs a browser regression check on a Creatio upgrade. It is not a documented cross-version public API. Do not replace the whole native designer or edit platform source.

## Registration and source delivery

`SqlScripts/UsrRegisterArithmeticPostgreSql` and `SqlScripts/UsrRegisterArithmeticMsSql` contain idempotent after-package registration scripts. Their database-engine descriptors are PostgreSQL `2` and MSSQL `0`. PostgreSQL execution is verified in the retained lab; MSSQL is authored but not executed against a SQL Server environment.

The tasks were created with the existing `create-user-task` MCP tool, including explicit parameter directions. The package-owned pages, FK11 links, icons, and SQL were then authored in the workspace. The current tool's Text option does not expose unlimited text: the reference uses the platform's MaxSizeText data-value-type identity for `ErrorMessage` in metadata.

`scripts/build-arithmetic-reference.cjs` is the fixture authoring helper. It requires the four Clio-created task schemas, preserves their schema and parameter identities, and writes the task bodies, service files, pages, icons and registration SQL. Do not rerun it after customizing those generated fixture files unless overwriting those customizations is intended.

In FSM, export browser-authored changes with `pkg-to-file-system` before the next `pkg-to-db`. New task C# needs native source generation/compilation. Client-page-only corrections need metadata synchronization and a fresh designer session, not a full C# compilation.

## Examples and testing

The package contains four executable acceptance processes: `UsrArithmetic_Add`, `UsrArithmetic_Subtract`, `UsrArithmetic_Multiply`, and `UsrArithmetic_Divide`. Each exposes five process parameters and maps both inputs and all three outputs. Their typed integration models are generated by Clio.

`UsrArithmetic_Selector` is a separate retained demonstration for switching pages. It intentionally has no downstream output mappings, so the native replacement guard permits operation changes. It is not one of the five-property integration fixtures.

Run `pwsh ./scripts/Test-Reference.ps1 -EnvironmentName <environment>` from the workspace. This runs both examples: 27 unit cases and 10 live integration cases currently pass, including 13 arithmetic unit cases and five arithmetic live cases. The tests use the existing Clio-scaffolded projects.

Browser acceptance additionally checks one toolbox entry, distinct page schemas, switching, cancel behavior, input reset, save/reopen, and protection of existing output mappings. Automated C# tests alone cannot establish these designer behaviors. See the lab record for the actually completed browser checks.

For general test-project setup, use Clio's published `integration-testing` guidance. This document owns only the custom-element-specific pattern.

## Verification boundary

The reference targets Creatio 10.1.585.0, .NET 8, PostgreSQL and its BPMN designer. Installation into a second fresh environment with FSM off is verified, including packaged PostgreSQL registration, the toolbox and division panel, and all ten live process tests; see [clean-install evidence](clean-install-verification.md). SQL Server installation, DCM, older designers and localized labels remain unverified. This is a published reference pattern; it does not implement new Clio MCP primitives.
