using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.ScoreSaberIntegration
{
    public class ScoreSaberSongViewModel : ObservableObject
    {
        public string CoverImage { get; }

        public string SongName { get; }

        public string SongAuthorName { get;  }

        public string LevelAuthorName { get; }

        public decimal Player1Accuracy { get; }

    }
}