// One-time fixture authoring helper. Existing schema IDs are preserved on rerun.
const fs = require('node:fs');
const path = require('node:path');
const {randomUUID} = require('node:crypto');
const root = path.resolve(__dirname, '..');
const pkg = path.join(root, 'packages/UsrCustomProcessElement');
// This historical seed predates the DI/ErrorOr contract. Do not overwrite the evolved reference.
const currentMetadata = path.join(pkg, 'Schemas/UsrFormatTextUserTask/metadata.json');
if (fs.existsSync(currentMetadata) && fs.readFileSync(currentMetadata, 'utf8').includes('ErrorMessage')) {
  throw new Error('Historical seed only. Maintain the current schema sources and use Clio to update parameters.');
}
const write = (p, s) => {fs.mkdirSync(path.dirname(p), {recursive:true}); fs.writeFileSync(p,s);};
const json = (p, o) => write(p, JSON.stringify(o,null,2)+'\n');
const idsPath = path.join(root,'assets/schema-ids.json');
const ids = fs.existsSync(idsPath) ? JSON.parse(fs.readFileSync(idsPath)) : {
  task:randomUUID(), page:randomUUID(), input:randomUUID(), prefix:randomUUID(), output:randomUUID(), sql:randomUUID()
};
json(idsPath,ids);
const packageDescriptor = JSON.parse(fs.readFileSync(path.join(pkg,'descriptor.json')));
const packageId = packageDescriptor.Descriptor.UId;
packageDescriptor.Descriptor.DependsOn = [{UId:'29996b0e-2b19-27a8-dab4-67fbc00db573',Name:'CrtProcessDesigner',PackageVersion:'7.8.0'}];
json(path.join(pkg,'descriptor.json'),packageDescriptor);
const stamp = `/Date(${Date.now()})/`;
const taskName = 'UsrFormatTextUserTask';
const pageName = 'UsrFormatTextUserTaskPropertiesPage';
const taskDir = path.join(pkg,'Schemas',taskName);
const pageDir = path.join(pkg,'Schemas',pageName);
json(path.join(taskDir,'descriptor.json'),{Descriptor:{UId:ids.task,Name:taskName,ModifiedOnUtc:stamp,ManagerName:'ProcessUserTaskSchemaManager',Caption:'Format text'}});
json(path.join(taskDir,'properties.json'),{Properties:{CreatedInVersion:'10.1.585.0',IsInteractive:'False',OptionalProperties:'{}'}});
const parameters = [['Text',ids.input],['Prefix',ids.prefix],['FormattedText',ids.output]].map(([name,uid])=>({
  BL1:'Terrasoft.Core.Process.ProcessSchemaParameter',UId:uid,A2:name,A3:ids.task,A4:ids.task,
  L1:'8b3f29bb-ea14-4ce5-a5c5-293a929b6ba2',L8:{},L11:true,L12:name==='FormattedText'?1:0,...(name==='FormattedText'?{L13:true}:{})
}));
const body = 'FormattedText = (Prefix ?? string.Empty) + (Text ?? string.Empty).Trim();\nreturn true;';
json(path.join(taskDir,'metadata.json'),{MetaData:{Schema:{ManagerName:'ProcessUserTaskSchemaManager',UId:ids.task,A2:taskName,A5:packageId,B1:[],B2:[],B3:[],B6:packageId,B8:'10.1.585.0',FJ1:parameters,FK1:body,FK2:1,FK4:true,FK7:true,FK11:ids.page,FK12:'#4F75C2',FK13:true,FK16:true,FK17:false}}});
write(path.join(taskDir,taskName+'.cs'),`namespace Terrasoft.Core.Process.Configuration\n{\n    /// <summary>Trims the input text and prepends the configured prefix.</summary>\n    public partial class ${taskName}\n    {\n        /// <summary>Produces the output synchronously and completes the element.</summary>\n        protected override bool InternalExecute(ProcessExecutingContext context)\n        {\n            FormattedText = (Prefix ?? string.Empty) + (Text ?? string.Empty).Trim();\n            return true;\n        }\n    }\n}\n`);
const parentId='0f347363-31e5-4222-a82e-dcfeda34cbb6';
json(path.join(pageDir,'descriptor.json'),{Descriptor:{UId:ids.page,Name:pageName,ModifiedOnUtc:stamp,Parent:{UId:parentId,Name:'ProcessFlowElementPropertiesPage'},ManagerName:'ClientUnitSchemaManager',Caption:'Format text parameters'}});
json(path.join(pageDir,'properties.json'),{Properties:{CreatedInVersion:'10.1.585.0',OptionalProperties:'{}',SchemaType:'EditViewModelSchema'}});
write(path.join(pageDir,'metadata.json'),`= MetaData.Schema.UId "${ids.page}"\n= MetaData.Schema.A2 "${pageName}"\n= MetaData.Schema.A5 "${packageId}"\n= MetaData.Schema.HD1 "${parentId}"\n`);
write(path.join(pageDir,pageName+'.js'),`/** Parent: ProcessFlowElementPropertiesPage. Field names match user-task parameter names. */
define("${pageName}", ["terrasoft"], function(Terrasoft) {
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
`);
const images = {
SmallSvgImage:'<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 16 16"><path fill="#4F75C2" d="M2 2h12v3h-1V3H9v10h2v1H5v-1h2V3H3v2H2z"/></svg>',
LargeSvgImage:'<svg xmlns="http://www.w3.org/2000/svg" width="69" height="55" viewBox="0 0 69 55"><rect width="69" height="55" rx="5" fill="#4F75C2"/><path fill="#fff" d="M23 14h23v7h-3v-4h-7v22h5v3H28v-3h5V17h-7v4h-3z"/></svg>',
TitleSvgImage:'<svg xmlns="http://www.w3.org/2000/svg" width="42" height="42" viewBox="0 0 42 42"><rect width="42" height="42" rx="3" fill="#fff"/><path fill="#4F75C2" d="M10 10h22v6h-3v-3h-6v17h4v3H15v-3h5V13h-7v3h-3z"/></svg>'
};
const entries = ['<Item Name="Caption" Value="Format text" />','<Item Name="Parameters.Text.Caption" Value="Text to format" />','<Item Name="Parameters.Prefix.Caption" Value="Prefix" />','<Item Name="Parameters.FormattedText.Caption" Value="Formatted text" />'];
for(const [name,svg] of Object.entries(images)) {
 write(path.join(root,'assets',name+'.svg'),svg+'\n');
 entries.push(`<Item Name="${name}" Type="Image" ContentType="Data" FileExtension=".svg" Value="${Buffer.from(svg).toString('base64')}" />`);
}
write(path.join(pkg,'Resources',taskName+'.ProcessUserTask','resource.en-US.xml'),`<?xml version="1.0" encoding="utf-8"?>\n<Resources Culture="en-US"><Group Type="String"><Items>\n${entries.join('\n')}\n</Items></Group></Resources>\n`);
write(path.join(pkg,'Resources',pageName+'.ClientUnit','resource.en-US.xml'),'<?xml version="1.0" encoding="utf-8"?>\n<Resources Culture="en-US"><Group Type="String"><Items><Item Name="Caption" Value="Format text parameters" /></Items></Group></Resources>\n');
const sqlName='UsrRegisterFormatTextPostgreSql';
json(path.join(pkg,'SqlScripts',sqlName,'descriptor.json'),{SqlScript:{UId:ids.sql,Name:sqlName,DBEngineType:2,InstallType:1,ModifiedOnUtc:stamp}});
write(path.join(pkg,'SqlScripts',sqlName,sqlName+'.sql'),`-- After-package registration; safe to execute again during an upgrade.\nINSERT INTO "SysProcessUserTask" ("SysUserTaskSchemaUId", "Caption")\nSELECT s."UId", 'Format text'\nFROM "SysSchema" s\nWHERE s."UId" = '${ids.task}'\n  AND NOT EXISTS (SELECT 1 FROM "SysProcessUserTask" t WHERE t."SysUserTaskSchemaUId" = s."UId");\n`);
console.log(JSON.stringify(ids));

