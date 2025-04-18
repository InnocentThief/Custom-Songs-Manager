using System.Text.Json;

namespace CSM.UiLogic.Helper
{
    internal static class JsonSerializerHelper
    {
        public static JsonSerializerOptions CreateDefaultSerializerOptions()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return options;
        }
    }
}
