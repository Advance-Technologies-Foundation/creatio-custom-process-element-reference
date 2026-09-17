using System;
using ErrorOr;
namespace UsrCustomProcessElementApp.Arithmetic {
 /// <summary>Implements divide with error-as-value semantics.</summary>
 public sealed class DivideNumbersHandler : IDivideNumbersHandler {
  /// <inheritdoc />
  public ErrorOr<decimal> Calculate(decimal first, decimal second) {
   if(second == 0m) { return Error.Validation("Arithmetic.DivisionByZero", "Divisor must not be zero."); }
   try { return checked(first / second); }
   catch(OverflowException) { return Error.Validation("Arithmetic.Overflow", "The result exceeds the supported decimal range."); }
  }
 }
}
