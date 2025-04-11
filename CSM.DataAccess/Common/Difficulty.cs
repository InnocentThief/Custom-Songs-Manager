using System.Text.Json.Serialization;

namespace CSM.DataAccess.Common
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum Difficulty
    {
        [JsonStringEnumMemberName("Easy")]
        Easy = 1,
        [JsonStringEnumMemberName("Normal")]
        Normal = 3,
        [JsonStringEnumMemberName("Hard")]
        Hard = 5,
        [JsonStringEnumMemberName("Expert")]
        Expert = 7,
        [JsonStringEnumMemberName("ExpertPlus")]
        ExpertPlus = 9,
    }
}
