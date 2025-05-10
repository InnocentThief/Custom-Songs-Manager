using System.Text.Json.Serialization;
using CSM.Framework.Helper;

namespace CSM.DataAccess.BeatSaver
{
    [JsonConverter(typeof(CaseInsensitiveJsonStringEnumConverter))]
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
