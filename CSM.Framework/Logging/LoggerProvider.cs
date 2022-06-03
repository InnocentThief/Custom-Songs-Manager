using System;

namespace CSM.Framework.Logging
{
    /// <summary>
    /// LoggerProvider for the Logger.
    /// </summary>
    public static class LoggerProvider
    {
        /// <summary>
        /// Main Logger.
        /// </summary>
        public static Logger Logger { get; private set; }

        /// <summary>
        /// Registers the given logger.
        /// </summary>
        public static void Register(Logger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}