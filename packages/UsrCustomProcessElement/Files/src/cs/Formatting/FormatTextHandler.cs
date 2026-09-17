using ErrorOr;

namespace UsrCustomProcessElementApp.Formatting {
    /// <summary>Implements the reference text-formatting business rule.</summary>
    public sealed class FormatTextHandler : IFormatTextHandler {
        /// <inheritdoc />
        public ErrorOr<string> Format(string text, string prefix) {
            if (string.IsNullOrWhiteSpace(text)) {
                return Error.Validation("FormatText.TextRequired", "Text to format must not be empty.");
            }
            return (prefix ?? string.Empty) + text.Trim();
        }
    }
}
