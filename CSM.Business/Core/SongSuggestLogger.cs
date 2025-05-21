using Microsoft.Extensions.Logging;
using System.Text;

namespace CSM.Business.Core
{
    internal class SongSuggestLogger(ILogger logger) : TextWriter
    {
        private readonly ILogger? logger = logger;

        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            logger?.LogInformation(value);

        }
    }
}
