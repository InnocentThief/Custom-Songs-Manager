using System.Text.Json.Serialization;
using CSM.Framework.Helper;

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
