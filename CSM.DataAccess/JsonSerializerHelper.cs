using System.Text.Json;
using CSM.DataAccess.BeatSaver;
using CSM.DataAccess.Common;
using CSM.Framework.Helper;

namespace CSM.DataAccess
{
    internal static class JsonSerializerHelper
    {
        private static JsonSerializerOptions? options;

        public static JsonSerializerOptions CreateDefaultSerializerOptions()
        {
            if (options != null)
            {
                return options;
            }

            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
                Converters =
                {
                    new CaseInsensitiveJsonStringEnumConverter(GetCustomMappings()),
                }
            };
            return options;
        }

        public static Dictionary<Type, Dictionary<string, object>> GetCustomMappings()
        {
            return new Dictionary<Type, Dictionary<string, object>>
            {
                {
                    typeof(DeclaredAi), new Dictionary<string, object>
                    {
                        { "admin", DeclaredAi.Admin },
                        { "uploader", DeclaredAi.Uploader },
                        { "sagescore", DeclaredAi.SageScore },
                        { "none", DeclaredAi.None }
                    }
                },
                {
                    typeof(BeatSaver.Environment),
                    new Dictionary<string, object>
                    {
                        { "none", BeatSaver.Environment.None },
                        { "all", BeatSaver.Environment.All },
                        { "default_environment", BeatSaver.Environment.DefaultEnvironment },
                        { "triangle_environment", BeatSaver.Environment.TriangleEnvironment },
                        { "nice_environment", BeatSaver.Environment.NiceEnvironment },
                        { "big_mirror_environment", BeatSaver.Environment.BigMirrorEnvironment },
                        { "kda_environment", BeatSaver.Environment.KDAEnvironment },
                        { "monstercat_environment", BeatSaver.Environment.MonstercatEnvironment },
                        { "crab_rave_environment", BeatSaver.Environment.CrabRaveEnvironment },
                        { "dragons_environment", BeatSaver.Environment.DragonsEnvironment },
                        { "origins_environment", BeatSaver.Environment.OriginsEnvironment },
                        { "panic_environment", BeatSaver.Environment.PanicEnvironment },
                        { "rocket_environment", BeatSaver.Environment.RocketEnvironment },
                        { "green_day_environment", BeatSaver.Environment.GreenDayEnvironment },
                        { "green_day_grenade_environment", BeatSaver.Environment.GreenDayGrenadeEnvironment },
                        { "timbaland_environment", BeatSaver.Environment.TimbalandEnvironment },
                        { "fit_beat_environment", BeatSaver.Environment.FitBeatEnvironment },
                        { "linkin_park_environment", BeatSaver.Environment.LinkinParkEnvironment },
                        { "bts_environment", BeatSaver.Environment.BTSEnvironment },
                        { "kaleidoscope_environment", BeatSaver.Environment.KaleidoscopeEnvironment },
                        { "interscope_environment", BeatSaver.Environment.InterscopeEnvironment },
                        { "skrillex_environment", BeatSaver.Environment.SkrillexEnvironment },
                        { "billie_environment", BeatSaver.Environment.BillieEnvironment },
                        { "halloween_environment", BeatSaver.Environment.HalloweenEnvironment },
                        { "gaga_environment", BeatSaver.Environment.GagaEnvironment },
                        { "glass_desert_environment", BeatSaver.Environment.GlassDesertEnvironment },
                        { "multiplayer_environment", BeatSaver.Environment.MultiplayerEnvironment },
                        { "weave_environment", BeatSaver.Environment.WeaveEnvironment },
                        { "pyro_environment", BeatSaver.Environment.PyroEnvironment },
                        { "edm_environment", BeatSaver.Environment.EDMEnvironment },
                        { "the_second_environment", BeatSaver.Environment.TheSecondEnvironment },
                        { "lizzo_environment", BeatSaver.Environment.LizzoEnvironment },
                        { "the_weeknd_environment", BeatSaver.Environment.TheWeekndEnvironment },
                        { "rock_mixtape_environment", BeatSaver.Environment.RockMixtapeEnvironment },
                        { "dragons2_environment", BeatSaver.Environment.Dragons2Environment },
                        { "panic2_environment", BeatSaver.Environment.Panic2Environment },
                        { "queen_environment", BeatSaver.Environment.QueenEnvironment },
                        { "linkin_park2_environment", BeatSaver.Environment.LinkinPark2Environment },
                        { "the_rolling_stones_environment", BeatSaver.Environment.TheRollingStonesEnvironment },
                        { "lattice_environment", BeatSaver.Environment.LatticeEnvironment },
                        { "daft_punk_environment", BeatSaver.Environment.DaftPunkEnvironment },
                        { "hip_hop_environment", BeatSaver.Environment.HipHopEnvironment },
                        { "collider_environment", BeatSaver.Environment.ColliderEnvironment },
                        { "britney_environment", BeatSaver.Environment.BritneyEnvironment },
                        { "monstercat2_environment", BeatSaver.Environment.Monstercat2Environment },
                        { "metallica_environment", BeatSaver.Environment.MetallicaEnvironment }
                    }
                },
                {
                    typeof(Sentiment), new Dictionary<string, object>
                    {
                        { "pending", Sentiment.Pending },
                        { "very_negative", Sentiment.VeryNegative },
                        { "mostly_negative", Sentiment.MostlyNegative },
                        { "mixed", Sentiment.Mixed },
                        { "mostly_positive", Sentiment.MostlyPositive },
                        { "very_positive", Sentiment.VeryPositive }
                    }
                },
                {
                    typeof(State), new Dictionary<string, object>
                    {
                        { "uploaded", State.Uploaded },
                        { "testplay", State.Testplay },
                        { "published", State.Published },
                        { "feedback", State.Feedback },
                        { "scheduled", State.Scheduled }
                    }
                },
                {
                    typeof(Tag), new Dictionary<string, object>
                    {
                        { "none", Tag.None },
                        { "tech", Tag.Tech },
                        { "dance-style", Tag.DanceStyle },
                        { "speed", Tag.Speed },
                        { "balanced", Tag.Balanced },
                        { "challenge", Tag.Challenge },
                        { "accuracy", Tag.Accuracy },
                        { "fitness", Tag.Fitness },
                        { "swing", Tag.Swing },
                        { "nightcore", Tag.Nightcore },
                        { "folk-acoustic", Tag.FolkAccoustic },
                        { "kids-family", Tag.KidsFamily },
                        { "ambient", Tag.Ambient },
                        { "funk-disco", Tag.FunkDisco },
                        { "jazz", Tag.Jazz },
                        { "classical-orchestral", Tag.ClassicalOrchestral },
                        { "soul", Tag.Soul },
                        { "speedcore", Tag.Speedcore },
                        { "punk", Tag.Punk },
                        { "rb", Tag.Rb },
                        { "holiday", Tag.Holiday },
                        { "vocaloid", Tag.Vocaloid },
                        { "j-rock", Tag.JRock },
                        { "trance", Tag.Trance },
                        { "drum-and-bass", Tag.DrumAndBass },
                        { "comedy-meme", Tag.ComedyMeme },
                        { "instrumental", Tag.Instrumental },
                        { "hardcore", Tag.Hardcore },
                        { "k-pop", Tag.KPop },
                        { "indie", Tag.Indie },
                        { "techno", Tag.Techno },
                        { "house", Tag.House },
                        { "video-game-soundtrack", Tag.VideoGameSoundtrack },
                        { "tv-movie-soundtrack", Tag.TvMovieSoundtrack },
                        { "alternative", Tag.Alternative },
                        { "dubstep", Tag.Dubstep },
                        { "metal", Tag.Metal },
                        { "anime", Tag.Anime },
                        { "hip-hop-rap", Tag.HipHopRap },
                        { "j-pop", Tag.JPop },
                        { "dance", Tag.Dance },
                        { "rock", Tag.Rock },
                        { "pop", Tag.Pop },
                        { "electronic", Tag.Electronic }
                    }
                },
                {
                    typeof(Characteristic), new Dictionary<string, object>
                    {
                        { "standard", Characteristic.Standard },
                        { "oneSaber", Characteristic.OneSaber },
                        { "noArrows", Characteristic.NoArrows },
                        { "90Degree", Characteristic.Degree90 },
                        { "360Degree", Characteristic.Degree360 },
                        { "lightshow", Characteristic.Lightshow },
                        { "lawless", Characteristic.Lawless },
                        { "legacy", Characteristic.Legacy }
                    }
                },
                {
                    typeof(Difficulty), new Dictionary<string, object>
                    {
                        { "easy", Difficulty.Easy },
                        { "normal", Difficulty.Normal },
                        { "hard", Difficulty.Hard },
                        { "expert", Difficulty.Expert },
                        { "expertPlus", Difficulty.ExpertPlus },
                        { "expert+", Difficulty.ExpertPlus },
                    }
                }
            };
        }

    }
}
