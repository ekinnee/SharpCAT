using System;

namespace SharpCAT.Client
{
    /// <summary>
    /// Exception thrown by SharpCAT client operations
    /// </summary>
    public class SharpCATClientException : Exception
    {
        /// <summary>
        /// Additional details about the error
        /// </summary>
        public string? Details { get; }

        /// <summary>
        /// Initializes a new instance of the SharpCATClientException class
        /// </summary>
        public SharpCATClientException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the SharpCATClientException class with a specified error message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public SharpCATClientException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SharpCATClientException class with a specified error message and details
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="details">Additional details about the error</param>
        public SharpCATClientException(string message, string? details) : base(message)
        {
            Details = details;
        }

        /// <summary>
        /// Initializes a new instance of the SharpCATClientException class with a specified error message and a reference to the inner exception
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public SharpCATClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SharpCATClientException class with a specified error message, details, and a reference to the inner exception
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="details">Additional details about the error</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public SharpCATClientException(string message, string? details, Exception innerException) : base(message, innerException)
        {
            Details = details;
        }
    }
}