using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.BeatSaver
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum Tag
    {
        [EnumMember(Value = "tech")]
        Tech,
        [EnumMember(Value = "dance-style")]
        DanceStyle,
        [EnumMember(Value = "speed")]
        Speed,
        [EnumMember(Value = "balanced")]
        Balanced,
        [EnumMember(Value = "challenge")]
        Challenge,
        [EnumMember(Value = "accuracy")]
        Accuracy,
        [EnumMember(Value = "fitness")]
        Fitness,
        [EnumMember(Value = "swing")]
        Swing,
        [EnumMember(Value = "nightcore")]
        Nightcore,
        [EnumMember(Value = "folk-acoustic")]
        FolkAccoustic,
        [EnumMember(Value = "kids-family")]
        KidsFamily,
        [EnumMember(Value = "ambient")]
        Ambient,
        [EnumMember(Value = "funk-disco")]
        FunkDisco,
        [EnumMember(Value = "jazz")]
        Jazz,
        [EnumMember(Value = "classical-orchestral")]
        ClassicalOrchestral,
        [EnumMember(Value = "soul")]
        Soul,
        [EnumMember(Value = "speedcore")]
        Speedcore,
        [EnumMember(Value = "punk")]
        Punk,
        [EnumMember(Value = "rb")]
        Rb,
        [EnumMember(Value = "holiday")]
        Holiday,
        [EnumMember(Value = "vocaloid")]
        Vocaloid,
        [EnumMember(Value = "j-rock")]
        JRock,
        [EnumMember(Value = "trance")]
        Trance,
        [EnumMember(Value = "drum-and-bass")]
        DrumAndBass,
        [EnumMember(Value = "comedy-meme")]
        ComedyMeme,
        [EnumMember(Value = "instrumental")]
        Instrumental,
        [EnumMember(Value = "hardcore")]
        Hardcore,
        [EnumMember(Value = "k-pop")]
        KPop,
        [EnumMember(Value = "indie")]
        Indie,
        [EnumMember(Value = "techno")]
        Techno,
        [EnumMember(Value = "house")]
        House,
        [EnumMember(Value = "video-game-soundtrack")]
        VideoGameSoundtrack,
        [EnumMember(Value = "tv-movie-soundtrack")]
        TvMovieSoundtrack,
        [EnumMember(Value = "alternative")]
        Alternative,
        [EnumMember(Value = "dubstep")]
        Dubstep,
        [EnumMember(Value = "metal")]
        Metal,
        [EnumMember(Value = "anime")]
        Anime,
        [EnumMember(Value = "hip-hop-rap")]
        HipHopRap,
        [EnumMember(Value = "j-pop")]
        JPop,
        [EnumMember(Value = "dance")]
        Dance,
        [EnumMember(Value = "rock")]
        Rock,
        [EnumMember(Value = "pop")]
        Pop,
        [EnumMember(Value = "electronic")]
        Electronic,
    }
}
