using System;
using System.Collections.Generic;
using ErrorOr;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Terrasoft.Core.Factories;
using UsrCustomProcessElementApp.Formatting;
using App = UsrCustomProcessElementApp.UsrCustomProcessElementApp;

namespace UsrCustomProcessElement.Tests {
    /// <summary>Uses the scaffold's reset and injection hooks to isolate the application container.</summary>
    public abstract class AppServiceTestFixture : BaseComposableAppTestFixture {
        /// <summary>Resets the app before each test.</summary>
        [SetUp]
        public void ResetApplication() {
            App.InjectedServices = null;
            App.Instance.Reset();
        }

        /// <summary>Removes test-only services after each test.</summary>
        [TearDown]
        public void CleanApplication() {
            App.InjectedServices = null;
            App.Instance.Reset();
        }

        /// <summary>Overrides the handler through the real application container.</summary>
        protected void UseHandler(IFormatTextHandler handler) {
            App.InjectedServices = new Func<IServiceCollection, IServiceCollection>[] {
                services => { services.AddSingleton<IFormatTextHandler>(handler); return services; }
            };
        }
    }

    /// <summary>Tests the ErrorOr service independently of the process adapter.</summary>
    [TestFixture, NonParallelizable]
    public class FormatTextHandlerTests : AppServiceTestFixture {
        /// <summary>Checks the successful business operation.</summary>
        [Test, Description("The registered handler trims boundaries and preserves prefix and internal whitespace.")]
        public void ReturnsFormattedValue() {
            // Arrange
            var handler = App.Instance.GetRequiredService<IFormatTextHandler>();
            // Act
            var result = handler.Format("  two  words  ", "Demo: ");
            // Assert
            result.IsError.Should().BeFalse("valid input must produce a value");
            result.Value.Should().Be("Demo: two  words", "only input boundaries should be trimmed");
        }

        /// <summary>Checks expected validation errors are values rather than exceptions.</summary>
        [TestCase(null), TestCase(""), TestCase(" \t ")]
        [Description("Missing text returns a typed ErrorOr validation failure.")]
        public void ReturnsValidationError(string text) {
            // Arrange
            var handler = App.Instance.GetRequiredService<IFormatTextHandler>();
            // Act
            var result = handler.Format(text, "Demo: ");
            // Assert
            result.IsError.Should().BeTrue("missing text violates the business rule");
            result.FirstError.Type.Should().Be(ErrorType.Validation, "this is an expected validation outcome");
            result.FirstError.Code.Should().Be("FormatText.TextRequired", "callers need a stable error identity");
        }
    }

    public partial class FormatTextUserTaskTests {
        /// <summary>Checks failure and recovery on the same process task instance.</summary>
        [TestCase(null), TestCase(""), TestCase(" \t ")]
        [Description("Validation failure completes normally with error outputs; a subsequent success clears stale errors.")]
        public void ReportsErrorAndThenRecovers(string invalidText) {
            // Arrange
            var task = ClassFactory.Get<TestClass>(new ConstructorArgument("userConnection", UserConnection));
            task.Text = invalidText;
            task.Prefix = "Demo: ";
            task.FormattedText = "stale";
            // Act
            bool completed = task.TestExecute();
            // Assert
            completed.Should().BeTrue("reported failure must allow downstream process branching");
            task.IsError.Should().BeTrue("the handler rejected missing text");
            task.ErrorMessage.Should().Be("Text to format must not be empty.", "the business error must reach the process");
            task.FormattedText.Should().BeEmpty("failure must clear an old result");

            // Arrange
            task.Text = " valid ";
            // Act
            bool recovered = task.TestExecute();
            // Assert
            recovered.Should().BeTrue("corrected input completes synchronously");
            task.IsError.Should().BeFalse("success must clear a previous error flag");
            task.ErrorMessage.Should().BeEmpty("success must clear a previous error message");
            task.FormattedText.Should().Be("Demo: valid", "the new result must replace prior state");
        }

        /// <summary>Proves the task resolves the injected handler and preserves its error descriptions.</summary>
        [Test, Description("The task maps all errors from its DI service without truncating long text.")]
        public void MapsInjectedHandlerErrors() {
            // Arrange
            var handler = Substitute.For<IFormatTextHandler>();
            string longMessage = new string('x', 5000);
            handler.Format("input", "prefix").Returns((ErrorOr<string>)new List<Error> {
                Error.Validation("Example.First", longMessage), Error.Failure("Example.Second", "Second error")
            });
            UseHandler(handler);
            var task = ClassFactory.Get<TestClass>(new ConstructorArgument("userConnection", UserConnection));
            task.Text = "input";
            task.Prefix = "prefix";
            // Act
            bool completed = task.TestExecute();
            // Assert
            completed.Should().BeTrue("ErrorOr failures are handled outcomes");
            task.IsError.Should().BeTrue("the injected service returned errors");
            task.ErrorMessage.Should().Be(longMessage + Environment.NewLine + "Second error", "all descriptions must survive output mapping");
            task.FormattedText.Should().BeEmpty("an error has no formatted value");
        }

        /// <summary>Checks the process boundary handles unexpected handler exceptions.</summary>
        [Test, Description("An unexpected exception becomes stable process error outputs without exposing internal details.")]
        public void MapsUnexpectedHandlerFailure() {
            // Arrange
            var handler = Substitute.For<IFormatTextHandler>();
            handler.Format("input", "").Returns(_ => throw new InvalidOperationException("Internal diagnostic detail"));
            UseHandler(handler);
            var task = ClassFactory.Get<TestClass>(new ConstructorArgument("userConnection", UserConnection));
            task.Text = "input";
            task.Prefix = "";
            // Act
            bool completed = task.TestExecute();
            // Assert
            completed.Should().BeTrue("the process must be able to handle the error result");
            task.IsError.Should().BeTrue("an exception cannot be reported as success");
            task.ErrorMessage.Should().Be("Text formatting failed unexpectedly.", "internal diagnostics belong in the server log");
            task.FormattedText.Should().BeEmpty("failure must not return a success value");
        }
    }
}
