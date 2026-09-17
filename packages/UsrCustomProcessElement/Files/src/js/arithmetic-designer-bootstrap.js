/** Keep the four active schemas available to ChangeElementType, with one toolbox entry.
 * The native file-processing family uses this same exclusion-list extension point.
 */
(function() {
    Ext.define("Terrasoft.UsrArithmeticDesignerPalette", {
        override: "Terrasoft.Designers.ProcessSchemaDesignerViewModelNew",
        getExcludedMenuItems: function() {
            return this.callParent(arguments).concat([
                "UsrSubtractNumbersUserTask",
                "UsrMultiplyNumbersUserTask",
                "UsrDivideNumbersUserTask"
            ]);
        }
    });
}());
