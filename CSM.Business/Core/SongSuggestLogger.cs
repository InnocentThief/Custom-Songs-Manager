using Microsoft.Extensions.Logging;
using System.Text;

namespace CSM.Business.Core
{
    internal class SongSuggestLogger : TextWriter
    {
        private ILogger? Logger;

        public override Encoding Encoding => Encoding.UTF8;

        public SongSuggestLogger(ILogger logger)
        {
            Logger = logger;
        }

        public override void Write(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            Logger?.LogInformation(value);

        }
    }
}
