using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.BeatSaver
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum Environment
    {
        [EnumMember(Value = "DefaultEnvironment")]
        DefaultEnvironment,
        [EnumMember(Value = "TriangleEnvironment")]
        TriangleEnvironment,
        [EnumMember(Value = "NiceEnvironment")]
        NiceEnvironment,
        [EnumMember(Value = "BigMirrorEnvironment")]
        BigMirrorEnvironment,
        [EnumMember(Value = "KDAEnvironment")]
        KDAEnvironment,
        [EnumMember(Value = "MonstercatEnvironment")]
        MonstercatEnvironment,
        [EnumMember(Value = "CrabRaveEnvironment")]
        CrabRaveEnvironment,
        [EnumMember(Value = "DragonsEnvironment")]
        DragonsEnvironment,
        [EnumMember(Value = "OriginsEnvironment")]
        OriginsEnvironment,
        [EnumMember(Value = "PanicEnvironment")]
        PanicEnvironment,
        [EnumMember(Value = "RocketEnvironment")]
        RocketEnvironment,
        [EnumMember(Value = "GreenDayEnvironment")]
        GreenDayEnvironment,
        [EnumMember(Value = "GreenDayGrenadeEnvironment")]
        GreenDayGrenadeEnvironment,
        [EnumMember(Value = "TimbalandEnvironment")]
        TimbalandEnvironment,
        [EnumMember(Value = "FitBeatEnvironment")]
        FitBeatEnvironment,
        [EnumMember(Value = "LinkinParkEnvironment")]
        LinkinParkEnvironment,
        [EnumMember(Value = "BTSEnvironment")]
        BTSEnvironment,
        [EnumMember(Value = "KaleidoscopeEnvironment")]
        KaleidoscopeEnvironment,
        [EnumMember(Value = "InterscopeEnvironment")]
        InterscopeEnvironment,
        [EnumMember(Value = "SkrillexEnvironment")]
        SkrillexEnvironment,
        [EnumMember(Value = "BillieEnvironment")]
        BillieEnvironment,
        [EnumMember(Value = "HalloweenEnvironment")]
        HalloweenEnvironment,
        [EnumMember(Value = "GagaEnvironment")]
        GagaEnvironment,
        [EnumMember(Value = "GlassDesertEnvironment")]
        GlassDesertEnvironment,
        [EnumMember(Value = "MultiplayerEnvironment")]
        MultiplayerEnvironment,
        [EnumMember(Value = "WeaveEnvironment")]
        WeaveEnvironment,
        [EnumMember(Value = "PyroEnvironment")]
        PyroEnvironment,
        [EnumMember(Value = "EDMEnvironment")]
        EDMEnvironment,
        [EnumMember(Value = "TheSecondEnvironment")]
        TheSecondEnvironment,
        [EnumMember(Value = "LizzoEnvironment")]
        LizzoEnvironment,
        [EnumMember(Value = "TheWeekndEnvironment")]
        TheWeekndEnvironment,
        [EnumMember(Value = "RockMixtapeEnvironment")]
        RockMixtapeEnvironment,
        [EnumMember(Value = "Dragons2Environment")]
        Dragons2Environment,
        [EnumMember(Value = "Panic2Environment")]
        Panic2Environment,
        [EnumMember(Value = "QueenEnvironment")]
        QueenEnvironment,
        [EnumMember(Value = "LinkinPark2Environment")]
        LinkinPark2Environment,
        [EnumMember(Value = "TheRollingStonesEnvironment")]
        TheRollingStonesEnvironment,
        [EnumMember(Value = "LatticeEnvironment")]
        LatticeEnvironment,
        [EnumMember(Value = "DaftPunkEnvironment")]
        DaftPunkEnvironment,
        [EnumMember(Value = "HipHopEnvironment")]
        HipHopEnvironment,
        [EnumMember(Value = "ColliderEnvironment")]
        ColliderEnvironment,
        [EnumMember(Value = "BritneyEnvironment")]
        BritneyEnvironment,
        [EnumMember(Value = "Monstercat2Environment")]
        Monstercat2Environment,
        [EnumMember(Value = "MetallicaEnvironment")]
        MetallicaEnvironment
    }
}
