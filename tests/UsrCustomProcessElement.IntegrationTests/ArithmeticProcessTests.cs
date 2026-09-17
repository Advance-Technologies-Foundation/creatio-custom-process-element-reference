using FluentAssertions;
using NUnit.Framework;
using UsrCustomProcessElement.IntegrationTests.Infrastructure;
using UsrCustomProcessElement.IntegrationTests.ProcessModels;

namespace UsrCustomProcessElement.IntegrationTests;

/// <summary>Exercises four distinct task schemas through saved native processes and generated models.</summary>
[TestFixture]
public class ArithmeticProcessTests : CreatioIntegrationFixture {
    /// <summary>Runs addition with both process inputs mapped.</summary>
    [Test, Description("The installed add task returns sum and cleared error outputs.")]
    public void Adds() {
        // Arrange
        var process = new UsrArithmetic_Add { FirstAddendParameter = 12.5f, SecondAddendParameter = -3f };
        // Act
        var response = ProcessContext.RunProcess(process);
        // Assert
        response.Success.Should().BeTrue("the process must complete: " + response.ErrorMessage);
        response.Result.ResultParameter.Should().Be(9.5f, "the add task must sum the operands");
        response.Result.IsErrorParameter.Should().BeFalse("valid addition has no business error");
        response.Result.ErrorMessageParameter.Should().BeNullOrEmpty("success clears the error message");
    }

    /// <summary>Runs subtraction and verifies operand order.</summary>
    [Test, Description("The installed subtract task preserves minuend and subtrahend order.")]
    public void Subtracts() {
        // Arrange
        var process = new UsrArithmetic_Subtract { MinuendParameter = 3f, SubtrahendParameter = 12.5f };
        // Act
        var response = ProcessContext.RunProcess(process);
        // Assert
        response.Success.Should().BeTrue("the process must complete: " + response.ErrorMessage);
        response.Result.ResultParameter.Should().Be(-9.5f, "subtraction must preserve operand order");
        response.Result.IsErrorParameter.Should().BeFalse("valid subtraction has no business error");
        response.Result.ErrorMessageParameter.Should().BeNullOrEmpty("success clears the error message");
    }

    /// <summary>Runs multiplication with fractional input.</summary>
    [Test, Description("The installed multiply task returns a decimal product.")]
    public void Multiplies() {
        // Arrange
        var process = new UsrArithmetic_Multiply { MultiplicandParameter = 2.5f, MultiplierParameter = -3f };
        // Act
        var response = ProcessContext.RunProcess(process);
        // Assert
        response.Success.Should().BeTrue("the process must complete: " + response.ErrorMessage);
        response.Result.ResultParameter.Should().Be(-7.5f, "multiplication must preserve decimal values");
        response.Result.IsErrorParameter.Should().BeFalse("valid multiplication has no business error");
        response.Result.ErrorMessageParameter.Should().BeNullOrEmpty("success clears the error message");
    }

    /// <summary>Runs division and checks its expected-error contract.</summary>
    [TestCase(4, false, 2.5), TestCase(0, true, 0)]
    [Description("Division returns a quotient or completes normally with the zero-divisor error outputs.")]
    public void DividesOrReportsError(int divisor, bool isError, double expected) {
        // Arrange
        var process = new UsrArithmetic_Divide { DividendParameter = 10f, DivisorParameter = divisor };
        // Act
        var response = ProcessContext.RunProcess(process);
        // Assert
        response.Success.Should().BeTrue("business errors must not fault the process: " + response.ErrorMessage);
        response.Result.ResultParameter.Should().Be((float)expected, "result must contain the quotient or the cleared failure value");
        response.Result.IsErrorParameter.Should().Be(isError, "the error flag must distinguish a zero divisor");
        if (isError) {
            response.Result.ErrorMessageParameter.Should().Be("Divisor must not be zero.", "the handler's validation message must survive process mapping");
        } else {
            response.Result.ErrorMessageParameter.Should().BeNullOrEmpty("success clears the error message");
        }
    }
}
