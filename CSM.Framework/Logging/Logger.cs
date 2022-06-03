using log4net;
using log4net.Config;
using System;
using System.IO;

namespace CSM.Framework.Logging
{
    /// <summary>
    /// Logger for exceptions.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Initializes a new <see cref="Logger"/>.
        /// </summary>
        private Logger()
        {
            XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
        }

        /// <summary>
        /// Creates an instance of the logger.
        /// </summary>
        public static void Create()
        {
            LoggerProvider.Register(new Logger());
        }

        /// <summary>
        /// Logs a message with debug severity.
        /// </summary>
        /// <typeparam name="T">The datatype of the sender.</typeparam>
        /// <param name="message">The message to log.</param>
        public void Debug<T>(string message)
        {
            GetLog<T>().Debug(message);
        }

        /// <summary>
        /// Logs a message with info severity.
        /// </summary>
        /// <typeparam name="T">The datatype of the sender.</typeparam>
        /// <param name="message">The message to log.</param>
        public void Info<T>(string message)
        {
            GetLog<T>().Info(message);
        }

        /// <summary>
        /// Logs a message with warning severity.
        /// </summary>
        /// <typeparam name="T">The datatype of the sender.</typeparam>
        /// <param name="message">The message to log.</param>
        public void Warn<T>(string message)
        {
            GetLog<T>().Warn(message);
        }

        /// <summary>
        /// Logs a message with error severity.
        /// </summary>
        /// <typeparam name="T">The datatype of the sender.</typeparam>
        /// <param name="message">The message to log.</param>
        public void Error<T>(string message)
        {
            GetLog<T>().Error(message);
        }

        /// <summary>
        /// Logs message and entire stack trace of the provided exception.
        /// </summary>
        /// <typeparam name="T">The datatype of the sender.</typeparam>
        /// <param name="ex">The exception to log.</param>
        public void Exception<T>(Exception ex)
        {
            GetLog<T>().Error(ex.ToString());
        }

        /// <summary>
        /// Logs a message with fatal severity.
        /// </summary>
        /// <typeparam name="T">The datatype of the sender.</typeparam>
        /// <param name="message">The message to log.</param>
        public void Fatal<T>(string message)
        {
            GetLog<T>().Fatal(message);
        }

        #region Helper methods

        private static ILog GetLog<T>()
        {
            var logger = LogManager.GetLogger(typeof(T));
            return logger;
        }

        #endregion
    }
}