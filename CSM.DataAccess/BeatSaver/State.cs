using System.Text.Json.Serialization;

namespace CSM.DataAccess.BeatSaver
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum State
    {
        [JsonStringEnumMemberName("Uploaded")]
        Uploaded,
        [JsonStringEnumMemberName("Testplay")]
        Testplay,
        [JsonStringEnumMemberName("Published")]
        Published,
        [JsonStringEnumMemberName("Feedback")]
        Feedback,
        [JsonStringEnumMemberName("Scheduled")]
        Scheduled,
    }
}
