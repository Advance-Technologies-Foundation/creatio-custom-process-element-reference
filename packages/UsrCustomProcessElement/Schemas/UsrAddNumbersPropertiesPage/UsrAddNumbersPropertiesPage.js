/** Separate add page, inheriting only the operation selector. */
define("UsrAddNumbersPropertiesPage",["terrasoft"],function(Terrasoft) {
 return {methods:{getOperationCode:function(){return "Add";}},attributes:{
  "FirstAddend":{dataValueType:Terrasoft.DataValueType.MAPPING,type:Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,initMethod:"initPropertySilent",doAutoSave:true},
  "SecondAddend":{dataValueType:Terrasoft.DataValueType.MAPPING,type:Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,initMethod:"initPropertySilent",doAutoSave:true}
 },diff:/**SCHEMA_DIFF*/[
  {operation:"insert",name:"OperationHeading",parentName:"ArithmeticContainer",propertyName:"items",values:{itemType:Terrasoft.ViewItemType.LABEL,caption:"Add numbers",layout:{column:0,row:1,colSpan:24}}},
  {operation:"insert",name:"FirstAddend",parentName:"ArithmeticContainer",propertyName:"items",values:{caption:"First addend",layout:{column:0,row:2,colSpan:24},controlConfig:{autocomplete:"FirstAddendMapping"},wrapClass:["top-caption-control"]}},
  {operation:"insert",name:"SecondAddend",parentName:"ArithmeticContainer",propertyName:"items",values:{caption:"Second addend",layout:{column:0,row:3,colSpan:24},controlConfig:{autocomplete:"SecondAddendMapping"},wrapClass:["top-caption-control"]}},
  {operation:"insert",name:"ArithmeticOutputHint",parentName:"ArithmeticContainer",propertyName:"items",values:{itemType:Terrasoft.ViewItemType.LABEL,caption:"Outputs: Result, Is error, Error message.",layout:{column:0,row:4,colSpan:24}}}
 ]/**SCHEMA_DIFF*/};
});
