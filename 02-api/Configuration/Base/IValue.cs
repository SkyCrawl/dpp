namespace Ini.Configuration.Base
{
    /// <summary>
    /// Interface for an option's value.
    /// </summary>
    public interface IValue : IElement
    {
        /// <summary>
        /// The element's value, cast to the output type.
        /// </summary>
        OutputType GetValue<OutputType>();

        /// <summary>
        /// Parses the string value and saves it into the internal value of the element.
        /// </summary>
        /// <param name="value">The string.</param>
        void FillFromString(string value);
    }
}
