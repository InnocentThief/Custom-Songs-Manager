using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.BeatSaver
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum DeclaredAi
    {
        [EnumMember(Value = "Admin")]
        Admin,
        [EnumMember(Value = "Uploader")]
        Uploader,
        [EnumMember(Value = "SageScore")]
        SageScore,
        [EnumMember(Value = "None")]
        None
    }
}
