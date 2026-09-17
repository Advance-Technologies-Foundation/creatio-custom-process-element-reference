using System;
using System.Linq;
using Common.Logging;
using UsrCustomProcessElementApp.Formatting;

namespace Terrasoft.Core.Process.Configuration
{
    /// <summary>Trims the input text and prepends the configured prefix.</summary>
    public partial class UsrFormatTextUserTask
    {
        /// <summary>Maps the DI handler's ErrorOr result into process outputs and completes the element.</summary>
        protected override bool InternalExecute(ProcessExecutingContext context)
        {
            FormattedText = string.Empty;
            IsError = false;
            ErrorMessage = string.Empty;
            var app = UsrCustomProcessElementApp.UsrCustomProcessElementApp.Instance;
            try {
                var handler = app.GetRequiredService<IFormatTextHandler>();
                var result = handler.Format(Text, Prefix);
                if (result.IsError) {
                    IsError = true;
                    ErrorMessage = string.Join(Environment.NewLine, result.Errors.Select(error => error.Description));
                } else {
                    FormattedText = result.Value;
                }
            } catch (Exception exception) {
                IsError = true;
                ErrorMessage = "Text formatting failed unexpectedly.";
                app.GetRequiredService<ILog>().Error("Format text user task failed.", exception);
            }
            return true;
        }
    }
}
