using System.Text.Json.Serialization;

namespace CSM.DataAccess.Common
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Characteristic
    {
        [JsonStringEnumMemberName("Standard")]
        Standard,
        [JsonStringEnumMemberName("OneSaber")]
        OneSaber,
        [JsonStringEnumMemberName("NoArrows")]
        NoArrows,
        [JsonStringEnumMemberName("90Degree")]
        Degree90,
        [JsonStringEnumMemberName("360Degree")]
        Degree360,
        [JsonStringEnumMemberName("Lightshow")]
        Lightshow,
        [JsonStringEnumMemberName("Lawless")]
        Lawless,
        [JsonStringEnumMemberName("Legacy")]
        Legacy
    }
}
