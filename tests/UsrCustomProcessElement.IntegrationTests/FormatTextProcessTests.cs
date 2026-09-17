using FluentAssertions;
using NUnit.Framework;
using UsrCustomProcessElement.IntegrationTests.Infrastructure;
using UsrCustomProcessElement.IntegrationTests.ProcessModels;

namespace UsrCustomProcessElement.IntegrationTests;

/// <summary>Runs the installed reference process through the real Creatio process engine.</summary>
[TestFixture]
public class FormatTextProcessTests : CreatioIntegrationFixture {
    /// <summary>Verifies both process inputs and all three mapped task outputs.</summary>
    [TestCase("  Creatio  ", "Demo: ", "Demo: Creatio")]
    [TestCase("  Grüß Gott 世界  ", "Hello: ", "Hello: Grüß Gott 世界")]
    [TestCase("  two  words  ", "", "two  words")]
    [Description("The real process must pass both inputs to the task and return its formatted value and error status.")]
    public void RunsCustomUserTask(string input, string prefix, string expected) {
        // Arrange
        var process = new UsrText_Format { InputTextParameter = input, PrefixParameter = prefix };

        // Act
        var response = ProcessContext.RunProcess(process);

        // Assert
        response.Success.Should().BeTrue(because: "the installed process must complete without a runtime error: " + response.ErrorMessage);
        response.Result.FormattedTextParameter.Should().Be(expected,
            because: "both process inputs must reach the DI handler through the task mappings");
        response.Result.IsErrorParameter.Should().BeFalse(because: "valid input must produce a successful business result");
        response.Result.ErrorMessageParameter.Should().BeNullOrEmpty(because: "ATF exposes cleared text as null, but success must expose no error message");
    }

    /// <summary>Checks expected business errors are returned as outputs by the real process engine.</summary>
    [TestCase("")]
    [TestCase(" \t ")]
    [Description("The process completes with explicit error outputs when the handler rejects blank input.")]
    public void ReturnsHandledBusinessError(string input) {
        // Arrange
        var process = new UsrText_Format { InputTextParameter = input, PrefixParameter = "Demo: " };
        // Act
        var response = ProcessContext.RunProcess(process);
        // Assert
        response.Success.Should().BeTrue(because: "business failure is represented by outputs rather than a process exception");
        response.Result.IsErrorParameter.Should().BeTrue(because: "the live handler must reject blank text");
        response.Result.ErrorMessageParameter.Should().Be("Text to format must not be empty.", because: "the handler error must survive task and process mapping");
        response.Result.FormattedTextParameter.Should().BeNullOrEmpty(because: "ATF exposes cleared text as null, but failure must not return a formatted value");
    }
}

