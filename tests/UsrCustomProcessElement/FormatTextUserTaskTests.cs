using FluentAssertions;
using NUnit.Framework;
using Terrasoft.Core;
using Terrasoft.Core.Factories;
using Terrasoft.Core.Process.Configuration;

namespace UsrCustomProcessElement.Tests {
    /// <summary>Checks the real task implementation, including its generated parameter properties.</summary>
    [TestFixture, NonParallelizable]
    public partial class FormatTextUserTaskTests : AppServiceTestFixture {
        [TestCase("  Creatio  ", "Hello, ", "Hello, Creatio")]
        [TestCase(" value ", "Prefix: ", "Prefix: value")]
        [TestCase(" value ", null, "value")]
        [TestCase("  Grüß Gott  ", "", "Grüß Gott")]
        [TestCase(" a  b ", "  ", "  a  b")]
        [Description("Runs the native task body and verifies completion and text transformation without changing internal whitespace or the prefix.")]
        public void InternalExecute_ShouldReturnFormattedText_WhenInputsAreProvided(string text, string prefix, string expected) {
            // Arrange
            var task = ClassFactory.Get<TestClass>(new ConstructorArgument("userConnection", UserConnection));
            task.Text = text;
            task.Prefix = prefix;

            // Act: the body does not use an execution context or access the database.
            var completed = task.TestExecute();

            // Assert
            completed.Should().BeTrue("the text transformation is synchronous and must allow the process to continue");
            task.FormattedText.Should().Be(expected, "the output must trim only the input boundaries and preserve the prefix exactly");
            task.IsError.Should().BeFalse("successful execution must report no error");
            task.ErrorMessage.Should().BeEmpty("successful execution must not leave an error message");
        }
    }




    /// <summary>Exposes the inherited task execution method for unit tests without reflection.</summary>
    public class TestClass : UsrFormatTextUserTask {



        /// <summary>Initializes the task using the test fixture's user connection.</summary>
        /// <param name="userConnection">The user connection supplied by the test fixture.</param>
        public TestClass(UserConnection userConnection): base(userConnection) {

        }

        /// <summary>Executes the real task body using the inherited input and output properties.</summary>
        /// <returns>Whether the task completed synchronously.</returns>
        public bool TestExecute() {
            return InternalExecute(null);
        }


    }



}
