using System.Text.Json.Serialization;

namespace CSM.DataAccess.BeatSaver
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum Sentiment
    {
        [JsonStringEnumMemberName("PENDING")]
        Pending,
        [JsonStringEnumMemberName("VERY_NEGATIVE")]
        VeryNegative,
        [JsonStringEnumMemberName("MOSTLY_NEGATIVE")]
        MostlyNegative,
        [JsonStringEnumMemberName("MIXED")]
        Mixed,
        [JsonStringEnumMemberName("MOSTLY_POSITIVE")]
        MostlyPositive,
        [JsonStringEnumMemberName("VERY_POSITIVE")]
        VeryPositive
    }
}
