using System;
using System.Runtime.Serialization;

namespace Ini.Exceptions
{
	/// <summary>
	/// A configuration related task required the configuration to be valid, but it was not.
	/// </summary>
	public class InvalidConfigException : Exception
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidConfigException"/> class.
		/// </summary>
		public InvalidConfigException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidConfigException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public InvalidConfigException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidConfigException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
		public InvalidConfigException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidConfigException"/> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
		public InvalidConfigException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		#endregion
	}
}
