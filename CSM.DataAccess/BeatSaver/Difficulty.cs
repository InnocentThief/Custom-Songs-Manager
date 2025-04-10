using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.BeatSaver
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum Difficulty
    {
        [EnumMember(Value = "Easy")]
        Easy = 1,
        [EnumMember(Value = "Normal")]
        Normal = 3,
        [EnumMember(Value = "Hard")]
        Hard = 5,
        [EnumMember(Value = "Expert")]
        Expert = 7,
        [EnumMember(Value = "ExpertPlus")]
        ExpertPlus = 9,
    }
}
