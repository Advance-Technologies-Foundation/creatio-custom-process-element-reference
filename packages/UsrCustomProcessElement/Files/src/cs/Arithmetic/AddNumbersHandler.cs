using System;
using ErrorOr;
namespace UsrCustomProcessElementApp.Arithmetic {
 /// <summary>Implements add with error-as-value semantics.</summary>
 public sealed class AddNumbersHandler : IAddNumbersHandler {
  /// <inheritdoc />
  public ErrorOr<decimal> Calculate(decimal first, decimal second) {
   try { return checked(first + second); }
   catch(OverflowException) { return Error.Validation("Arithmetic.Overflow", "The result exceeds the supported decimal range."); }
  }
 }
}
