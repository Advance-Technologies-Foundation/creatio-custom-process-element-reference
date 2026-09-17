using ErrorOr;
namespace UsrCustomProcessElementApp.Arithmetic {
 /// <summary>Performs the subtract operation independently of the process adapter.</summary>
 public interface ISubtractNumbersHandler {
  /// <summary>Returns the calculated value or a business error.</summary>
  /// <param name="first">The minuend.</param>
  /// <param name="second">The subtrahend.</param>
  /// <returns>The result or an error value.</returns>
  ErrorOr<decimal> Calculate(decimal first, decimal second);
 }
}
