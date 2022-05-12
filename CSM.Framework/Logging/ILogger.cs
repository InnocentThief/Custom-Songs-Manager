using System;

namespace CSM.Framework.Logging
{
    /// <summary>
    /// Interface that provides access to logger functionality.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs a message with debug severity.
        /// </summary>
        /// <typeparam name="T">The datatype of the sender.</typeparam>
        /// <param name="message">The message to log.</param>
        void Debug<T>(string message);

        /// <summary>
        /// Logs a message with info severity.
        /// </summary>
        /// <typeparam name="T">The datatype of the sender.</typeparam>
        /// <param name="message">The message to log.</param>
        void Info<T>(string message);

        /// <summary>
        /// Logs a message with warning severity.
        /// </summary>
        /// <typeparam name="T">The datatype of the sender.</typeparam>
        /// <param name="message">The message to log.</param>
        void Warn<T>(string message);

        /// <summary>
        /// Logs a message with error severity.
        /// </summary>
        /// <typeparam name="T">The datatype of the sender.</typeparam>
        /// <param name="message">The message to log.</param>
        void Error<T>(string message);

        /// <summary>
        /// Logs message and entire stack trace of the provided exception.
        /// </summary>
        /// <typeparam name="T">The datatype of the sender.</typeparam>
        /// <param name="ex">The exception to log.</param>
        void Exception<T>(Exception ex);

        /// <summary>
        /// Logs a message with fatal severity.
        /// </summary>
        /// <typeparam name="T">The datatype of the sender.</typeparam>
        /// <param name="message">The message to log.</param>
        void Fatal<T>(string message);
    }
}