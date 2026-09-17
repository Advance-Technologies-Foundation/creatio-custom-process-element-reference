/** Separate divide page, inheriting only the operation selector. */
define("UsrDivideNumbersPropertiesPage",["terrasoft"],function(Terrasoft) {
 return {methods:{getOperationCode:function(){return "Divide";}},attributes:{
  "Dividend":{dataValueType:Terrasoft.DataValueType.MAPPING,type:Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,initMethod:"initPropertySilent",doAutoSave:true},
  "Divisor":{dataValueType:Terrasoft.DataValueType.MAPPING,type:Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,initMethod:"initPropertySilent",doAutoSave:true}
 },diff:/**SCHEMA_DIFF*/[
  {operation:"insert",name:"OperationHeading",parentName:"ArithmeticContainer",propertyName:"items",values:{itemType:Terrasoft.ViewItemType.LABEL,caption:"Divide numbers",layout:{column:0,row:1,colSpan:24}}},
  {operation:"insert",name:"Dividend",parentName:"ArithmeticContainer",propertyName:"items",values:{caption:"Dividend",layout:{column:0,row:2,colSpan:24},controlConfig:{autocomplete:"DividendMapping"},wrapClass:["top-caption-control"]}},
  {operation:"insert",name:"Divisor",parentName:"ArithmeticContainer",propertyName:"items",values:{caption:"Divisor",layout:{column:0,row:3,colSpan:24},controlConfig:{autocomplete:"DivisorMapping"},wrapClass:["top-caption-control"]}},
  {operation:"insert",name:"ArithmeticOutputHint",parentName:"ArithmeticContainer",propertyName:"items",values:{itemType:Terrasoft.ViewItemType.LABEL,caption:"Outputs: Result, Is error, Error message. A zero divisor returns an error.",layout:{column:0,row:4,colSpan:24}}}
 ]/**SCHEMA_DIFF*/};
});
