using CSM.Framework;
using CSM.UiLogic.Workspaces.Playlists;
using CSM.UiLogic.Workspaces.TwitchIntegration;
using CSM.UiLogic.Workspaces.TwitchIntegration.ScoreSaberIntegration;

namespace CSM.UiLogic.Workspaces
{
    /// <summary>
    /// ViewModel for the Twitch Integration workspace.
    /// </summary>
    internal class TwitchIntegrationViewModel : BaseWorkspaceViewModel
    {
        public PlaylistsViewModel Playlists { get; set; }

        public ScoreSaberViewModel ScoreSaber { get; }

        public TwitchViewModel Twitch { get; }

        /// <summary>
        /// Gets the workspace type.
        /// </summary>
        public override WorkspaceType WorkspaceType => WorkspaceType.TwitchIntegration;

        public TwitchIntegrationViewModel()
        {
            Playlists = new PlaylistsViewModel();
            Twitch = new TwitchViewModel(Playlists.PlaylistSelectionState);
            Twitch.AddSongToPlaylistEvent += Twitch_AddSongToPlaylistEvent;
            Twitch.SongChangedEvent += Twitch_SongChangedEvent;
            //ScoreSaber = new ScoreSaberViewModel();

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

        #endregion
    }
}