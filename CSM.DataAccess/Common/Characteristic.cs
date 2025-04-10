using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.Common
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum Characteristic
    {
        [EnumMember(Value = "Standard")]
        Standard,
        [EnumMember(Value = "OneSaber")]
        OneSaber,
        [EnumMember(Value = "NoArrows")]
        NoArrows,
        [EnumMember(Value = "90Degree")]
        Degree90,
        [EnumMember(Value = "360Degree")]
        Degree360,
        [EnumMember(Value = "Lightshow")]
        Lightshow,
        [EnumMember(Value = "Lawless")]
        Lawless,
        [EnumMember(Value = "Legacy")]
        Legacy
    }
}
