# Validation record

Verified on 2026-09-17 using Creatio 10.1.585.0, .NET 8, PostgreSQL, English captions, and Clio 8.1.0.130.

## Evidence and provenance

The original investigation used a disposable FSM development environment and a second fresh instance with FSM disabled. No customer data was used. Source research began with `EmailTemplateUserTaskPropertiesPage`, then followed `ProcessFlowElementPropertiesPage`, parameter mapping behavior, and the native file-processing task family's `ChangeElementType` mechanism. The Classic package sources supplied the panel pattern; the surrounding modern designer does not make this a Freedom UI page.

The checked-in task schemas, pages, handlers, SQL, and typed process models are the tested artifacts. The public snapshot omits local configuration, raw investigative output, and platform binaries.

## Completed checks

- 27 NUnit/FluentAssertions unit cases passed: 14 text and 13 arithmetic cases. Tests use subclasses to expose protected execution, inherited parameters, and the real app's DI container; no reflection invocation.
- 10 live integration cases passed: five text and five arithmetic cases. Typed models invoke saved processes through ATF. All process inputs and outputs are explicitly exposed and mapped.
- Browser: text constants and parameter mappings persisted across save/reopen. Explicit In/Out directions separated native mapping choices.
- Browser: one Arithmetic toolbox entry; Add, Divide, Subtract, Multiply, Add, Divide switching loaded distinct page schemas. Saved division inputs 10 and 4 survived reopening. Cancel preserved inputs. Accepted replacement cleared operation-specific inputs. Existing output references prevented replacement and restored the selector.
- Division by zero and blank text return IsError and ErrorMessage while completing normally. Unit cases include overflow, unexpected exceptions, and recovery clearing previous outputs.
- Fresh non-FSM installation succeeded without manual registration writes; see [installation record](clean-install-verification.md).

## Discoveries that changed the implementation

- Copying LocalizableString parameter metadata into plain text logic caused a compile failure; plain Text was required.
- Unset direction behaved as Variable/bidirectional. Set In/Out explicitly; Resulting alone is insufficient.
- PTP declaration of ChangeElementType showed the selector but did not switch tasks. The subscriber requires BROADCAST.
- Restoring the selector before dialog dismissal was overwritten by the pending control event. Restore it in the dismissal callback.
- Marking secondary tasks inactive is not the mechanism for a single toolbox entry. Keep them active and registered, and extend the native exclusion list while preserving existing entries.
- Native generated companions are necessary for inherited task parameter properties. Do not invent test stubs for missing native generation.

## Limits

SQL Server scripts are authored but not executed. DCM, asynchronous waiting, older Creatio versions, and multilingual captions remain unverified. The toolbox exclusion override is an internal designer extension and requires upgrade regression testing. Float2 generates decimal server properties but Single in the tested Clio process models; the live cases do not establish arbitrary decimal precision across that boundary.

Existing scaffold build warnings included optional assembly references, a System.Text.Json 8.0.0 advisory, and CreatioSDK framework compatibility. Passing tests are not a claim of a warning-free dependency stack.
