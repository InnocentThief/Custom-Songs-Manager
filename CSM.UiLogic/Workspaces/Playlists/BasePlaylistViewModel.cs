using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.Playlists
{
    public abstract class BasePlaylistViewModel : ObservableObject
    {
        public string Name { get; set; }

        public BasePlaylistViewModel(string name)
        {
            Name = name;
        }
    }
}