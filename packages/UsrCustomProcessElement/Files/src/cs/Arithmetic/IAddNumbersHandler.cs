using ErrorOr;
namespace UsrCustomProcessElementApp.Arithmetic {
 /// <summary>Performs the add operation independently of the process adapter.</summary>
 public interface IAddNumbersHandler {
  /// <summary>Returns the calculated value or a business error.</summary>
  /// <param name="first">The first addend.</param>
  /// <param name="second">The second addend.</param>
  /// <returns>The result or an error value.</returns>
  ErrorOr<decimal> Calculate(decimal first, decimal second);
 }
}
