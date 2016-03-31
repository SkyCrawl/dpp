using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _02_api_alternative.Exceptions
{
    /// <summary>
    /// There is more than one configuration section with the same identifier.
    /// </summary>
    public class DuplicateSectionException : ConfigurationException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateSectionException"/> class.
        /// </summary>
        public DuplicateSectionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateSectionException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DuplicateSectionException(string message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateSectionException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        public DuplicateSectionException(SerializationInfo info, StreamingContext context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateSectionException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public DuplicateSectionException(string message, Exception innerException)
        {
        }

        #endregion
    }
}
