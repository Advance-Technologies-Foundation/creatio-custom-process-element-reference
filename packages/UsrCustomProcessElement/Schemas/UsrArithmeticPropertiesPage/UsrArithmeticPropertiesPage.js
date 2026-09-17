/** Shared selector. Concrete pages define their own input parameters. */
define("UsrArithmeticPropertiesPage", ["terrasoft"], function(Terrasoft) {
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
});
