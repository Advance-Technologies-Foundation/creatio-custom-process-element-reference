using ErrorOr;
namespace UsrCustomProcessElementApp.Arithmetic {
 /// <summary>Performs the multiply operation independently of the process adapter.</summary>
 public interface IMultiplyNumbersHandler {
  /// <summary>Returns the calculated value or a business error.</summary>
  /// <param name="first">The multiplicand.</param>
  /// <param name="second">The multiplier.</param>
  /// <returns>The result or an error value.</returns>
  ErrorOr<decimal> Calculate(decimal first, decimal second);
 }
}
