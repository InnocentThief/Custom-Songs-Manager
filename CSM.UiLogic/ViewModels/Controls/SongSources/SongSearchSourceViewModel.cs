using CSM.Business.Core.SongCopy;
using CSM.Business.Interfaces;
using CSM.DataAccess.BeatSaver;
using CSM.DataAccess.Playlists;
using CSM.Framework.Extensions;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.Helper;
using CSM.UiLogic.ViewModels.Common.Playlists;
using CSM.UiLogic.ViewModels.Controls.SongSources.SongSearch;
using System.Collections.ObjectModel;

namespace CSM.UiLogic.ViewModels.Controls.SongSources
{
    internal class SongSearchSourceViewModel : BaseViewModel, ISongSourceViewModel
    {
        #region Private fields

        private IRelayCommand? searchCommand, showMoreCommand, showFilterCommand, hideFilterCommand, resetFilterCommand, createPlaylistCommand, overwritePlaylistCommand, mergePlaylistCommand;
        private readonly SearchQueryBuilder searchQueryBuilder = new();
        private bool filterVisible;
        private string query = string.Empty;
        private SearchResultMapDetailViewModel? selectedResult;
        private string? createPlaylistCommandText, overwritePlaylistCommandText, mergePlaylistCommandText;
        private string songCount = string.Empty;
        private double npsStart, npsEnd = 16, starsStart, starsEnd = 16;
        private int votesStart, votesEnd = 1000, upVotesStart, upVotesEnd = 1000, downVotesStart, downVotesEnd = 1000;
        private DateTime? dateSelectionStart;
        private DateTime? dateSelectionEnd;
        private EnumWrapper<SearchParamRelevance>? selectedRelevance;
        private int currentPageIndex = 0;

        private readonly IBeatSaverService beatSaverService;
        private readonly ISongCopyDomain songCopyDomain;

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

        public ObservableCollection<EnumWrapper<SearchParamRelevance>> Relevances { get; } = [];

        public EnumWrapper<SearchParamRelevance>? SelectedRelevance
        {
            get => selectedRelevance;
            set
            {
                if (value == selectedRelevance)
                    return;
                selectedRelevance = value;
                OnPropertyChanged();
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

        public string NPSRange
        {
            get
            {
                var min = NPSStart.ToString("0.0");
                var max = NPSEnd == 16 ? "∞" : NPSEnd.ToString("0.0");
                return $"{min} - {max}";
            }
        }

        public double NPSStart
        {
            get => npsStart;
            set
            {
                if (value == npsStart)
                    return;
                npsStart = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NPSRange));
            }
        }

        public double NPSEnd
        {
            get => npsEnd;
            set
            {
                if (value == npsEnd)
                    return;
                npsEnd = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NPSRange));
            }
        }

        public static DateTime DateMinimum => new(2018, 5, 8);

        public static DateTime DateMaximum => DateTime.Today;

        public DateTime? DateSelectionStart
        {
            get => dateSelectionStart;
            set
            {
                if (value == dateSelectionStart) return;
                dateSelectionStart = value;
                OnPropertyChanged();
            }
        }

        public DateTime? DateSelectionEnd
        {
            get => dateSelectionEnd;
            set
            {
                if (value == dateSelectionEnd) return;
                dateSelectionEnd = value;
                OnPropertyChanged();
            }
        }

        public string Stars
        {
            get
            {
                var min = StarsStart.ToString("0.0");
                var max = StarsEnd == 16 ? "∞" : StarsEnd.ToString("0.0");
                return $"{min} - {max}";
            }
        }

        public static double StarsMinimum => 0;
        public static double StarsMaximum => 16;

        public double StarsStart
        {
            get => starsStart;
            set
            {
                if (value == starsStart)
                    return;
                starsStart = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Stars));
            }
        }

        public double StarsEnd
        {
            get => starsEnd;
            set
            {
                if (value == starsEnd)
                    return;
                starsEnd = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Stars));
            }
        }

        public string Votes
        {
            get
            {
                var min = VotesStart.ToString();
                var max = VotesEnd == 1000 ? "∞" : VotesEnd.ToString();
                return $"{min} - {max}";
            }
        }

        public static int VotesMinimum => 0;
        public static int VotesMaximum => 1000;

        public int VotesStart
        {
            get => votesStart;
            set
            {
                if (value == votesStart)
                    return;
                votesStart = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Votes));
            }
        }

        public int VotesEnd
        {
            get => votesEnd;
            set
            {
                if (value == votesEnd)
                    return;
                votesEnd = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Votes));
            }
        }

        public string DownVotes
        {
            get
            {
                var min = DownVotesStart.ToString();
                var max = DownVotesEnd == 1000 ? "∞" : DownVotesEnd.ToString();
                return $"{min} - {max}";
            }
        }

        public static int DownVotesMinimum => 0;
        public static int DownVotesMaximum => 1000;

        public int DownVotesStart
        {
            get => downVotesStart;
            set
            {
                if (value == downVotesStart)
                    return;
                downVotesStart = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DownVotes));
            }
        }

        public int DownVotesEnd
        {
            get => downVotesEnd;
            set
            {
                if (value == downVotesEnd)
                    return;
                downVotesEnd = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DownVotes));
            }
        }

        public string UpVotes
        {
            get
            {
                var min = UpVotesStart.ToString();
                var max = UpVotesEnd == 1000 ? "∞" : UpVotesEnd.ToString();
                return $"{min} - {max}";
            }
        }

        public static int UpVotesMinimum => 0;
        public static int UpVotesMaximum => 1000;

        public int UpVotesStart
        {
            get => upVotesStart;
            set
            {
                if (value == upVotesStart)
                    return;
                upVotesStart = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(UpVotes));
            }
        }

        public int UpVotesEnd
        {
            get => upVotesEnd;
            set
            {
                if (value == upVotesEnd)
                    return;
                upVotesEnd = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(UpVotes));
            }
        }

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
            songCopyDomain = serviceLocator.GetService<ISongCopyDomain>();
            songCopyDomain.OnPlaylistSelectionChanged += SongCopyDomain_OnPlaylistSelectionChanged;

            Relevances.AddRange(EnumWrapper<SearchParamRelevance>.GetValues(serviceLocator, SearchParamRelevance.Undefined));
        }

        public async Task SearchAsync()
        {
            currentPageIndex = 0;

            SetSearchQueryBuilderData();
            var searchQuery = searchQueryBuilder.GetSearchQuery(currentPageIndex);
            if (string.IsNullOrWhiteSpace(searchQuery?.Query))
            {
                UpdateSearchData(null);
                return;
            }

            MapDetails? searchResult;
            if (searchQuery.IsKey)
            {
                var mapDetail = await beatSaverService.GetMapDetailAsync(searchQuery.Query, BeatSaverKeyType.Id);
                if (mapDetail == null)
                {
                    UpdateSearchData(null);
                    return;
                }

                searchResult = new MapDetails
                {
                    Docs = [mapDetail]
                };
            }
            else
            {
                searchResult = await beatSaverService.SearchAsync(searchQuery.Query);
            }
            UpdateSearchData(searchResult);
        }

        #region Private fields

        private void UpdateSearchData(MapDetails? searchResult)
        {
            Results.ForEach(result => result.CleanUpReferences());
            Results.Clear();

            if (searchResult == null || searchResult.Docs.Count == 0)
            {
                OnPropertyChanged(nameof(ShowResults));
                OnPropertyChanged(nameof(ShowNoResults));
                return;
            }
            Results.AddRange(searchResult.Docs.Select(mapDetail => new SearchResultMapDetailViewModel(ServiceLocator, mapDetail)));
            SongCount = $"Showing {searchResult.Docs.Count} from {Math.Max(searchResult.Info.Total, searchResult.Docs.Count)} results";
            FilterVisible = false;
            ShowMoreCommand?.RaiseCanExecuteChanged();
        }

        private void SetSearchQueryBuilderData()
        {
            searchQueryBuilder.MinNps = NPSStart;
            searchQueryBuilder.MaxNps = NPSEnd;

            if (DateSelectionStart != null && DateSelectionStart > DateMinimum)
                searchQueryBuilder.From = DateSelectionStart;
            if (DateSelectionEnd != null && DateSelectionEnd < DateMaximum)
                searchQueryBuilder.To = DateSelectionEnd;

            if (SelectedRelevance != null)
                searchQueryBuilder.Relevance = SelectedRelevance.Value;

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
                    searchQueryBuilder.MaxBlStars = StarsEnd < 16 ? StarsEnd : null;
                    searchQueryBuilder.MaxSsStars = StarsEnd < 16 ? StarsEnd : null;
                    searchQueryBuilder.MinBlStars = StarsStart > 0 ? StarsStart : null;
                    searchQueryBuilder.MinSsStars = StarsStart > 0 ? StarsStart : null;
                    break;
                case SearchParamLeaderboard.Ranked:
                    searchQueryBuilder.MaxBlStars = StarsEnd < 16 ? StarsEnd : null;
                    searchQueryBuilder.MaxSsStars = StarsEnd < 16 ? StarsEnd : null;
                    searchQueryBuilder.MinBlStars = StarsStart > 0 ? StarsStart : null;
                    searchQueryBuilder.MinSsStars = StarsStart > 0 ? StarsStart : null;
                    break;
                case SearchParamLeaderboard.BeatLeader:
                    searchQueryBuilder.MaxBlStars = StarsEnd < 16 ? StarsEnd : null;
                    searchQueryBuilder.MinBlStars = StarsStart > 0 ? StarsStart : null;
                    break;
                case SearchParamLeaderboard.ScoreSaber:
                    searchQueryBuilder.MaxSsStars = StarsEnd < 16 ? StarsEnd : null;
                    searchQueryBuilder.MinSsStars = StarsStart > 0 ? StarsStart : null;
                    break;
                default:
                    searchQueryBuilder.MaxBlStars = null;
                    searchQueryBuilder.MaxSsStars = null;
                    searchQueryBuilder.MinBlStars = null;
                    searchQueryBuilder.MinSsStars = null;
                    break;
            }

            searchQueryBuilder.MinVotes = VotesStart;
            searchQueryBuilder.MaxVotes = VotesEnd;
            searchQueryBuilder.MinDownVotes = DownVotesStart;
            searchQueryBuilder.MaxDownVotes = DownVotesEnd;
            searchQueryBuilder.MinUpVotes = UpVotesStart;
            searchQueryBuilder.MaxUpVotes = UpVotesEnd;

            searchQueryBuilder.Tags.Clear();
            var selectedMapStyle = MapStyles.SingleOrDefault(ms => ms.IsSelected);
            if (selectedMapStyle?.None == false)
            {
                searchQueryBuilder.Tags.Add(selectedMapStyle.Key);
            }
            var selectedSongStyle = SongStyles.SingleOrDefault(ss => ss.IsSelected);
            if (selectedSongStyle?.None == false)
            {
                searchQueryBuilder.Tags.Add(selectedSongStyle.Key);
            }

            searchQueryBuilder.Environments.Clear();
            var selectedLegacyEnvironment = LegacyEnvironments.SingleOrDefault(le => le.IsSelected);
            if (selectedLegacyEnvironment != null && selectedLegacyEnvironment.Key == DataAccess.BeatSaver.Environment.All)
            {
                searchQueryBuilder.Environments.AddRange(LegacyEnvironments.Where(le => !le.None && le.Key != DataAccess.BeatSaver.Environment.All).Select(le => le.Key));
            }
            else if (selectedLegacyEnvironment != null && !selectedLegacyEnvironment.None)
            {
                searchQueryBuilder.Environments.Add(selectedLegacyEnvironment.Key);
            }
            var selectedNewEnvironment = NewEnvironments.SingleOrDefault(le => le.IsSelected);
            if (selectedNewEnvironment != null && selectedNewEnvironment.Key == DataAccess.BeatSaver.Environment.All)
            {
                searchQueryBuilder.Environments.AddRange(NewEnvironments.Where(le => !le.None && le.Key != DataAccess.BeatSaver.Environment.All).Select(le => le.Key));
            }
            else if (selectedNewEnvironment != null && !selectedNewEnvironment.None)
            {
                searchQueryBuilder.Environments.Add(selectedNewEnvironment.Key);
            }

            searchQueryBuilder.Query = Query == "*" ? " " : Query;
        }

        private void SongCopyDomain_OnPlaylistSelectionChanged(object? sender, Business.Core.SongCopy.PlaylistSelectionChangedEventArgs e)
        {
            if (e.Playlist == null)
            {
                CreatePlaylistCommandText = "Create new playlist in root with all songs (all filter will apply)";
                OnPropertyChanged(nameof(CreatePlaylistCommandText));
            }
            else if (e.Playlist is PlaylistFolderViewModel playlistFolderViewModel)
            {
                CreatePlaylistCommandText = $"Create new playlist in folder '{playlistFolderViewModel.Name}' with all songs (all filter will apply)";
                OnPropertyChanged(nameof(CreatePlaylistCommandText));
            }
            else if (e.Playlist is PlaylistViewModel playlistViewModel)
            {
                OverwritePlaylistCommandText = $"Overwrite playlist '{playlistViewModel.PlaylistTitle}' with all songs (all filter will apply)";
                OnPropertyChanged(nameof(OverwritePlaylistCommandText));
                MergePlaylistCommandText = $"Merge all songs (all filter will apply) with songs from playlist '{playlistViewModel.PlaylistTitle}'";
                OnPropertyChanged(nameof(MergePlaylistCommandText));
            }

            CreatePlaylistCommand?.RaiseCanExecuteChanged();
            OverwritePlaylistCommand?.RaiseCanExecuteChanged();
            MergePlaylistCommand?.RaiseCanExecuteChanged();
        }

        private bool CanSearch()
        {
            return !string.IsNullOrWhiteSpace(Query);
        }

        private async Task ShowMoreAsync()
        {
            currentPageIndex++;

            SetSearchQueryBuilderData();
            var searchQuery = searchQueryBuilder.GetSearchQuery(currentPageIndex);
            if (string.IsNullOrWhiteSpace(searchQuery?.Query))
            {
                UpdateSearchData(null);
                return;
            }

            MapDetails? searchResult;
            if (searchQuery.IsKey)
            {
                var mapDetail = await beatSaverService.GetMapDetailAsync(searchQuery.Query, BeatSaverKeyType.Id);
                if (mapDetail == null)
                {
                    UpdateSearchData(null);
                    return;
                }

                searchResult = new MapDetails
                {
                    Docs = [mapDetail]
                };
            }
            else
            {
                searchResult = await beatSaverService.SearchAsync(searchQuery.Query);
            }
            UpdateSearchData(searchResult);
        }

        private bool CanShowMore()
        {
            return ShowResults;
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

            NPSStart = searchQueryBuilder.MinNps;
            NPSEnd = searchQueryBuilder.MaxNps;
            DateSelectionStart = searchQueryBuilder.From;
            DateSelectionEnd = searchQueryBuilder.To;
            SelectedRelevance = null;

            var defaultAutoMapper = AutoMapper.SingleOrDefault(am => am.None);
            if (defaultAutoMapper != null)
                defaultAutoMapper.IsSelected = true;

            var defaultCurated = Curated.SingleOrDefault(c => c.None);
            if (defaultCurated != null)
                defaultCurated.IsSelected = true;

            var defaultVerified = Verified.SingleOrDefault(c => c.None);
            if (defaultVerified != null)
                defaultVerified.IsSelected = true;

            var defaultFullSpread = FullSpread.SingleOrDefault(c => c.None);
            if (defaultFullSpread != null)
                defaultFullSpread.IsSelected = true;

            var defaultLeaderboard = Leaderboard.SingleOrDefault(c => c.None);
            if (defaultLeaderboard != null)
                defaultLeaderboard.IsSelected = true;
            StarsStart = 0;
            StarsEnd = 16;

            VotesStart = 0;
            VotesEnd = 1000;
            UpVotesStart = 0;
            UpVotesEnd = 1000;
            DownVotesStart = 0;
            DownVotesEnd = 1000;

            var defaultChroma = Chroma.SingleOrDefault(c => c.None);
            if (defaultChroma != null)
                defaultChroma.IsSelected = true;

            var defaultNoodle = NoodleExtensions.SingleOrDefault(c => c.None);
            if (defaultNoodle != null)
                defaultNoodle.IsSelected = true;

            var defaultMappingExtensions = MappingExtensions.SingleOrDefault(c => c.None);
            if (defaultMappingExtensions != null)
                defaultMappingExtensions.IsSelected = true;

            var defaultCinema = Cinema.SingleOrDefault(c => c.None);
            if (defaultCinema != null)
                defaultCinema.IsSelected = true;

            var defaultVivify = Vivify.SingleOrDefault(c => c.None);
            if (defaultVivify != null)
                defaultVivify.IsSelected = true;

            var defaulMapStyle = MapStyles.SingleOrDefault(c => c.None);
            if (defaulMapStyle != null)
                defaulMapStyle.IsSelected = true;

            var defaulSongStyle = SongStyles.SingleOrDefault(c => c.None);
            if (defaulSongStyle != null)
                defaulSongStyle.IsSelected = true;

            var defaulLegacyEnvironment = LegacyEnvironments.SingleOrDefault(c => c.None);
            if (defaulLegacyEnvironment != null)
                defaulLegacyEnvironment.IsSelected = true;

            var defaulNewEnvironment = NewEnvironments.SingleOrDefault(c => c.None);
            if (defaulNewEnvironment != null)
                defaulNewEnvironment.IsSelected = true;
        }

        private bool CanResetFilter()
        {
            return true;
        }

        private void CreatePlaylist()
        {
            // todo: only take filtered songs
            var songs = Results.Select(r => new Song
            {
                Hash = r.Model.Versions.OrderByDescending(v => v.CreatedAt).First().Hash,
                Key = r.Model.Id,
                SongName = r.SongName,
            });

            var createPlaylistEventArgs = new CreatePlaylistEventArgs
            {
                PlaylistName = $"Song search {DateTime.Now:yyyy-MM-dd HH-mm-ss}",
                Songs = [.. songs]
            };
            songCopyDomain.CreatePlaylist(createPlaylistEventArgs);
        }

        private bool CanCreatePlaylist()
        {
            return songCopyDomain.SelectedPlaylist != null || songCopyDomain.SelectedPlaylist is PlaylistFolderViewModel;
        }
        private void OverwritePlaylist()
        {
            // todo: only take filtered songs
            var songs = Results.Select(r => new Song
            {
                Hash = r.Model.Versions.OrderByDescending(v => v.CreatedAt).First().Hash,
                Key = r.Model.Id,
                SongName = r.SongName,
            });

            var songCopyEventArgs = new SongCopyEventArgs
            {
                OverwritePlaylist = true,
                Songs = [.. songs]
            };
            songCopyDomain.CopySongs(songCopyEventArgs);
        }

        private bool CanOverwritePlaylist()
        {
            return songCopyDomain.SelectedPlaylist != null && songCopyDomain.SelectedPlaylist is PlaylistViewModel;
        }
        private void MergePlaylist()
        {
            // todo: only take filtered songs
            var songs = Results.Select(r => new Song
            {
                Hash = r.Model.Versions.OrderByDescending(v => v.CreatedAt).First().Hash,
                Key = r.Model.Id,
                SongName = r.SongName,
            });

            var songCopyEventArgs = new SongCopyEventArgs
            {
                Songs = [.. songs]
            };
            songCopyDomain.CopySongs(songCopyEventArgs);
        }

        private bool CanMergePlaylist()
        {
            return songCopyDomain.SelectedPlaylist != null && songCopyDomain.SelectedPlaylist is PlaylistViewModel;
        }

        #endregion
    }
}
