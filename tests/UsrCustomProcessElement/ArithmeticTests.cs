using System;
using FluentAssertions;
using NUnit.Framework;
using Terrasoft.Core;
using Terrasoft.Core.Factories;
using Terrasoft.Core.Process.Configuration;
using UsrCustomProcessElementApp.Arithmetic;
using App = UsrCustomProcessElementApp.UsrCustomProcessElementApp;
namespace UsrCustomProcessElement.Tests {
/// <summary>Tests the real add task through a derived wrapper and its DI service.</summary>
[TestFixture,NonParallelizable]
public class AddArithmeticTests : AppServiceTestFixture {
 /// <summary>Verifies inputs, synchronous completion, calculated output and cleared errors.</summary>
 [Test,Description("Add executes through the app DI container and maps all outputs.")]
 public void ExecutesTask() {
  // Arrange
  var task=ClassFactory.Get<TestAddTask>(new ConstructorArgument("userConnection",UserConnection));
  task.FirstAddend=12m; task.SecondAddend=3m; task.IsError=true; task.ErrorMessage="stale";
  // Act
  var completed=task.TestExecute();
  // Assert
  completed.Should().BeTrue("arithmetic tasks complete synchronously");
  task.Result.Should().Be(15m,"the selected task must execute its own operation");
  task.IsError.Should().BeFalse("successful execution clears the previous error");
  task.ErrorMessage.Should().BeEmpty("successful execution clears the previous message");
 }
 /// <summary>Checks decimal arithmetic through the registered service.</summary>
 [Test,Description("The add service supports negative and fractional operands.")]
 public void SupportsDecimalValues() {
  // Arrange
  var handler=App.Instance.GetRequiredService<IAddNumbersHandler>();
  // Act
  var result=handler.Calculate(-2.5m,0.5m);
  // Assert
  result.IsError.Should().BeFalse("fractional and negative numbers are valid operands");
  result.Value.Should().Be(-2m,"decimal precision and operand order must be preserved");
 }
 /// <summary>Checks arithmetic overflow is an error value.</summary>
 [Test,Description("The add handler reports overflow through ErrorOr.")]
 public void ReportsOverflow() {
  // Arrange
  var handler=App.Instance.GetRequiredService<IAddNumbersHandler>();
  // Act
  var result=handler.Calculate(decimal.MaxValue,1m);
  // Assert
  result.IsError.Should().BeTrue("an unrepresentable decimal result is a business failure");
  result.FirstError.Code.Should().Be("Arithmetic.Overflow","overflow has a stable error identity");
 }
}
/// <summary>Exposes the protected method without reflection.</summary>
public class TestAddTask : UsrAddNumbersUserTask {
 /// <summary>Forwards the test user connection to the real task.</summary>
 /// <param name="userConnection">The platform test connection.</param>
 public TestAddTask(UserConnection userConnection) : base(userConnection) {}
 /// <summary>Runs the actual task implementation.</summary>
 /// <returns>True when the task completes.</returns>
 public bool TestExecute() { return InternalExecute(null); }
}
/// <summary>Tests the real subtract task through a derived wrapper and its DI service.</summary>
[TestFixture,NonParallelizable]
public class SubtractArithmeticTests : AppServiceTestFixture {
 /// <summary>Verifies inputs, synchronous completion, calculated output and cleared errors.</summary>
 [Test,Description("Subtract executes through the app DI container and maps all outputs.")]
 public void ExecutesTask() {
  // Arrange
  var task=ClassFactory.Get<TestSubtractTask>(new ConstructorArgument("userConnection",UserConnection));
  task.Minuend=12m; task.Subtrahend=3m; task.IsError=true; task.ErrorMessage="stale";
  // Act
  var completed=task.TestExecute();
  // Assert
  completed.Should().BeTrue("arithmetic tasks complete synchronously");
  task.Result.Should().Be(9m,"the selected task must execute its own operation");
  task.IsError.Should().BeFalse("successful execution clears the previous error");
  task.ErrorMessage.Should().BeEmpty("successful execution clears the previous message");
 }
 /// <summary>Checks decimal arithmetic through the registered service.</summary>
 [Test,Description("The subtract service supports negative and fractional operands.")]
 public void SupportsDecimalValues() {
  // Arrange
  var handler=App.Instance.GetRequiredService<ISubtractNumbersHandler>();
  // Act
  var result=handler.Calculate(-2.5m,0.5m);
  // Assert
  result.IsError.Should().BeFalse("fractional and negative numbers are valid operands");
  result.Value.Should().Be(-3m,"decimal precision and operand order must be preserved");
 }
 /// <summary>Checks arithmetic overflow is an error value.</summary>
 [Test,Description("The subtract handler reports overflow through ErrorOr.")]
 public void ReportsOverflow() {
  // Arrange
  var handler=App.Instance.GetRequiredService<ISubtractNumbersHandler>();
  // Act
  var result=handler.Calculate(decimal.MaxValue,-1m);
  // Assert
  result.IsError.Should().BeTrue("an unrepresentable decimal result is a business failure");
  result.FirstError.Code.Should().Be("Arithmetic.Overflow","overflow has a stable error identity");
 }
}
/// <summary>Exposes the protected method without reflection.</summary>
public class TestSubtractTask : UsrSubtractNumbersUserTask {
 /// <summary>Forwards the test user connection to the real task.</summary>
 /// <param name="userConnection">The platform test connection.</param>
 public TestSubtractTask(UserConnection userConnection) : base(userConnection) {}
 /// <summary>Runs the actual task implementation.</summary>
 /// <returns>True when the task completes.</returns>
 public bool TestExecute() { return InternalExecute(null); }
}
/// <summary>Tests the real multiply task through a derived wrapper and its DI service.</summary>
[TestFixture,NonParallelizable]
public class MultiplyArithmeticTests : AppServiceTestFixture {
 /// <summary>Verifies inputs, synchronous completion, calculated output and cleared errors.</summary>
 [Test,Description("Multiply executes through the app DI container and maps all outputs.")]
 public void ExecutesTask() {
  // Arrange
  var task=ClassFactory.Get<TestMultiplyTask>(new ConstructorArgument("userConnection",UserConnection));
  task.Multiplicand=12m; task.Multiplier=3m; task.IsError=true; task.ErrorMessage="stale";
  // Act
  var completed=task.TestExecute();
  // Assert
  completed.Should().BeTrue("arithmetic tasks complete synchronously");
  task.Result.Should().Be(36m,"the selected task must execute its own operation");
  task.IsError.Should().BeFalse("successful execution clears the previous error");
  task.ErrorMessage.Should().BeEmpty("successful execution clears the previous message");
 }
 /// <summary>Checks decimal arithmetic through the registered service.</summary>
 [Test,Description("The multiply service supports negative and fractional operands.")]
 public void SupportsDecimalValues() {
  // Arrange
  var handler=App.Instance.GetRequiredService<IMultiplyNumbersHandler>();
  // Act
  var result=handler.Calculate(-2.5m,0.5m);
  // Assert
  result.IsError.Should().BeFalse("fractional and negative numbers are valid operands");
  result.Value.Should().Be(-1.25m,"decimal precision and operand order must be preserved");
 }
 /// <summary>Checks arithmetic overflow is an error value.</summary>
 [Test,Description("The multiply handler reports overflow through ErrorOr.")]
 public void ReportsOverflow() {
  // Arrange
  var handler=App.Instance.GetRequiredService<IMultiplyNumbersHandler>();
  // Act
  var result=handler.Calculate(decimal.MaxValue,2m);
  // Assert
  result.IsError.Should().BeTrue("an unrepresentable decimal result is a business failure");
  result.FirstError.Code.Should().Be("Arithmetic.Overflow","overflow has a stable error identity");
 }
}
/// <summary>Exposes the protected method without reflection.</summary>
public class TestMultiplyTask : UsrMultiplyNumbersUserTask {
 /// <summary>Forwards the test user connection to the real task.</summary>
 /// <param name="userConnection">The platform test connection.</param>
 public TestMultiplyTask(UserConnection userConnection) : base(userConnection) {}
 /// <summary>Runs the actual task implementation.</summary>
 /// <returns>True when the task completes.</returns>
 public bool TestExecute() { return InternalExecute(null); }
}
/// <summary>Tests the real divide task through a derived wrapper and its DI service.</summary>
[TestFixture,NonParallelizable]
public class DivideArithmeticTests : AppServiceTestFixture {
 /// <summary>Verifies inputs, synchronous completion, calculated output and cleared errors.</summary>
 [Test,Description("Divide executes through the app DI container and maps all outputs.")]
 public void ExecutesTask() {
  // Arrange
  var task=ClassFactory.Get<TestDivideTask>(new ConstructorArgument("userConnection",UserConnection));
  task.Dividend=12m; task.Divisor=3m; task.IsError=true; task.ErrorMessage="stale";
  // Act
  var completed=task.TestExecute();
  // Assert
  completed.Should().BeTrue("arithmetic tasks complete synchronously");
  task.Result.Should().Be(4m,"the selected task must execute its own operation");
  task.IsError.Should().BeFalse("successful execution clears the previous error");
  task.ErrorMessage.Should().BeEmpty("successful execution clears the previous message");
 }
 /// <summary>Checks decimal arithmetic through the registered service.</summary>
 [Test,Description("The divide service supports negative and fractional operands.")]
 public void SupportsDecimalValues() {
  // Arrange
  var handler=App.Instance.GetRequiredService<IDivideNumbersHandler>();
  // Act
  var result=handler.Calculate(-2.5m,0.5m);
  // Assert
  result.IsError.Should().BeFalse("fractional and negative numbers are valid operands");
  result.Value.Should().Be(-5m,"decimal precision and operand order must be preserved");
 }
 /// <summary>Checks arithmetic overflow is an error value.</summary>
 [Test,Description("The divide handler reports overflow through ErrorOr.")]
 public void ReportsOverflow() {
  // Arrange
  var handler=App.Instance.GetRequiredService<IDivideNumbersHandler>();
  // Act
  var result=handler.Calculate(decimal.MaxValue,0.1m);
  // Assert
  result.IsError.Should().BeTrue("an unrepresentable decimal result is a business failure");
  result.FirstError.Code.Should().Be("Arithmetic.Overflow","overflow has a stable error identity");
 }
}
/// <summary>Exposes the protected method without reflection.</summary>
public class TestDivideTask : UsrDivideNumbersUserTask {
 /// <summary>Forwards the test user connection to the real task.</summary>
 /// <param name="userConnection">The platform test connection.</param>
 public TestDivideTask(UserConnection userConnection) : base(userConnection) {}
 /// <summary>Runs the actual task implementation.</summary>
 /// <returns>True when the task completes.</returns>
 public bool TestExecute() { return InternalExecute(null); }
}
/// <summary>Checks division failure and recovery on the same real task.</summary>
[TestFixture,NonParallelizable]
public class DivisionErrorArithmeticTests : AppServiceTestFixture {
 /// <summary>Checks the business failure contract and clears stale outputs on reuse.</summary>
 [Test,Description("Division by zero completes with error outputs; a later valid execution clears them.")]
 public void DivisionByZeroThenRecovery() {
  // Arrange
  var task=ClassFactory.Get<TestDivideTask>(new ConstructorArgument("userConnection",UserConnection));
  task.Dividend=10m; task.Divisor=0m; task.Result=999m;
  // Act
  var completed=task.TestExecute();
  // Assert
  completed.Should().BeTrue("expected errors must allow the process to branch");
  task.IsError.Should().BeTrue("division by zero is invalid");
  task.ErrorMessage.Should().Be("Divisor must not be zero.","the ErrorOr message must reach the process output");
  task.Result.Should().Be(0m,"failure must clear stale results");
  // Arrange
  task.Divisor=4m;
  // Act
  completed=task.TestExecute();
  // Assert
  completed.Should().BeTrue("the task remains executable after a handled error");
  task.Result.Should().Be(2.5m,"recovery must calculate the new result");
  task.IsError.Should().BeFalse("success clears the error flag");
  task.ErrorMessage.Should().BeEmpty("success clears the error message");
 }
}
}
