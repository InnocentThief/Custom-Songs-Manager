using CSM.Business.Interfaces;
using CSM.DataAccess.BeatSaver;
using CSM.DataAccess.Playlists;
using CSM.Framework.Extensions;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.Commands;
using CSM.UiLogic.Converter;
using CSM.UiLogic.Helper;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows.Media;

namespace CSM.UiLogic.ViewModels.Common.Playlists
{
    internal sealed class PlaylistViewModel : BasePlaylistViewModel
    {
        #region Private fields

        private readonly Playlist playlist;
        private IRelayCommand? fetchDataCommand;

        private readonly ILogger logger;
        private readonly IBeatSaverService beatSaverService;

        #endregion

        #region Properties

        public IRelayCommand? FetchDataCommand => fetchDataCommand ??= CommandFactory.CreateFromAsync(FetchDataAsync, CanFetchData);

        public string PlaylistTitle
        {
            get => playlist.PlaylistTitle;
            set
            {
                if (value == playlist.PlaylistTitle) return;
                playlist.PlaylistTitle = value;
                OnPropertyChanged();
            }
        }

        public string PlaylistAuthor
        {
            get => playlist.PlaylistAuthor;
            set
            {
                if (value == playlist.PlaylistAuthor) return;
                playlist.PlaylistAuthor = value;
                OnPropertyChanged();
            }
        }

        public string? PlaylistDescription
        {
            get => playlist.PlaylistDescription;
            set
            {
                if (value == playlist.PlaylistDescription) return;
                playlist.PlaylistDescription = value;
                OnPropertyChanged();
            }
        }

        public ImageSource? CoverImage
        {
            get
            {
                if (string.IsNullOrWhiteSpace(playlist.Image)) return null;
                return ImageConverter.BitmapFromBase64(playlist.Image.Split(',').Last());
            }
        }

        public ObservableCollection<PlaylistSongViewModel> Songs { get; } = [];

        #endregion

        public PlaylistViewModel(
           IServiceLocator serviceLocator,
           Playlist playlist,
           string path) : base(serviceLocator, playlist.PlaylistTitle, path)
        {
            this.playlist = playlist;
            logger = serviceLocator.GetService<ILogger<PlaylistViewModel>>();
            beatSaverService = serviceLocator.GetService<IBeatSaverService>();

            Songs.AddRange(playlist.Songs.Select(s => new PlaylistSongViewModel(serviceLocator, s)));
        }

        public async Task FetchDataAsync()
        {
            SetLoadingInProgress(true, "Fetching and updating song data...");

            try
            {
                int totalPages = Songs.Count / 50 + 1;
                for (int i = 0; i < totalPages; i++)
                {
                    var songHashes = Songs.Skip(i * 50).Take(50).Select(s => s.Hash).ToList();
                    var mapDetails = await beatSaverService.GetMapDetailsAsync(songHashes, BeatSaverKeyType.Hash);
                    if (mapDetails == null)
                        continue;
                    foreach (var mapDetail in mapDetails)
                    {
                        if (mapDetail.Value == null)
                            continue;
                        var existingSongs = Songs.Where(s => s.Hash == mapDetail.Key);
                        foreach (var existingSong in existingSongs)
                        {
                            existingSong.UpdateData(mapDetail.Value);
                        }
                    }
                }
                await SaveAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching data for playlist {PlaylistTitle}", PlaylistTitle);
            }
            finally
            {
                SetLoadingInProgress(false, string.Empty);
            }
        }

        #region Helper methods

        private async Task SaveAsync()
        {
            var content = JsonSerializer.Serialize(playlist, JsonSerializerHelper.CreateDefaultSerializerOptions());
            await File.WriteAllTextAsync(Path, content);
        }

        private bool CanFetchData()
        {
            return true;
        }

        #endregion

    }
}
