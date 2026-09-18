# Create a custom business process element

> Validated on Creatio 10.1.585.0, .NET 8, PostgreSQL. See [the lab record](lab-record.md) for evidence and limits, and [README](../README.md) for deployment and tests.

A custom process element has four parts: a **user-task schema**, its **C# execution logic**, a **Classic client schema for the designer's parameter panel**, and **package installation artifacts** for the toolbox entry and images. These parts belong in the same custom package, with a dependency on `CrtProcessDesigner`.

The example is **Format text**. It accepts `Text` and `Prefix`, trims the text, prepends the prefix, and exposes `FormattedText`, `IsError`, and unlimited-text `ErrorMessage` for the process. It runs synchronously and does not create an activity or wait for a user.

## 1. Create the package and user task

Create a custom package that depends on `CrtProcessDesigner`. In Configuration, select the package and choose **Add → User task**. Set its code to `UsrFormatTextUserTask` and title to **Format text**. Keep the schema code stable: process elements reference the task's schema identity.

Add these parameters in the user-task designer:

| Code | Title | Type | Direction | Serializable | Resulting |
|---|---|---|---|---|---|
| `Text` | Text to format | Text | In | Yes | No |
| `Prefix` | Prefix | Text | In | Yes | No |
| `FormattedText` | Formatted text | Text | Out | Yes | Yes |
| `IsError` | Is error | Boolean | Out | Yes | Yes |
| `ErrorMessage` | Error message | Unlimited text | Out | Yes | Yes |

The output is a runtime value. Do not add it as an editable input on the configuration panel. `Resulting` exposes the value for downstream use; it is independent of a field being visible on the panel.

Set **direction** explicitly as well: it controls whether the native mapping picker offers a parameter as a source. Leaving it unset defaults to `Variable` (both input and output); marking only `Resulting` does not make the other parameters input-only. In metadata, `L12` is direction: `0 = In`, `1 = Out`, `2 = Variable`; the platform also defines `3 = Internal`. Clio supports named In/Out/Variable updates:

```powershell
clio modify-user-task-parameters UsrFormatTextUserTask -e <environment> --set-direction 'Text=In|Prefix=In|FormattedText=Out'
```

Run from this workspace. The equivalent MCP tool is `modify-user-task-parameters`, with `set-parameter-directions` entries containing `parameter-name` and `direction`. Use this supported operation instead of deleting/recreating parameters, which would change their identities. Its save/build/metadata-sync sequence applied the directions to the existing reference process while preserving mappings.

Use the plain **Text** type deliberately. The native email task's `Subject` uses `LocalizableString`, which generates a different C# property type. Copying that parameter metadata caused a real compile failure on `.Trim()` in this lab. Plain Text uses data-type UId `8b3f29bb-ea14-4ce5-a5c5-293a929b6ba2` and generated `string` properties here.

This reference enables **User task**, **Partial**, and parameter serialization, and keeps the task non-interactive. The **User task** classification places it under **User actions** in the toolbox. That category does not mean execution waits for a human.

The task implementation is in `Schemas/UsrFormatTextUserTask/UsrFormatTextUserTask.cs`. It resets all outputs, resolves `IFormatTextHandler` from the app instance, and maps its `ErrorOr<string>` result. The registered handler owns trimming and prefixing; blank input returns a validation error. Expected errors complete normally with `IsError = true`. The task also logs unexpected exceptions and exposes a generic error message.

Creatio generates the other part of this partial class, including its base class and parameter properties. Do not manually invent those generated declarations to make an incomplete local build pass. Generate the schema source through Creatio and build using the appropriate package workflow.

Returning `true` completes this synchronous element. Returning `false` leaves the element running; that is an interactive/asynchronous lifecycle requiring a deliberate resume or completion mechanism, outside this example.

## 2. Create the parameter panel

Create a **Client module / edit view-model schema** named `UsrFormatTextUserTaskPropertiesPage`, inheriting from **ProcessFlowElementPropertiesPage**. This is a Classic UI schema even when the surrounding user-task schema designer uses Angular.

The native `EmailTemplateUserTaskPropertiesPage` is useful for understanding mapping fields, but its activity and email-specific behavior is unnecessary here. `ProcessFlowElementPropertiesPage` already supplies process-element loading, validation hooks, parameter persistence, and the `EditorsContainer` host.

The JavaScript follows the Classic schema structure:

```javascript
define("UsrFormatTextUserTaskPropertiesPage", ["terrasoft"], function(Terrasoft) {
    return {
        attributes: {
            "Text": {
                dataValueType: Terrasoft.DataValueType.MAPPING,
                type: Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,
                initMethod: "initPropertySilent",
                doAutoSave: true
            }
        },
        diff: [
            {
                operation: "insert",
                name: "UserTaskContainer",
                parentName: "EditorsContainer",
                propertyName: "items",
                values: {
                    itemType: Terrasoft.ViewItemType.GRID_LAYOUT,
                    items: []
                }
            },
            {
                operation: "insert",
                name: "Text",
                parentName: "UserTaskContainer",
                propertyName: "items",
                values: {
                    caption: "Text to format",
                    layout: {column: 0, row: 0, colSpan: 24},
                    controlConfig: {autocomplete: "TextMapping"},
                    wrapClass: ["top-caption-control"]
                }
            }
        ]
    };
});
```

The complete reference adds `Prefix` in the same way and an explanatory output label. This English-only example uses literal panel labels for clarity; a localized product should place them in the client schema's localizable strings and bind `Resources.Strings.<Key>`.

### How the old-style panel works

- `define(...)` is a RequireJS module. It returns a schema configuration; it does not create an Angular component.
- The schema's **parent metadata** supplies inheritance. Naming the parent in a JavaScript comment does not create inheritance.
- `attributes` declares view-model fields. `Text` must exactly match the user-task parameter code. `VIRTUAL_COLUMN` means this is not an entity column.
- `MAPPING` preserves a process binding object rather than just a textbox's string. It lets low-code users select constants, process parameters, or other supported value sources.
- `initPropertySilent` reads the existing parameter value when the panel opens. `doAutoSave` makes the base page save the field using `setMappingValue()` during the designer's normal save lifecycle. It is not a separate database autosave service.
- `diff` inserts controls into the inherited layout. A control's name binds it to the matching view-model attribute. The mapping autocomplete name follows the field name.
- Additional methods can use `this.get(...)`, `this.set(...)`, and `this.callParent(arguments)` under the Classic view-model framework. Do not override the save pipeline for these basic fields.

## 3. Associate the page with the task

Open the user task's properties. Select **Format text parameters** in **Parameters edit page**. This is the setting highlighted in the request's screenshot. It configures the process designer's right-side panel, not an end-user execution page.

In exported metadata the modern Classic page association is `FK11` (`ParametersEditPageSchemaV2UId`). It stores the **client schema UId**, not the `SysSchema.Id` database row key or the page caption. Do not substitute the older `FK3` property.

The client page must be available through the package dependency hierarchy. Keep it in the same package as the task so both travel together. The DCM page property is separate and remains unset in this BPMN example.

### What the generated DesignModeProperty Group means

The generated C# companion annotates parameters with `DesignModeProperty(..., Group = "", ...)`. This `Group` belongs to Creatio's design-time property descriptor system. It groups properties under a declared design-mode group; it does **not** select the process toolbox category, parameter direction, or a container on our custom Classic panel.

The reference leaves parameter groups empty. Native tasks use localized resources such as `Parameters.Account.Group = Connected to` and `Parameters.ActivityCategory.Group = General`. The backend collects nonempty parameter groups and generates matching class-level `DesignModeGroup` declarations automatically. An explicitly empty group skips named grouping; do not assume it becomes General. Author the localized parameter Group value and regenerate; do not hand-edit generated C#. Our custom panel's grouping/layout is authored in its client-schema `diff`. Toolbox registration and User actions classification are separate mechanisms.

## 4. Add the icons

Set **Color**, **Small vector image**, **Large vector image**, and **Title vector image** in the task properties. Use three representations of the same symbol, designed for their different display sizes.

The example's editable SVG originals live in `assets/`. The package owns the image bytes in `Resources/UsrFormatTextUserTask.ProcessUserTask/resource.en-US.xml`, under the resource keys:

| Resource key | Purpose |
|---|---|
| `SmallSvgImage` | Toolbox representation |
| `LargeSvgImage` | Diagram element |
| `TitleSvgImage` | Parameter panel heading |

An SVG resource is an `Item` with `Type="Image"`, `ContentType="Data"`, `FileExtension=".svg"`, and a base64-encoded `Value`. Merely adding an SVG to a package directory does not associate it with the task.

The example uses 16×16, 69×55, and 42×42 canvases respectively, with color `#4F75C2`. DCM has its own image slot and is not claimed as validated here.

## 5. Register the toolbox entry in the package

The toolbox reads `SysProcessUserTask`. Include registration in a package SQL script, rather than running an undocumented one-time database edit.

For PostgreSQL, use `INSERT ... SELECT` and guard against an existing task UId:

```sql
INSERT INTO "SysProcessUserTask" ("SysUserTaskSchemaUId", "Caption")
SELECT s."UId", 'Format text'
FROM "SysSchema" s
WHERE s."UId" = '<your-user-task-schema-uid>'
  AND NOT EXISTS (
    SELECT 1 FROM "SysProcessUserTask" t
    WHERE t."SysUserTaskSchemaUId" = s."UId"
  );
```

The package's SQL descriptor uses `DBEngineType: 2` for PostgreSQL and `InstallType: 1` for **After package**. Running after schema installation is essential because the schema must exist before registration. The example resolves by the fixed exported schema UId and guards against duplicates on upgrades.

This script intentionally preserves an existing caption. If you later rename the toolbox entry, include an explicit upgrade for the existing registration. A multilingual product also needs a deliberate localization strategy for the toolbox caption.

The Academy page's displayed PostgreSQL `VALUES (SELECT ...)` example is not a valid two-column insertion pattern as shown. Use the executable `INSERT ... SELECT` form above. Do not copy vendor sample SQL without running it against the target database engine.

The package includes PostgreSQL and MSSQL registration scripts. Both have native install and forced re-execution evidence on the builds listed in [installation verification](clean-install-verification.md). Oracle requires separate implementation and validation.

## 6. Verify the complete low-code experience

Registration and successful compilation are separate from acceptance. Verify all of the following:

1. Start a fresh designer session after deployment and confirm **Format text** appears under **User actions** with its icon.
2. Add it between a start and end event. Confirm the diagram icon and custom parameter panel.
3. Enter a constant in `Prefix`, map `Text` to a process input, then save the process.
4. Close and reopen the process. Confirm both bindings survived.
5. Map `FormattedText` into a process output or another element's input.
6. Execute with `Text = "  Creatio  "` and `Prefix = "Hello, "`; require **Hello, Creatio** as the actual output.
7. Repeat with blank input: require IsError=true, the validation message, and cleared FormattedText. Repeat a successful input afterward to verify cleared error outputs.
8. Reinstall or resynchronize the package and confirm the registration count remains one.

The original panel constant/mapping and save/reopen checks passed in the retained lab. The current process exposes both Text and Prefix as inputs and maps all three outputs. Fourteen text unit cases and five live text cases pass, including blank-input errors and Unicode text. See [the arithmetic example](arithmetic-element.md) for the separate operation pages and its additional tests.

See the checked-in Classic parameter-page schema for the complete panel.

## Troubleshooting

| Symptom | Check |
|---|---|
| Task absent from toolbox | Package SQL ran after schema installation; correct task **UId**; check User actions; start a fresh designer session |
| Generic panel or missing custom panel | Parameters edit page points to the client schema's UId; correct parent and dependency hierarchy; client schema loaded |
| Editor value disappears on reopen | Attribute code matches task parameter; use MAPPING and inherited save path; save the process, not just the user-task schema |
| `.Trim()` compilation error | Parameter is plain Text, not LocalizableString copied from an email task |
| `InternalExecute` has no suitable override in a local build | Native generated companion is missing; generate/compile through Creatio before local unit tests |
| Browser change disappears after sync | Export with `pkg-to-file-system` before importing older workspace files with `pkg-to-db` |
| ATF reports `Unexpected parse response exception` | Normalize application URL with `TrimEnd('/')`; ATF 2.0.3.5 concatenates its own leading-slash route |

The authored files are the reusable template. `scripts/build-reference.cjs` records how the first fixture was assembled; it is not an upgrade/deployment command and should not be rerun over browser-edited metadata. Prefer native designers for future schema edits, then export them.

## References

- [Creatio Academy: user tasks, parameters, icons, registration and execution lifecycle](https://academy.creatio.com/docs/developer/development_tools/creatio_ide/user_task/overview).
- Reference package source: the `Schemas`, `Resources`, and `SqlScripts` directories in this repository.
- Research provenance and version boundary: `lab-record.md`.
