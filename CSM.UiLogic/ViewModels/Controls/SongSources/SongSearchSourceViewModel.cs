using System.Collections.ObjectModel;
using CSM.Business.Interfaces;
using CSM.DataAccess.BeatSaver;
using CSM.Framework.Extensions;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.ViewModels.Controls.SongSources.SongSearch;

namespace CSM.UiLogic.ViewModels.Controls.SongSources
{
    internal class SongSearchSourceViewModel : BaseViewModel, ISongSourceViewModel
    {
        #region Private fields

        private IRelayCommand? searchCommand, showMoreCommand, showFilterCommand, hideFilterCommand, resetFilterCommand, createPlaylistCommand, overwritePlaylistCommand, mergePlaylistCommand;
        private SearchQueryBuilder searchQueryBuilder = new();
        private bool filterVisible;
        private string query;
        private SearchResultMapDetailViewModel? selectedResult;
        private string? createPlaylistCommandText, overwritePlaylistCommandText, mergePlaylistCommandText;
        private string songCount = string.Empty;

        private readonly IBeatSaverService beatSaverService;

        #endregion

        #region Properties

        public IRelayCommand? SearchCommand => searchCommand ??= CommandFactory.CreateFromAsync(SearchAsync, CanSearch);
        public IRelayCommand? ShowMoreCommand => showMoreCommand ??= CommandFactory.CreateFromAsync(ShowMoreAsync, CanShowMore);
        public IRelayCommand? ShowFilterCommand => showFilterCommand ??= CommandFactory.Create(ShowFilter, CanShowFilter);
        public IRelayCommand? HideFilterCommand => hideFilterCommand ??= CommandFactory.Create(HideFilter, CanHideFilter);
        public IRelayCommand? ResetFilterCommand => resetFilterCommand ??= CommandFactory.Create(ResetFilter, CanResetFilter);
        public IRelayCommand? CreatePlaylistCommand => createPlaylistCommand ??= CommandFactory.Create(CreatePlaylist, CanCreatePlaylist);
        public IRelayCommand? OverwritePlaylistCommand => overwritePlaylistCommand ??= CommandFactory.Create(OverwritePlaylist, CanOverwritePlaylist);
        public IRelayCommand? MergePlaylistCommand => mergePlaylistCommand ??= CommandFactory.Create(MergePlaylist, CanMergePlaylist);

        public string? CreatePlaylistCommandText
        {
            get => createPlaylistCommandText;
            set
            {
                if (createPlaylistCommandText == value)
                    return;
                createPlaylistCommandText = value;
                OnPropertyChanged();
            }
        }

        public string? OverwritePlaylistCommandText
        {
            get => overwritePlaylistCommandText;
            set
            {
                if (overwritePlaylistCommandText == value)
                    return;
                overwritePlaylistCommandText = value;
                OnPropertyChanged();
            }
        }

        public string? MergePlaylistCommandText
        {
            get => mergePlaylistCommandText;
            set
            {
                if (mergePlaylistCommandText == value)
                    return;
                mergePlaylistCommandText = value;
                OnPropertyChanged();
            }
        }

        public bool FilterVisible
        {
            get => filterVisible;
            set
            {
                filterVisible = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShowResults));
                OnPropertyChanged(nameof(ShowNoResults));
                ShowFilterCommand?.RaiseCanExecuteChanged();
                HideFilterCommand?.RaiseCanExecuteChanged();
            }
        }

        public string Query
        {
            get => query;
            set
            {
                if (value == query)
                    return;
                query = value;
                OnPropertyChanged();
                SearchCommand?.RaiseCanExecuteChanged();
            }
        }

        public List<YesNoItem> AutoMapper { get; } = [
            new YesNoItem(null, "Human", true),
            new YesNoItem(true, "All"),
            new YesNoItem(false, "AI"),
        ];

        public List<YesNoItem> Curated { get; } = [
            new YesNoItem(null, string.Empty, true),
            new YesNoItem(true, "Yes"),
            new YesNoItem(false, "No"),
        ];

        public List<YesNoItem> Verified { get; } = [
            new YesNoItem(null, string.Empty, true),
            new YesNoItem(true, "Yes"),
            new YesNoItem(false, "No"),
        ];

        public List<YesNoItem> FullSpread { get; } = [
            new YesNoItem(null, string.Empty, true),
            new YesNoItem(true, "Yes"),
            new YesNoItem(false, "No"),
        ];

        public List<LeaderboardItem> Leaderboard { get; } = [
            new LeaderboardItem(null, string.Empty, true),
            new LeaderboardItem(SearchParamLeaderboard.All, "All"),
            new LeaderboardItem(SearchParamLeaderboard.Ranked, "Ranked"),
            new LeaderboardItem(SearchParamLeaderboard.BeatLeader, "BeatLeader"),
            new LeaderboardItem(SearchParamLeaderboard.ScoreSaber, "ScoreSaber"),
        ];

        public List<YesNoItem> Chroma { get; } = [
            new YesNoItem(null, string.Empty, true),
            new YesNoItem(true, "Yes"),
            new YesNoItem(false, "No"),
        ];

        public List<YesNoItem> NoodleExtensions { get; } = [
            new YesNoItem(null, string.Empty, true),
            new YesNoItem(true, "Yes"),
            new YesNoItem(false, "No"),
        ];

        public List<YesNoItem> MappingExtensions { get; } = [
            new YesNoItem(null, string.Empty, true),
            new YesNoItem(true, "Yes"),
            new YesNoItem(false, "No"),
        ];

        public List<YesNoItem> Cinema { get; } = [
            new YesNoItem(null, string.Empty, true),
            new YesNoItem(true, "Yes"),
            new YesNoItem(false, "No"),
        ];

        public List<YesNoItem> Vivify { get; } = [
            new YesNoItem(null, string.Empty, true),
            new YesNoItem(true, "Yes"),
            new YesNoItem(false, "No"),
        ];

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

        public ObservableCollection<SearchResultMapDetailViewModel> Results { get; } = [];

        public SearchResultMapDetailViewModel? SelectedResult
        {
            get => selectedResult;
            set
            {
                if (value == selectedResult)
                    return;
                selectedResult = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelectedResult));
            }
        }

        public bool HasSelectedResult => SelectedResult != null;

        public bool ShowResults => !FilterVisible && Results.Count > 0;

        public bool ShowNoResults => !FilterVisible && Results.Count == 0;

        public string SongCount
        {
            get => songCount;
            set
            {
                if (value == songCount)
                    return;
                songCount = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public SongSearchSourceViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            beatSaverService = serviceLocator.GetService<IBeatSaverService>();
        }

        #region Private fields

        private async Task SearchAsync()
        {
            Results.ForEach(result => result.CleanUpReferences());
            Results.Clear();

            SetSearchQueryBuilderData();
            var searchQuery = searchQueryBuilder.GetSearchQuery(0);
            if (string.IsNullOrWhiteSpace(searchQuery?.Query))
                return;
            var searchResult = await beatSaverService.SearchAsync(searchQuery.Query);
            if (searchResult == null || searchResult.Docs.Count == 0)
                return;
            Results.AddRange(searchResult.Docs.Select(mapDetail => new SearchResultMapDetailViewModel(ServiceLocator, mapDetail)));
            SongCount = $"Showing {searchResult.Docs.Count} from {searchResult.Info.Total} results";
            FilterVisible = false;
        }

        private void SetSearchQueryBuilderData()
        {
            var selectedAutoMapper = AutoMapper.SingleOrDefault(am => am.IsSelected);
            searchQueryBuilder.AI = selectedAutoMapper?.Key;

            var selectedChroma = Chroma.SingleOrDefault(c => c.IsSelected);
            searchQueryBuilder.Chroma = selectedChroma?.Key;

            var selectedCinema = Cinema.SingleOrDefault(c => c.IsSelected);
            searchQueryBuilder.Cinema = selectedCinema?.Key;

            var selectedCurated = Curated.SingleOrDefault(c => c.IsSelected);
            searchQueryBuilder.Curated = selectedCurated?.Key;

            var selectedFullSpread = FullSpread.SingleOrDefault(fs => fs.IsSelected);
            searchQueryBuilder.FullSpread = selectedFullSpread?.Key;

            var selectedMappingExtensions = MappingExtensions.SingleOrDefault(me => me.IsSelected);
            searchQueryBuilder.Me = selectedMappingExtensions?.Key;

            var selectedNoodleExtensions = NoodleExtensions.SingleOrDefault(ne => ne.IsSelected);
            searchQueryBuilder.Noodle = selectedNoodleExtensions?.Key;

            var selectedVerified = Verified.SingleOrDefault(v => v.IsSelected);
            searchQueryBuilder.Verified = selectedVerified?.Key;

            var selectedVivify = Vivify.SingleOrDefault(v => v.IsSelected);
            searchQueryBuilder.Vivify = selectedVivify?.Key;

            var selectedLeaderboard = Leaderboard.SingleOrDefault(lb => lb.IsSelected);
            searchQueryBuilder.Leaderboard = selectedLeaderboard?.Key;
            switch (selectedLeaderboard?.Key)
            {
                case SearchParamLeaderboard.All:
                    searchQueryBuilder.MaxBlStars = 16; // todo: get from slider
                    searchQueryBuilder.MaxSsStars = 16; // todo: get from slider
                    searchQueryBuilder.MinBlStars = 0; // todo: get from slider
                    searchQueryBuilder.MinSsStars = 0; // todo: get from slider
                    break;
                case SearchParamLeaderboard.Ranked:
                    searchQueryBuilder.MaxBlStars = 16; // todo: get from slider
                    searchQueryBuilder.MaxSsStars = 16; // todo: get from slider
                    searchQueryBuilder.MinBlStars = 0; // todo: get from slider
                    searchQueryBuilder.MinSsStars = 0; // todo: get from slider
                    break;
                case SearchParamLeaderboard.BeatLeader:
                    searchQueryBuilder.MaxBlStars = 16; // todo: get from slider
                    searchQueryBuilder.MinBlStars = 0; // todo: get from slider
                    break;
                case SearchParamLeaderboard.ScoreSaber:
                    searchQueryBuilder.MaxSsStars = 16; // todo: get from slider
                    searchQueryBuilder.MinSsStars = 0; // todo: get from slider
                    break;
                default:
                    searchQueryBuilder.MaxBlStars = null;
                    searchQueryBuilder.MaxSsStars = null;
                    searchQueryBuilder.MinBlStars = null;
                    searchQueryBuilder.MinSsStars = null;
                    break;
            }

            searchQueryBuilder.Query = Query;
        }

        private bool CanSearch()
        {
            return !string.IsNullOrWhiteSpace(Query);
        }

        private async Task ShowMoreAsync()
        {
        }

        private bool CanShowMore()
        {
            return true;
        }

        private void ShowFilter()
        {
            FilterVisible = true;
        }

        private bool CanShowFilter()
        {
            return !FilterVisible;
        }

        private void HideFilter()
        {
            FilterVisible = false;
        }

        private bool CanHideFilter()
        {
            return FilterVisible;
        }

        private void ResetFilter()
        {
            searchQueryBuilder.ResetSearchParameters();
        }

        private bool CanResetFilter()
        {
            return true;
        }

        private void CreatePlaylist()
        {
            //if (Playlist == null || Playlist.Songs.Count == 0)
            //{
            //    MessageBox.Show("No songs to copy.", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}

            //var createPlaylistEventArgs = new CreatePlaylistEventArgs
            //{
            //    PlaylistName = $"Suggested Songs {DateTime.Now:yyyy-MM-dd HH-mm-ss}",
            //    Songs = [.. Playlist.Songs.Select(x => x.Model)] // todo: only take filtered songs
            //};
            //songCopyDomain.CreatePlaylist(createPlaylistEventArgs);
        }

        private bool CanCreatePlaylist()
        {
            return true;
            //return songCopyDomain.SelectedPlaylist == null || songCopyDomain.SelectedPlaylist is PlaylistFolderViewModel;
        }
        private void OverwritePlaylist()
        {
        }

        private bool CanOverwritePlaylist()
        {
            return true;
            //return songCopyDomain.SelectedPlaylist != null && songCopyDomain.SelectedPlaylist is PlaylistViewModel;
        }
        private void MergePlaylist()
        {
        }

        private bool CanMergePlaylist()
        {
            return true;
            //return songCopyDomain.SelectedPlaylist != null && songCopyDomain.SelectedPlaylist is PlaylistViewModel;
        }

        #endregion
    }
}
