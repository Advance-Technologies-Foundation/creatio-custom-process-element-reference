using ErrorOr;

namespace UsrCustomProcessElementApp.Formatting {
    /// <summary>Formats nonblank text or returns an expected validation error as a value.</summary>
    public interface IFormatTextHandler {
        /// <summary>Trims the input boundaries and prepends the prefix without changing internal whitespace.</summary>
        /// <param name="text">Text to format; null, empty, and whitespace-only input are invalid.</param>
        /// <param name="prefix">An optional prefix, preserved exactly.</param>
        /// <returns>The formatted string or a validation error.</returns>
        ErrorOr<string> Format(string text, string prefix);
    }
}
