using CSM.UiLogic.Workspaces.Settings;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace CSM.UiLogic.Workspaces
{
    public class SettingsViewModel : ObservableObject
    {
        private bool visible;

        public BeatSaberSettingsViewModel BeatSaberSettings { get; }

        public WorkspaceSettingsViewModel WorkspaceSettings { get; }

        public bool Visible
        {
            get => visible;
            set
            {
                if (value == visible) return;
                visible = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand CloseCommand { get; }

        public SettingsViewModel()
        {
            CloseCommand = new RelayCommand(Close);
            BeatSaberSettings = new BeatSaberSettingsViewModel();
            WorkspaceSettings = new WorkspaceSettingsViewModel();
        }

        private void Close()
        {
            Visible = false;
        }
    }
}