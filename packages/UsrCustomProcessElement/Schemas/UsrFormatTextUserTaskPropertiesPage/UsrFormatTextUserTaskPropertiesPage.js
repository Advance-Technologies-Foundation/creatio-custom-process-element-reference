/** Parent: ProcessFlowElementPropertiesPage. Field names match user-task parameter names. */
define("UsrFormatTextUserTaskPropertiesPage", ["terrasoft"], function(Terrasoft) {
    return {
        attributes: {
            "Text": {
                dataValueType: Terrasoft.DataValueType.MAPPING,
                type: Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,
                initMethod: "initPropertySilent",
                doAutoSave: true
            },
            "Prefix": {
                dataValueType: Terrasoft.DataValueType.MAPPING,
                type: Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,
                initMethod: "initPropertySilent",
                doAutoSave: true
            }
        },
        diff: /**SCHEMA_DIFF*/[
            {
                operation: "insert", name: "UserTaskContainer",
                parentName: "EditorsContainer", propertyName: "items",
                values: {itemType: Terrasoft.ViewItemType.GRID_LAYOUT, items: []}
            },
            {
                operation: "insert", name: "Text",
                parentName: "UserTaskContainer", propertyName: "items",
                values: {caption: "Text to format", layout: {column: 0, row: 0, colSpan: 24},
                    controlConfig: {autocomplete: "TextMapping"}, wrapClass: ["top-caption-control"]}
            },
            {
                operation: "insert", name: "Prefix",
                parentName: "UserTaskContainer", propertyName: "items",
                values: {caption: "Prefix", layout: {column: 0, row: 1, colSpan: 24},
                    controlConfig: {autocomplete: "PrefixMapping"}, wrapClass: ["top-caption-control"]}
            },
            {
                operation: "insert", name: "OutputHint",
                parentName: "UserTaskContainer", propertyName: "items",
                values: {itemType: Terrasoft.ViewItemType.LABEL,
                    caption: "Result: Formatted text. Use it as an input in the next process element.",
                    layout: {column: 0, row: 2, colSpan: 24}}
            }
        ]/**SCHEMA_DIFF*/
    };
});
