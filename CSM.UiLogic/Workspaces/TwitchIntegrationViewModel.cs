using CSM.Framework;
using CSM.UiLogic.Workspaces.Playlists;
using CSM.UiLogic.Workspaces.TwitchIntegration;
using CSM.UiLogic.Workspaces.ScoreSaberIntegration;

namespace CSM.UiLogic.Workspaces
{
    /// <summary>
    /// ViewModel for the Twitch Integration workspace.
    /// </summary>
    internal class TwitchIntegrationViewModel : BaseWorkspaceViewModel
    {
        #region Public Properties

        /// <summary>
        /// Gets the view model for the playlists area.
        /// </summary>
        public PlaylistsViewModel Playlists { get; set; }

        /// <summary>
        /// Gets the view model for the Twitch area.
        /// </summary>
        public TwitchViewModel Twitch { get; }

        /// <summary>
        /// Gets the view model for the ScoreSaber area.
        /// </summary>
        public ScoreSaberViewModel ScoreSaber { get; }

        /// <summary>
        /// Gets the workspace type.
        /// </summary>
        public override WorkspaceType WorkspaceType => WorkspaceType.TwitchIntegration;

        #endregion

        /// <summary>
        /// Initializes a new <see cref="TwitchIntegrationViewModel"/>.
        /// </summary>
        public TwitchIntegrationViewModel()
        {
            Playlists = new PlaylistsViewModel(false);
            Twitch = new TwitchViewModel(Playlists.PlaylistSelectionState);
            Twitch.AddSongToPlaylistEvent += Twitch_AddSongToPlaylistEvent;
            Twitch.SongChangedEvent += Twitch_SongChangedEvent;
            Twitch.OnScoreSaberAddPlayer += Twitch_OnScoreSaberAddPlayer;
            ScoreSaber = new ScoreSaberViewModel();
            ScoreSaber.TabIndex = 0;
        }

        /// <summary>
        /// Used to load the workspace data.
        /// </summary>
        public override void LoadData()
        {
            Playlists.LoadData();
            Twitch.Initialize();
        }

        /// <summary>
        /// Used to unload the workspace data.
        /// </summary>
        public override void UnloadData()
        {

        }

        #region Helper methods

        private void Twitch_AddSongToPlaylistEvent(object sender, AddSongToPlaylistEventArgs e)
        {
            ((PlaylistViewModel)Playlists.SelectedPlaylist).AddPlaylistSong(e);
        }

        private void Twitch_SongChangedEvent(object sender, Playlists.PlaylistSongChangedEventArgs e)
        {
            Playlists.SongChangedEvent(sender, e);
        }

        private async void Twitch_OnScoreSaberAddPlayer(object sender, ScoreSaberAddPlayerEventArgs e)
        {
            await ScoreSaber.AddPlayerFromTwitchAsync(e.Playername);
        }

        #endregion
    }
}