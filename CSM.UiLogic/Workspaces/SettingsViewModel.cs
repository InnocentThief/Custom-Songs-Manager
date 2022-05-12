using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces
{
    public class SettingsViewModel : ObservableObject
    {
        private bool visible;

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
        }

        private void Close()
        {
            Visible = false;
        }
    }
}