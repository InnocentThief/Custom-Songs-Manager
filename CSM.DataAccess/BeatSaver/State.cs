using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.BeatSaver
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum State
    {
        [EnumMember(Value = "Uploaded")]
        Uploaded,
        [EnumMember(Value = "Testplay")]
        Testplay,
        [EnumMember(Value = "Published")]
        Published,
        [EnumMember(Value = "Feedback")]
        Feedback,
        [EnumMember(Value = "Scheduled")]
        Scheduled,
    }
}
