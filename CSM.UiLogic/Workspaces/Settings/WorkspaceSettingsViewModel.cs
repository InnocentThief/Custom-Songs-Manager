﻿using CSM.Framework;
using CSM.Framework.Configuration.UserConfiguration;
using CSM.Framework.Extensions;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSM.UiLogic.Workspaces.Settings
{
    public class WorkspaceSettingsViewModel : ObservableObject
    {
        private WorkspaceViewModel workspaceViewModel;
        private bool songDetailPositionRight;
        private bool songDetailPositionBottom;

        public List<WorkspaceViewModel> Workspaces { get; }

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
        }
    }
}