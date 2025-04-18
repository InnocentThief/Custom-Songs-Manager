using CSM.DataAccess.BeatSaver;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Controls.SongSources.SongSearch;

namespace CSM.UiLogic.ViewModels.Controls.SongSources
{
    internal class SongSearchSourceViewModel : BaseViewModel, ISongSourceViewModel
    {
        private SearchQueryBuilder searchQueryBuilder = new SearchQueryBuilder();

        public List<StyleItem> MapStyles { get; } = [
            new StyleItem(Tag.None, string.Empty, true),
            new StyleItem(Tag.Accuracy, "Accuracy"),
            new StyleItem(Tag.Balanced, "Balanced"),
            new StyleItem(Tag.Challenge, "Challenge"),
            new StyleItem(Tag.DanceStyle, "Dance"),
            new StyleItem(Tag.Fitness, "Fitness"),
            new StyleItem(Tag.Speed, "Speed"),
            new StyleItem(Tag.Tech, "Tech"),
        ];

        public List<StyleItem> SongStyles { get; } = [
            new StyleItem(Tag.None, string.Empty, true),
            new StyleItem(Tag.Alternative, "Alternative"),
            new StyleItem(Tag.Ambient, "Ambient"),
            new StyleItem(Tag.Anime, "Anime"),
            new StyleItem(Tag.ClassicalOrchestral, "Classical & Orchestral"),
            new StyleItem(Tag.ComedyMeme, "Comedy & Meme"),
            new StyleItem(Tag.Dance, "Dance"),
            new StyleItem(Tag.DrumAndBass, "Drum and Bass"),
            new StyleItem(Tag.Dubstep, "Dubstep"),
            new StyleItem(Tag.Electronic, "Electronic"),
            new StyleItem(Tag.FolkAccoustic, "Folk & Acoustic"),
            new StyleItem(Tag.FunkDisco, "Funk & Disco"),
            new StyleItem(Tag.Hardcore, "Hardcore"),
            new StyleItem(Tag.HipHopRap, "Hip Hop & Rap"),
            new StyleItem(Tag.Holiday, "Holiday"),
            new StyleItem(Tag.House, "House"),
            new StyleItem(Tag.Indie, "Indie"),
            new StyleItem(Tag.Instrumental, "Instrumental"),
            new StyleItem(Tag.JPop, "J-Pop"),
            new StyleItem(Tag.JRock, "J-Rock"),
            new StyleItem(Tag.Jazz, "Jazz"),
            new StyleItem(Tag.KPop, "K-Pop"),
            new StyleItem(Tag.KidsFamily, "Kids & Family"),
            new StyleItem(Tag.Metal, "Metal"),
            new StyleItem(Tag.Nightcore, "Nightcore"),
            new StyleItem(Tag.Pop, "Pop"),
            new StyleItem(Tag.Punk, "Punk"),
            new StyleItem(Tag.Rb, "R&B"),
            new StyleItem(Tag.Rock, "Rock"),
            new StyleItem(Tag.Soul, "Soul"),
            new StyleItem(Tag.Speedcore, "Speedcore"),
            new StyleItem(Tag.Swing, "Swing"),
            new StyleItem(Tag.TvMovieSoundtrack, "TV & Film"),
            new StyleItem(Tag.Techno, "Techno"),
            new StyleItem(Tag.Trance, "Trance"),
            new StyleItem(Tag.VideoGameSoundtrack, "Video Game"),
            new StyleItem(Tag.Vocaloid, "Vocaloid"),
        ];

        public List<EnvironmentItem> LegacyEnvironments { get; } = [
            new EnvironmentItem(DataAccess.BeatSaver.Environment.None, string.Empty, true),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.All, "All"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.DefaultEnvironment, "Default"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.TriangleEnvironment, "Triangle"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.NiceEnvironment, "Nice"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.BigMirrorEnvironment, "Big Mirror"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.KDAEnvironment, "KDA"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.MonstercatEnvironment, "Monstercat"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.CrabRaveEnvironment, "Crab Rave"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.DragonsEnvironment, "Dragons"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.OriginsEnvironment, "Origins"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.PanicEnvironment, "Panic"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.RocketEnvironment, "Rocket"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.GreenDayEnvironment, "Green Day"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.GreenDayGrenadeEnvironment, "Green Day Grenade"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.TimbalandEnvironment, "Timbaland"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.FitBeatEnvironment, "Fitbeat"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.LinkinParkEnvironment, "Linkin Park"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.BTSEnvironment, "BTS"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.KaleidoscopeEnvironment, "Kaleidoscope"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.InterscopeEnvironment, "Interscope"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.SkrillexEnvironment, "Skrillex"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.BillieEnvironment, "Billie"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.HalloweenEnvironment, "Halloween"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.GagaEnvironment, "Gaga"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.GlassDesertEnvironment, "Glass Desert")
        ];

        public List<EnvironmentItem> NewEnvironments { get; } = [
            new EnvironmentItem(DataAccess.BeatSaver.Environment.None, string.Empty, true),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.All, "All"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.WeaveEnvironment, "Weave"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.PyroEnvironment, "Pyro"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.EDMEnvironment, "EDM"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.TheSecondEnvironment, "The Second"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.LizzoEnvironment, "Lizzo"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.TheWeekndEnvironment, "The Weeknd"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.RockMixtapeEnvironment, "Rock Mixtape"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.Dragons2Environment, "Dragons 2"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.Panic2Environment, "Panic 2"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.QueenEnvironment, "Queen"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.LinkinPark2Environment, "Linkin Park 2"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.TheRollingStonesEnvironment, "The Rolling Stones"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.LatticeEnvironment, "Lattice"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.DaftPunkEnvironment, "Daft Punk"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.HipHopEnvironment, "Hip Hop"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.ColliderEnvironment, "Collider"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.BritneyEnvironment, "Britney"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.Monstercat2Environment, "Monstercat 2"),
            new EnvironmentItem(DataAccess.BeatSaver.Environment.MetallicaEnvironment, "Metallica")
        ];

        public SongSearchSourceViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }
    }
}
