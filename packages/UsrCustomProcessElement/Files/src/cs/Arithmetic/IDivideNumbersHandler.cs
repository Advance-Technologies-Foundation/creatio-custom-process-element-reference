using ErrorOr;
namespace UsrCustomProcessElementApp.Arithmetic {
 /// <summary>Performs the divide operation independently of the process adapter.</summary>
 public interface IDivideNumbersHandler {
  /// <summary>Returns the calculated value or a business error.</summary>
  /// <param name="first">The dividend.</param>
  /// <param name="second">The divisor.</param>
  /// <returns>The result or an error value.</returns>
  ErrorOr<decimal> Calculate(decimal first, decimal second);
 }
}
