/** Separate multiply page, inheriting only the operation selector. */
define("UsrMultiplyNumbersPropertiesPage",["terrasoft"],function(Terrasoft) {
 return {methods:{getOperationCode:function(){return "Multiply";}},attributes:{
  "Multiplicand":{dataValueType:Terrasoft.DataValueType.MAPPING,type:Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,initMethod:"initPropertySilent",doAutoSave:true},
  "Multiplier":{dataValueType:Terrasoft.DataValueType.MAPPING,type:Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,initMethod:"initPropertySilent",doAutoSave:true}
 },diff:/**SCHEMA_DIFF*/[
  {operation:"insert",name:"OperationHeading",parentName:"ArithmeticContainer",propertyName:"items",values:{itemType:Terrasoft.ViewItemType.LABEL,caption:"Multiply numbers",layout:{column:0,row:1,colSpan:24}}},
  {operation:"insert",name:"Multiplicand",parentName:"ArithmeticContainer",propertyName:"items",values:{caption:"Multiplicand",layout:{column:0,row:2,colSpan:24},controlConfig:{autocomplete:"MultiplicandMapping"},wrapClass:["top-caption-control"]}},
  {operation:"insert",name:"Multiplier",parentName:"ArithmeticContainer",propertyName:"items",values:{caption:"Multiplier",layout:{column:0,row:3,colSpan:24},controlConfig:{autocomplete:"MultiplierMapping"},wrapClass:["top-caption-control"]}},
  {operation:"insert",name:"ArithmeticOutputHint",parentName:"ArithmeticContainer",propertyName:"items",values:{itemType:Terrasoft.ViewItemType.LABEL,caption:"Outputs: Result, Is error, Error message.",layout:{column:0,row:4,colSpan:24}}}
 ]/**SCHEMA_DIFF*/};
});
