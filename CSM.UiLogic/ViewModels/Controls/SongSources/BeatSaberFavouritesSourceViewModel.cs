using CSM.Business.Interfaces;
using CSM.DataAccess;
using CSM.DataAccess.BeatSaber;
using CSM.DataAccess.UserConfiguration;
using CSM.Framework.Extensions;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.ViewModels.Controls.SongSources.BeatSaberFavourites;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace CSM.UiLogic.ViewModels.Controls.SongSources
{
    internal class BeatSaberFavouritesSourceViewModel(IServiceLocator serviceLocator) : BaseViewModel(serviceLocator), ISongSourceViewModel
    {
        #region Private fields

        private BeatSaberFavouriteViewModel? selectedFavourite;
        private IRelayCommand? refreshCommand;
        private ViewDefinition? selectedViewDefinition;

        private readonly ILogger<BeatSaberFavouritesSourceViewModel> logger = serviceLocator.GetService<ILogger<BeatSaberFavouritesSourceViewModel>>()!;
        private readonly IBeatSaverService? beatSaverService = serviceLocator.GetService<IBeatSaverService>();
        private readonly IUserConfigDomain userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();

        #endregion

        #region Properties

        public IRelayCommand RefreshCommand => refreshCommand ??= CommandFactory.CreateFromAsync(RefreshAsync, CanRefresh);

        public ObservableCollection<BeatSaberFavouriteViewModel> Favourites { get; } = [];

        public List<BeatSaberFavouriteViewModel> FavouritesFiltered { get; } = [];

        public BeatSaberFavouriteViewModel? SelectedFavourite
        {
            get => selectedFavourite;
            set
            {
                if (value == selectedFavourite)
                    return;
                selectedFavourite = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelectedSong));
            }
        }

        public bool HasSelectedSong => selectedFavourite != null;

        public string SongCount
        {
            get
            {
                if (FavouritesFiltered.Count == 0)
                {
                    if (Favourites.Count == 0)
                        return "No favourites";
                    if (Favourites.Count == 1)
                        return $"1 favourite";
                    return $"{Favourites.Count} favourites";
                }
                else
                {
                    if (Favourites.Count == 0)
                        return "No favourites";
                    if (Favourites.Count == 1)
                        return $"Showing {FavouritesFiltered.Count} of 1 favourite";
                    return $"Showing {FavouritesFiltered.Count} of {Favourites.Count} favourites";
                }
            }
        }

        public ObservableCollection<ViewDefinition> ViewDefinitions { get; } = [];

        public ViewDefinition? SelectedViewDefinition
        {
            get => selectedViewDefinition;
            set
            {
                if (value == selectedViewDefinition)
                    return;
                selectedViewDefinition = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanSaveViewDefinition));
                OnPropertyChanged(nameof(CanDeleteViewDefinition));

                userConfigDomain!.Config!.PlaylistsConfig.LastBeatSaberFavouriteViewDefinitionName = selectedViewDefinition?.Name;
                userConfigDomain.SaveUserConfig();
            }
        }

        public bool ShowViewDefinitions => ViewDefinitions.Count > 0;

        public bool CanSaveViewDefinition => SelectedViewDefinition != null;

        public bool CanDeleteViewDefinition => SelectedViewDefinition != null;

        public FilterMode FilterMode => userConfigDomain.Config?.FilterMode ?? FilterMode.PopUp;

        #endregion

        public async Task LoadAsync(bool refresh = false)
        {
            if (Favourites.Count > 0 && !refresh)
                return;

            SetLoadingInProgress(true, "Loading favourites");
            RefreshCommand.RaiseCanExecuteChanged();

            Favourites.ForEach(f => f.CleanUpReferences());
            Favourites.Clear();
            FavouritesFiltered.Clear();

            var localLow = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).Replace("Local", "LocalLow");
            try
            {
                var playerDataFile = Path.Combine(localLow, "Hyperbolic Magnetism", "Beat Saber", "PlayerData.dat");
                if (!File.Exists(playerDataFile))
                    return;

                var content = await File.ReadAllTextAsync(playerDataFile);
                var playerData = JsonSerializer.Deserialize<PlayerData>(content, JsonSerializerHelper.CreateDefaultSerializerOptions());
                if (playerData?.LocalPlayers == null || playerData.LocalPlayers.Count == 0)
                    return;

                foreach (var favouriteLevelId in playerData.LocalPlayers.First().FavoritesLevelIds)
                {
                    if (!favouriteLevelId.StartsWith("custom_level")) continue;
                    var hash = favouriteLevelId[13..];
                    var mapDetail = await beatSaverService!.GetMapDetailAsync(hash, DataAccess.BeatSaver.BeatSaverKeyType.Hash);
                    if (mapDetail != null)
                    {
                        var mapDetailViewModel = new BeatSaberFavouriteViewModel(ServiceLocator, mapDetail);
                        Favourites.Add(mapDetailViewModel);
                        FavouritesFiltered.Add(mapDetailViewModel);
                    }
                }
                OnPropertyChanged(nameof(SongCount));

                // Load view definitions
                ViewDefinitions.Clear();
                ViewDefinitions.AddRange(await LoadViewDefinitionsAsync(SavableUiElement.BeatSaberFavourites));
                SelectedViewDefinition = ViewDefinitions.FirstOrDefault(vd => vd.Name == userConfigDomain!.Config?.PlaylistsConfig.LastBeatSaberFavouriteViewDefinitionName);
                OnPropertyChanged(nameof(ShowViewDefinitions));

                SetLoadingInProgress(false, string.Empty);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error loading Beat Saber favourites");
            }
            finally
            {
                SetLoadingInProgress(false, string.Empty);
                RefreshCommand.RaiseCanExecuteChanged();
            }
        }

        public override async Task<ViewDefinition?> SaveViewDefinitionAsync(Stream stream, SavableUiElement savableUiElement, string? name = null)
        {
            var newViewDefinition = await base.SaveViewDefinitionAsync(stream, savableUiElement, name);
            if (newViewDefinition != null)
            {
                ViewDefinitions.Add(newViewDefinition);
                SelectedViewDefinition = newViewDefinition;
                OnPropertyChanged(nameof(ShowViewDefinitions));
            }
            return newViewDefinition;
        }

        public override void DeleteViewDefinition(SavableUiElement savableUiElement, string name)
        {
            if (SelectedViewDefinition == null)
                return;
            base.DeleteViewDefinition(savableUiElement, name);
            ViewDefinitions.Remove(SelectedViewDefinition);
            SelectedViewDefinition = ViewDefinitions.FirstOrDefault();
            OnPropertyChanged(nameof(ShowViewDefinitions));
        }

        public void FilterChanged()
        {
            OnPropertyChanged(nameof(SongCount));
        }

        #region Helper methods

        private async Task RefreshAsync()
        {
            await LoadAsync(true);
        }

        private bool CanRefresh()
        {
            return !LoadingInProgress;
        }

        #endregion
    }
}