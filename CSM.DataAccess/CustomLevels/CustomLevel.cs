using System.Text.Json.Serialization;

namespace CSM.DataAccess.CustomLevels
{
    internal class CustomLevel
    {
        [JsonPropertyName("_songName")]
        public string SongName { get; set; } = string.Empty;
    }
}
