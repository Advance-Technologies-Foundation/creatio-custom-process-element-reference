/** Separate subtract page, inheriting only the operation selector. */
define("UsrSubtractNumbersPropertiesPage",["terrasoft"],function(Terrasoft) {
 return {methods:{getOperationCode:function(){return "Subtract";}},attributes:{
  "Minuend":{dataValueType:Terrasoft.DataValueType.MAPPING,type:Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,initMethod:"initPropertySilent",doAutoSave:true},
  "Subtrahend":{dataValueType:Terrasoft.DataValueType.MAPPING,type:Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,initMethod:"initPropertySilent",doAutoSave:true}
 },diff:/**SCHEMA_DIFF*/[
  {operation:"insert",name:"OperationHeading",parentName:"ArithmeticContainer",propertyName:"items",values:{itemType:Terrasoft.ViewItemType.LABEL,caption:"Subtract numbers",layout:{column:0,row:1,colSpan:24}}},
  {operation:"insert",name:"Minuend",parentName:"ArithmeticContainer",propertyName:"items",values:{caption:"Minuend",layout:{column:0,row:2,colSpan:24},controlConfig:{autocomplete:"MinuendMapping"},wrapClass:["top-caption-control"]}},
  {operation:"insert",name:"Subtrahend",parentName:"ArithmeticContainer",propertyName:"items",values:{caption:"Subtrahend",layout:{column:0,row:3,colSpan:24},controlConfig:{autocomplete:"SubtrahendMapping"},wrapClass:["top-caption-control"]}},
  {operation:"insert",name:"ArithmeticOutputHint",parentName:"ArithmeticContainer",propertyName:"items",values:{itemType:Terrasoft.ViewItemType.LABEL,caption:"Outputs: Result, Is error, Error message.",layout:{column:0,row:4,colSpan:24}}}
 ]/**SCHEMA_DIFF*/};
});
