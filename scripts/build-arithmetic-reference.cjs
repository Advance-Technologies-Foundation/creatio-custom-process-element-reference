// Enrich the four schemas created with clio create-user-task. Stable identities are retained.
const fs = require('node:fs');
const path = require('node:path');
const {randomUUID} = require('node:crypto');
const root = path.resolve(__dirname, '..');
const pkg = path.join(root, 'packages/UsrCustomProcessElement');
const write = (p,s) => { fs.mkdirSync(path.dirname(p),{recursive:true}); fs.writeFileSync(p,s); };
const json = (p,o) => write(p,JSON.stringify(o,null,2)+'\n');
const read = p => JSON.parse(fs.readFileSync(p,'utf8').replace(/^\uFEFF/,''));
const idPath=path.join(root,'assets/arithmetic-ids.json');
const ids=fs.existsSync(idPath)?read(idPath):{basePage:randomUUID(),sqlPg:randomUUID(),sqlMs:randomUUID(),pages:{}};
const operations=[['Add','FirstAddend','SecondAddend','First addend','Second addend','+'],['Subtract','Minuend','Subtrahend','Minuend','Subtrahend','-'],['Multiply','Multiplicand','Multiplier','Multiplicand','Multiplier','*'],['Divide','Dividend','Divisor','Dividend','Divisor','/']];
const packageId=read(path.join(pkg,'descriptor.json')).Descriptor.UId;
const stamp=`/Date(${Date.now()})/`;
const baseName='UsrArithmeticPropertiesPage';
function page(name,uid,parentName,parentId,code) {
 const dir=path.join(pkg,'Schemas',name);
 json(path.join(dir,'descriptor.json'),{Descriptor:{UId:uid,Name:name,ModifiedOnUtc:stamp,Parent:{UId:parentId,Name:parentName},ManagerName:'ClientUnitSchemaManager',Caption:name}});
 json(path.join(dir,'properties.json'),{Properties:{CreatedInVersion:'10.1.585.0',OptionalProperties:'{}',SchemaType:'EditViewModelSchema'}});
 write(path.join(dir,'metadata.json'),`= MetaData.Schema.UId "${uid}"\n= MetaData.Schema.A2 "${name}"\n= MetaData.Schema.A5 "${packageId}"\n= MetaData.Schema.HD1 "${parentId}"\n`);
 write(path.join(dir,name+'.js'),code);
 write(path.join(pkg,'Resources',name+'.ClientUnit','resource.en-US.xml'),`<Resources Culture="en-US"><Group Type="String"><Items><Item Name="Caption" Value="Arithmetic parameters" /></Items></Group></Resources>`);
}
page(baseName,ids.basePage,'ProcessFlowElementPropertiesPage','0f347363-31e5-4222-a82e-dcfeda34cbb6',`/** Shared selector. Concrete pages define their own input parameters. */
define("${baseName}", ["terrasoft"], function(Terrasoft) {
 return {
  messages: {"ChangeElementType": {mode: Terrasoft.MessageMode.BROADCAST, direction: Terrasoft.MessageDirectionType.PUBLISH}},
  attributes: {
   "Operation": {dataValueType: Terrasoft.DataValueType.ENUM, type: Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN},
   "OperationList": {dataValueType: Terrasoft.DataValueType.COLLECTION, type: Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN}
  },
  methods: {
   onElementDataLoad: function(element, callback, scope) {
    this.callParent([element, function() {
     this.set("OperationList", Ext.create("Terrasoft.Collection"));
     this.set("Operation", {value:this.getOperationCode(), displayValue:this.getOperationCode()});
     callback.call(scope || this);
    },this]);
   },
   prepareOperationList: function(filter,list) {
    if (!list) { return; }
    list.clear();
    list.loadAll({Add:{value:"Add",displayValue:"Add"},Subtract:{value:"Subtract",displayValue:"Subtract"},Multiply:{value:"Multiply",displayValue:"Multiply"},Divide:{value:"Divide",displayValue:"Divide"}});
   },
   onOperationChanged: function(value) {
    if (!value || value.value === this.getOperationCode()) { return; }
    const types = {Add:"usrAddNumbersUserTask",Subtract:"usrSubtractNumbersUserTask",Multiply:"usrMultiplyNumbersUserTask",Divide:"usrDivideNumbersUserTask"};
    const nextType=types[value.value];
    if (!nextType) { return; }
    const element=this.get("ProcessElement");
    const check=element.parentSchema.canRemoveElements([element.name]);
    const revert=()=>this.set("Operation",{value:this.getOperationCode(),displayValue:this.getOperationCode()});
    if (!check.canRemove) {
     this.showConfirmationDialog("This operation has output references. Remove those references before changing the operation.", function() { revert(); }, [Terrasoft.MessageBoxButtons.OK]);
     return;
    }
    this.showConfirmationDialog("Changing the operation replaces this task and clears its configured inputs. Continue?",function(answer) {
     if(answer === Terrasoft.MessageBoxButtons.YES.returnCode) {
      this.sandbox.publish("ChangeElementType",{id:this.get("uId"),type:"userTask",userTaskType:nextType});
     } else { revert(); }
    },[Terrasoft.MessageBoxButtons.YES,Terrasoft.MessageBoxButtons.NO]);
   }
  },
  diff: /**SCHEMA_DIFF*/[
   {operation:"insert",name:"ArithmeticContainer",parentName:"EditorsContainer",propertyName:"items",values:{itemType:Terrasoft.ViewItemType.GRID_LAYOUT,items:[]}},
   {operation:"insert",name:"Operation",parentName:"ArithmeticContainer",propertyName:"items",values:{caption:"Operation",contentType:Terrasoft.ContentType.ENUM,layout:{column:0,row:0,colSpan:24},controlConfig:{className:"Terrasoft.ComboBoxEdit",list:{bindTo:"OperationList"},prepareList:{bindTo:"prepareOperationList"},change:{bindTo:"onOperationChanged"}},wrapClass:["top-caption-control"]}}
  ]/**SCHEMA_DIFF*/
 };
});\n`);
const registry=[];
for(const [op,a,b,captionA,captionB,symbol] of operations) {
 const name=`Usr${op}NumbersUserTask`, pageName=`Usr${op}NumbersPropertiesPage`;
 ids.pages[op] ||= randomUUID();
 const dir=path.join(pkg,'Schemas',name), md=read(path.join(dir,'metadata.json')), s=md.MetaData.Schema;
 registry.push({op,name,uid:s.UId,page:pageName,pageUId:ids.pages[op]});
 const body=`Result = 0m;\nIsError = false;\nErrorMessage = string.Empty;\nvar app = UsrCustomProcessElementApp.UsrCustomProcessElementApp.Instance;\ntry {\n    var result = app.GetRequiredService<UsrCustomProcessElementApp.Arithmetic.I${op}NumbersHandler>().Calculate(${a}, ${b});\n    if (result.IsError) {\n        IsError = true;\n        ErrorMessage = string.Join(System.Environment.NewLine, System.Linq.Enumerable.Select(result.Errors, error => error.Description));\n    } else { Result = result.Value; }\n} catch (System.Exception exception) {\n    IsError = true;\n    ErrorMessage = "Arithmetic operation failed unexpectedly.";\n    app.GetRequiredService<global::Common.Logging.ILog>().Error("${op} numbers task failed.", exception);\n}\nreturn true;`;
 Object.assign(s,{FK1:body,FK2:1,FK11:ids.pages[op],FK4:true,FK7:true,FK12:'#287A68',FK13:true,FK16:true,FK17:false});
 for(const p of s.FJ1) { p.L11=true; p.L13=p.L12===1; if(p.A2==='ErrorMessage') p.L1='c0f04627-4620-4bc0-84e5-9419dc8516b1'; }
 json(path.join(dir,'metadata.json'),md);
 const desc=read(path.join(dir,'descriptor.json')); desc.Descriptor.ModifiedOnUtc=stamp; json(path.join(dir,'descriptor.json'),desc);
 write(path.join(dir,name+'.cs'),`namespace Terrasoft.Core.Process.Configuration {\n /// <summary>Maps ${op.toLowerCase()} inputs to its DI service and returns result/error outputs.</summary>\n public partial class ${name} {\n  /// <summary>Completes synchronously, representing business failures through process outputs.</summary>\n  protected override bool InternalExecute(ProcessExecutingContext context) {\n${body}\n  }\n }\n}\n`);
 const serviceDir=path.join(pkg,'Files/src/cs/Arithmetic');
 write(path.join(serviceDir,`I${op}NumbersHandler.cs`),`using ErrorOr;\nnamespace UsrCustomProcessElementApp.Arithmetic {\n /// <summary>Performs the ${op.toLowerCase()} operation independently of the process adapter.</summary>\n public interface I${op}NumbersHandler {\n  /// <summary>Returns the calculated value or a business error.</summary>\n  /// <param name="first">The ${captionA.toLowerCase()}.</param>\n  /// <param name="second">The ${captionB.toLowerCase()}.</param>\n  /// <returns>The result or an error value.</returns>\n  ErrorOr<decimal> Calculate(decimal first, decimal second);\n }\n}\n`);
 write(path.join(serviceDir,`${op}NumbersHandler.cs`),`using System;\nusing ErrorOr;\nnamespace UsrCustomProcessElementApp.Arithmetic {\n /// <summary>Implements ${op.toLowerCase()} with error-as-value semantics.</summary>\n public sealed class ${op}NumbersHandler : I${op}NumbersHandler {\n  /// <inheritdoc />\n  public ErrorOr<decimal> Calculate(decimal first, decimal second) {\n${op==='Divide'?'   if(second == 0m) { return Error.Validation("Arithmetic.DivisionByZero", "Divisor must not be zero."); }\n':''}   try { return checked(first ${symbol} second); }\n   catch(OverflowException) { return Error.Validation("Arithmetic.Overflow", "The result exceeds the supported decimal range."); }\n  }\n }\n}\n`);
 page(pageName,ids.pages[op],baseName,ids.basePage,`/** Separate ${op.toLowerCase()} page, inheriting only the operation selector. */
define("${pageName}",["terrasoft"],function(Terrasoft) {
 return {methods:{getOperationCode:function(){return "${op}";}},attributes:{
  "${a}":{dataValueType:Terrasoft.DataValueType.MAPPING,type:Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,initMethod:"initPropertySilent",doAutoSave:true},
  "${b}":{dataValueType:Terrasoft.DataValueType.MAPPING,type:Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,initMethod:"initPropertySilent",doAutoSave:true}
 },diff:/**SCHEMA_DIFF*/[
  {operation:"insert",name:"OperationHeading",parentName:"ArithmeticContainer",propertyName:"items",values:{itemType:Terrasoft.ViewItemType.LABEL,caption:"${op} numbers",layout:{column:0,row:1,colSpan:24}}},
  {operation:"insert",name:"${a}",parentName:"ArithmeticContainer",propertyName:"items",values:{caption:"${captionA}",layout:{column:0,row:2,colSpan:24},controlConfig:{autocomplete:"${a}Mapping"},wrapClass:["top-caption-control"]}},
  {operation:"insert",name:"${b}",parentName:"ArithmeticContainer",propertyName:"items",values:{caption:"${captionB}",layout:{column:0,row:3,colSpan:24},controlConfig:{autocomplete:"${b}Mapping"},wrapClass:["top-caption-control"]}},
  {operation:"insert",name:"ArithmeticOutputHint",parentName:"ArithmeticContainer",propertyName:"items",values:{itemType:Terrasoft.ViewItemType.LABEL,caption:"Outputs: Result, Is error, Error message.${op==='Divide'?' A zero divisor returns an error.':''}",layout:{column:0,row:4,colSpan:24}}}
 ]/**SCHEMA_DIFF*/};
});\n`);
 const svg='<svg xmlns="http://www.w3.org/2000/svg" width="42" height="42" viewBox="0 0 42 42"><rect width="42" height="42" rx="5" fill="#287A68"/><path d="M8 12h12m-6-6v12m11-6h10M8 27l10 10m0-10L8 37m17-5h10" stroke="white" stroke-width="2"/><circle cx="30" cy="26" r="1.5" fill="white"/><circle cx="30" cy="38" r="1.5" fill="white"/></svg>';
 const res=path.join(pkg,'Resources',name+'.ProcessUserTask','resource.en-US.xml');
 let xml=fs.readFileSync(res,'utf8');
 xml=xml.replace(/<Item Name="Caption"[^>]*\/>/,`<Item Name="Caption" Value="${op==='Add'?'Arithmetic':'Arithmetic: '+op}" />`);
 for(const image of ['SmallSvgImage','LargeSvgImage','TitleSvgImage']) { xml=xml.replace(new RegExp('<Item Name="'+image+'"[^>]*\\/>','g'),''); }
 xml=xml.replace('</Items>', ['SmallSvgImage','LargeSvgImage','TitleSvgImage'].map(n=>`<Item Name="${n}" Type="Image" ContentType="Data" FileExtension=".svg" Value="${Buffer.from(svg).toString('base64')}" />`).join('\n')+'</Items>');
 write(res,xml);
}
ids.tasks=registry; json(idPath,ids);
for(const [suffix,engine,key] of [['PostgreSql',2,'sqlPg'],['MsSql',0,'sqlMs']]) {
 const name='UsrRegisterArithmetic'+suffix;
 json(path.join(pkg,'SqlScripts',name,'descriptor.json'),{SqlScript:{UId:ids[key],Name:name,DBEngineType:engine,InstallType:1,ModifiedOnUtc:stamp}});
 const q=engine===2?n=>'"'+n+'"':n=>'['+n+']';
 write(path.join(pkg,'SqlScripts',name,name+'.sql'),registry.map(t=>`INSERT INTO ${q('SysProcessUserTask')} (${q('SysUserTaskSchemaUId')}, ${q('Caption')})\nSELECT s.${q('UId')}, '${t.op==='Add'?'Arithmetic':'Arithmetic: '+t.op}' FROM ${q('SysSchema')} s\nWHERE s.${q('UId')} = '${t.uid}'\nAND NOT EXISTS (SELECT 1 FROM ${q('SysProcessUserTask')} t WHERE t.${q('SysUserTaskSchemaUId')} = s.${q('UId')});`).join('\n\n')+'\n');
}
const app=path.join(pkg,'Files/src/cs/UsrCustomProcessElementApp.cs');
let appCode=fs.readFileSync(app,'utf8');
if(!appCode.includes('AddTransient<Arithmetic.IAddNumbersHandler')) appCode=appCode.replace('serviceCollection.AddTransient<Formatting.IFormatTextHandler, Formatting.FormatTextHandler>();', 'serviceCollection.AddTransient<Formatting.IFormatTextHandler, Formatting.FormatTextHandler>();\n'+operations.map(([op])=>`\t\t\tserviceCollection.AddTransient<Arithmetic.I${op}NumbersHandler, Arithmetic.${op}NumbersHandler>();`).join('\n'));
write(app,appCode);
console.log(JSON.stringify(ids,null,2));
