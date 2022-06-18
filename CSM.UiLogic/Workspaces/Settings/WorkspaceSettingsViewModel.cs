using CSM.Framework;
using CSM.Framework.Configuration.UserConfiguration;
using CSM.Framework.Extensions;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSM.UiLogic.Workspaces.Settings
{
    /// <summary>
    /// Contains the settings related to workspaces.
    /// </summary>
    public class WorkspaceSettingsViewModel : ObservableObject
    {
        #region Private fields

        private WorkspaceViewModel workspaceViewModel;
        private bool songDetailPositionRight;
        private bool songDetailPositionBottom;
        private bool removeReceivedSongAfterAddingToPlaylist;

        #endregion

        #region Public Properties

        /// <summary>
        /// Contains the available workspaces. Used to select the default workspace.
        /// </summary>
        public List<WorkspaceViewModel> Workspaces { get; }

        /// <summary>
        /// Gets or sets the selected workspace.
        /// </summary>
        public WorkspaceViewModel SelectedWorkspace
        {
            get => workspaceViewModel;
            set
            {
                if (workspaceViewModel == value) return;
                workspaceViewModel = value;
                UserConfigManager.Instance.Config.DefaultWorkspace = workspaceViewModel.Type;
                UserConfigManager.Instance.SaveUserConfig();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets whether the song detail position on the custom level workspace is set to right.
        /// </summary>
        public bool SongDetailPositionRight
        {
            get => songDetailPositionRight;
            set
            {
                if (songDetailPositionRight == value) return;
                songDetailPositionRight = value;
                if (value)
                {
                    UserConfigManager.Instance.Config.CustomLevelsSongDetailPosition = SongDetailPosition.Right;
                    UserConfigManager.Instance.SaveUserConfig();
                    UserConfigManager.Instance.Changed(new UserConfigChangedEventArgs { CustomLevelDetailPositionChanged = true });
                    SongDetailPositionBottom = false;
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets whether the received song should be deleted after adding it to a playlist.
        /// </summary>
        public bool RemoveReceivedSongAfterAddingToPlaylist
        {
            get => removeReceivedSongAfterAddingToPlaylist;
            set
            {
                if (removeReceivedSongAfterAddingToPlaylist == value) return;
                removeReceivedSongAfterAddingToPlaylist = value;
                UserConfigManager.Instance.Config.RemoveReceivedSongAfterAddingToPlaylist = value;
                UserConfigManager.Instance.SaveUserConfig();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets whether the song detail position on the custom level workspace is set to bottom.
        /// </summary>
        public bool SongDetailPositionBottom
        {
            get => songDetailPositionBottom;
            set
            {
                if (songDetailPositionBottom == value) return;
                songDetailPositionBottom = value;
                if (value)
                {
                    UserConfigManager.Instance.Config.CustomLevelsSongDetailPosition = SongDetailPosition.Bottom;
                    UserConfigManager.Instance.SaveUserConfig();
                    UserConfigManager.Instance.Changed(new UserConfigChangedEventArgs { CustomLevelDetailPositionChanged = true });
                    SongDetailPositionRight = false;
                }
                OnPropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new <see cref="WorkspaceSettingsViewModel"/>.
        /// </summary>
        public WorkspaceSettingsViewModel()
        {
            Workspaces = new List<WorkspaceViewModel>();
            foreach (var workspaceType in Enum.GetValues(typeof(WorkspaceType)))
            {
                Workspaces.Add(new WorkspaceViewModel(workspaceType.ToString().ToWorkspaceType(), (WorkspaceType)workspaceType));
            }
            SelectedWorkspace = Workspaces.SingleOrDefault(w => w.Type == UserConfigManager.Instance.Config.DefaultWorkspace);
            songDetailPositionRight = UserConfigManager.Instance.Config.CustomLevelsSongDetailPosition == SongDetailPosition.Right;
            songDetailPositionBottom = UserConfigManager.Instance.Config.CustomLevelsSongDetailPosition == SongDetailPosition.Bottom;
            removeReceivedSongAfterAddingToPlaylist = UserConfigManager.Instance.Config.RemoveReceivedSongAfterAddingToPlaylist;
        }
    }
}