using CSM.Business.Core.SongCopy;
using CSM.Business.Interfaces;
using CSM.DataAccess.BeatSaver;
using CSM.DataAccess.Playlists;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.ViewModels.Common.MapDetails;
using CSM.UiLogic.ViewModels.Common.Playlists;
using System.Globalization;

namespace CSM.UiLogic.ViewModels.Controls.SongSources.Twitch
{
    internal class TwitchSongViewModel : BaseViewModel
    {
        #region Private fields

        private MapDetail mapDetail;
        private IRelayCommand? addToPlaylistCommand, removeCommand;

        private readonly ISongCopyDomain songCopyDomain;

        #endregion

        #region Properties

        public IRelayCommand? AddToPlaylistCommand => addToPlaylistCommand ??= CommandFactory.Create(AddToPlaylist, CanAddToPlaylist);
        public IRelayCommand? RemoveCommand => removeCommand ??= CommandFactory.Create(Remove, CanRemove);

        public string ChannelName { get; set; } = string.Empty;

        public DateTime ReceivedAt { get; }

        public string BsrKey => mapDetail.Id;

        public int BsrKeyHex
        {
            get
            {
                int.TryParse(BsrKey, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int result);
                return result;
            }
        }

        public string SongName => mapDetail.Metadata?.SongName ?? string.Empty;

        public string LevelAuthorName => mapDetail.Metadata?.LevelAuthorName ?? string.Empty;

        public string SongAuthorName => mapDetail.Metadata?.SongAuthorName ?? string.Empty;

        public MapDetailViewModel? MapDetailViewModel { get; set; }

        #endregion

        public event EventHandler OnRemoveSong;

        public TwitchSongViewModel(IServiceLocator serviceLocator, string channelName, MapDetail mapDetail) : base(serviceLocator)
        {
            ChannelName = channelName;
            this.mapDetail = mapDetail;
            ReceivedAt = DateTime.Now;
            MapDetailViewModel = new MapDetailViewModel(serviceLocator, mapDetail);

            songCopyDomain = serviceLocator.GetService<ISongCopyDomain>();
            songCopyDomain.OnPlaylistSelectionChanged += SongCopyDomain_OnPlaylistSelectionChanged;
        }

        public void CleanUpReferences()
        {
            songCopyDomain.OnPlaylistSelectionChanged -= SongCopyDomain_OnPlaylistSelectionChanged;
        }

        #region Helper methods

        private void AddToPlaylist()
        {
            var version = mapDetail.Versions.OrderByDescending(v => v.CreatedAt).FirstOrDefault();
            if (version == null)
                return;

            var songToCopy = new Song
            {
                Hash = version.Hash,
                Key = version.Key,
                LevelAuthorName = mapDetail.Metadata?.LevelAuthorName ?? string.Empty,
                SongName = mapDetail.Metadata?.SongName ?? string.Empty
            };
            var songCopyEventArgs = new SongCopyEventArgs();
            songCopyEventArgs.Songs.Add(songToCopy);
            songCopyDomain.CopySongs(songCopyEventArgs);
        }

        private bool CanAddToPlaylist()
        {
            return songCopyDomain.SelectedPlaylist is PlaylistViewModel;
        }

        private void Remove()
        {
            OnRemoveSong?.Invoke(this, EventArgs.Empty);
        }

        private bool CanRemove()
        {
            return true;
        }

        private void SongCopyDomain_OnPlaylistSelectionChanged(object? sender, PlaylistSelectionChangedEventArgs e)
        {
            addToPlaylistCommand?.RaiseCanExecuteChanged();
        }

        #endregion
    }
}
