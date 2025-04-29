using System.Text.Json.Serialization;

namespace CSM.DataAccess.BeatSaver
{
    [JsonConverter(typeof(CaseInsensitiveJsonStringEnumConverter))]
    internal enum DeclaredAi
    {
        [JsonStringEnumMemberName("Admin")]
        Admin,
        [JsonStringEnumMemberName("Uploader")]
        Uploader,
        [JsonStringEnumMemberName("SageScore")]
        SageScore,
        [JsonStringEnumMemberName("None")]
        None
    }
}
