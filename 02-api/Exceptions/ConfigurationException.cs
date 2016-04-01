using System;
using System.Runtime.Serialization;

namespace Ini.Exceptions
{
    /// <summary>
    /// The general exception type for invalid configuration.
    /// </summary>
    public class ConfigurationException : Exception
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationException"/> class.
        /// </summary>
        public ConfigurationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ConfigurationException(string message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        public ConfigurationException(SerializationInfo info, StreamingContext context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ConfigurationException(string message, Exception innerException)
        {
        }

        #endregion
    }
}
