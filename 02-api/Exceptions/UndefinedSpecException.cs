﻿using System;
using System.Runtime.Serialization;

namespace Ini.Exceptions
{
	/// <summary>
	/// An operation demaning a specification was called but none was defined.
	/// </summary>
	public class UndefinedSpecException : Exception
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="UndefinedSpecException"/> class.
		/// </summary>
		public UndefinedSpecException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UndefinedSpecException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public UndefinedSpecException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UndefinedSpecException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
		public UndefinedSpecException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UndefinedSpecException"/> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
		public UndefinedSpecException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		#endregion
	}
}
