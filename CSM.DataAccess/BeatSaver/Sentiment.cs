using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.BeatSaver
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum Sentiment
    {
        [EnumMember(Value = "PENDING")]
        Pending,
        [EnumMember(Value = "VERY_NEGATIVE")]
        VeryNegative,
        [EnumMember(Value = "MOSTLY_NEGATIVE")]
        MostlyNegative,
        [EnumMember(Value = "MIXED")]
        Mixed,
        [EnumMember(Value = "MOSTLY_POSITIVE")]
        MostlyPositive,
        [EnumMember(Value = "VERY_POSITIVE")]
        VeryPositive
    }
}
