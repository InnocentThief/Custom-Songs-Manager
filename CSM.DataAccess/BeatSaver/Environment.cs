using System.Text.Json.Serialization;

namespace CSM.DataAccess.BeatSaver
{
    [JsonConverter(typeof(CaseInsensitiveJsonStringEnumConverter))]
    internal enum Environment
    {
        None,
        All,
        [JsonStringEnumMemberName("DefaultEnvironment")]
        DefaultEnvironment,
        [JsonStringEnumMemberName("TriangleEnvironment")]
        TriangleEnvironment,
        [JsonStringEnumMemberName("NiceEnvironment")]
        NiceEnvironment,
        [JsonStringEnumMemberName("BigMirrorEnvironment")]
        BigMirrorEnvironment,
        [JsonStringEnumMemberName("KDAEnvironment")]
        KDAEnvironment,
        [JsonStringEnumMemberName("MonstercatEnvironment")]
        MonstercatEnvironment,
        [JsonStringEnumMemberName("CrabRaveEnvironment")]
        CrabRaveEnvironment,
        [JsonStringEnumMemberName("DragonsEnvironment")]
        DragonsEnvironment,
        [JsonStringEnumMemberName("OriginsEnvironment")]
        OriginsEnvironment,
        [JsonStringEnumMemberName("PanicEnvironment")]
        PanicEnvironment,
        [JsonStringEnumMemberName("RocketEnvironment")]
        RocketEnvironment,
        [JsonStringEnumMemberName("GreenDayEnvironment")]
        GreenDayEnvironment,
        [JsonStringEnumMemberName("GreenDayGrenadeEnvironment")]
        GreenDayGrenadeEnvironment,
        [JsonStringEnumMemberName("TimbalandEnvironment")]
        TimbalandEnvironment,
        [JsonStringEnumMemberName("FitBeatEnvironment")]
        FitBeatEnvironment,
        [JsonStringEnumMemberName("LinkinParkEnvironment")]
        LinkinParkEnvironment,
        [JsonStringEnumMemberName("BTSEnvironment")]
        BTSEnvironment,
        [JsonStringEnumMemberName("KaleidoscopeEnvironment")]
        KaleidoscopeEnvironment,
        [JsonStringEnumMemberName("InterscopeEnvironment")]
        InterscopeEnvironment,
        [JsonStringEnumMemberName("SkrillexEnvironment")]
        SkrillexEnvironment,
        [JsonStringEnumMemberName("BillieEnvironment")]
        BillieEnvironment,
        [JsonStringEnumMemberName("HalloweenEnvironment")]
        HalloweenEnvironment,
        [JsonStringEnumMemberName("GagaEnvironment")]
        GagaEnvironment,
        [JsonStringEnumMemberName("GlassDesertEnvironment")]
        GlassDesertEnvironment,
        [JsonStringEnumMemberName("MultiplayerEnvironment")]
        MultiplayerEnvironment,
        [JsonStringEnumMemberName("WeaveEnvironment")]
        WeaveEnvironment,
        [JsonStringEnumMemberName("PyroEnvironment")]
        PyroEnvironment,
        [JsonStringEnumMemberName("EDMEnvironment")]
        EDMEnvironment,
        [JsonStringEnumMemberName("TheSecondEnvironment")]
        TheSecondEnvironment,
        [JsonStringEnumMemberName("LizzoEnvironment")]
        LizzoEnvironment,
        [JsonStringEnumMemberName("TheWeekndEnvironment")]
        TheWeekndEnvironment,
        [JsonStringEnumMemberName("RockMixtapeEnvironment")]
        RockMixtapeEnvironment,
        [JsonStringEnumMemberName("Dragons2Environment")]
        Dragons2Environment,
        [JsonStringEnumMemberName("Panic2Environment")]
        Panic2Environment,
        [JsonStringEnumMemberName("QueenEnvironment")]
        QueenEnvironment,
        [JsonStringEnumMemberName("LinkinPark2Environment")]
        LinkinPark2Environment,
        [JsonStringEnumMemberName("TheRollingStonesEnvironment")]
        TheRollingStonesEnvironment,
        [JsonStringEnumMemberName("LatticeEnvironment")]
        LatticeEnvironment,
        [JsonStringEnumMemberName("DaftPunkEnvironment")]
        DaftPunkEnvironment,
        [JsonStringEnumMemberName("HipHopEnvironment")]
        HipHopEnvironment,
        [JsonStringEnumMemberName("ColliderEnvironment")]
        ColliderEnvironment,
        [JsonStringEnumMemberName("BritneyEnvironment")]
        BritneyEnvironment,
        [JsonStringEnumMemberName("Monstercat2Environment")]
        Monstercat2Environment,
        [JsonStringEnumMemberName("MetallicaEnvironment")]
        MetallicaEnvironment
    }

    public static class EnvironmentExtensions
    {
        public static Dictionary<Type, Dictionary<string, object>> GetCustomMappings()
        {
            return new Dictionary<Type, Dictionary<string, object>>
            {
                {
                    typeof(Environment),
                    new Dictionary<string, object>
                    {
                        { "none", Environment.None },
                        { "all", Environment.All },
                        { "default_environment", Environment.DefaultEnvironment },
                        { "triangle_environment", Environment.TriangleEnvironment },
                        { "nice_environment", Environment.NiceEnvironment },
                        { "big_mirror_environment", Environment.BigMirrorEnvironment },
                        { "kda_environment", Environment.KDAEnvironment },
                        { "monstercat_environment", Environment.MonstercatEnvironment },
                        { "crab_rave_environment", Environment.CrabRaveEnvironment },
                        { "dragons_environment", Environment.DragonsEnvironment },
                        { "origins_environment", Environment.OriginsEnvironment },
                        { "panic_environment", Environment.PanicEnvironment },
                        { "rocket_environment", Environment.RocketEnvironment },
                        { "green_day_environment", Environment.GreenDayEnvironment },
                        { "green_day_grenade_environment", Environment.GreenDayGrenadeEnvironment },
                        { "timbaland_environment", Environment.TimbalandEnvironment },
                        { "fit_beat_environment", Environment.FitBeatEnvironment },
                        { "linkin_park_environment", Environment.LinkinParkEnvironment },
                        { "bts_environment", Environment.BTSEnvironment },
                        { "kaleidoscope_environment", Environment.KaleidoscopeEnvironment },
                        { "interscope_environment", Environment.InterscopeEnvironment },
                        { "skrillex_environment", Environment.SkrillexEnvironment },
                        { "billie_environment", Environment.BillieEnvironment },
                        { "halloween_environment", Environment.HalloweenEnvironment },
                        { "gaga_environment", Environment.GagaEnvironment },
                        { "glass_desert_environment", Environment.GlassDesertEnvironment },
                        { "multiplayer_environment", Environment.MultiplayerEnvironment },
                        { "weave_environment", Environment.WeaveEnvironment },
                        { "pyro_environment", Environment.PyroEnvironment },
                        { "edm_environment", Environment.EDMEnvironment },
                        { "the_second_environment", Environment.TheSecondEnvironment },
                        { "lizzo_environment", Environment.LizzoEnvironment },
                        { "the_weeknd_environment", Environment.TheWeekndEnvironment },
                        { "rock_mixtape_environment", Environment.RockMixtapeEnvironment },
                        { "dragons2_environment", Environment.Dragons2Environment },
                        { "panic2_environment", Environment.Panic2Environment },
                        { "queen_environment", Environment.QueenEnvironment },
                        { "linkin_park2_environment", Environment.LinkinPark2Environment },
                        { "the_rolling_stones_environment", Environment.TheRollingStonesEnvironment },
                        { "lattice_environment", Environment.LatticeEnvironment },
                        { "daft_punk_environment", Environment.DaftPunkEnvironment },
                        { "hip_hop_environment", Environment.HipHopEnvironment },
                        { "collider_environment", Environment.ColliderEnvironment },
                        { "britney_environment", Environment.BritneyEnvironment },
                        { "monstercat2_environment", Environment.Monstercat2Environment },
                        { "metallica_environment", Environment.MetallicaEnvironment }
                    }
                }
            };
        }
    }
}
