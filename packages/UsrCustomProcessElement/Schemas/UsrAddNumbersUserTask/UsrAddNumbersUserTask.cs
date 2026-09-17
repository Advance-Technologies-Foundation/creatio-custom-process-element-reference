namespace Terrasoft.Core.Process.Configuration {
 /// <summary>Maps add inputs to its DI service and returns result/error outputs.</summary>
 public partial class UsrAddNumbersUserTask {
  /// <summary>Completes synchronously, representing business failures through process outputs.</summary>
  protected override bool InternalExecute(ProcessExecutingContext context) {
Result = 0m;
IsError = false;
ErrorMessage = string.Empty;
var app = UsrCustomProcessElementApp.UsrCustomProcessElementApp.Instance;
try {
    var result = app.GetRequiredService<UsrCustomProcessElementApp.Arithmetic.IAddNumbersHandler>().Calculate(FirstAddend, SecondAddend);
    if (result.IsError) {
        IsError = true;
        ErrorMessage = string.Join(System.Environment.NewLine, System.Linq.Enumerable.Select(result.Errors, error => error.Description));
    } else { Result = result.Value; }
} catch (System.Exception exception) {
    IsError = true;
    ErrorMessage = "Arithmetic operation failed unexpectedly.";
    app.GetRequiredService<global::Common.Logging.ILog>().Error("Add numbers task failed.", exception);
}
return true;
  }
 }
}
