using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.Framework.Logging
{
    public static class LoggerProvider
    {
        public static Logger Logger { get; private set; }

        public static void Register(Logger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}