using CSM.DataAccess.BeatSaver;
using CSM.DataAccess.Playlists;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using System.Globalization;

namespace CSM.UiLogic.ViewModels.Common.Playlists
{
    internal class PlaylistSongViewModel : BaseViewModel
    {
        #region Private fields

        private readonly Song song;
        private readonly List<PlaylistSongDifficultyViewModel> difficulties = [];

        #endregion

        #region Properties

        public Song Model => song;

        public string Hash => song.Hash.ToLower();

        public string BsrKey => song.Key ?? string.Empty;

        public int BsrKeyHex
        {
            get
            {
                if (string.IsNullOrWhiteSpace(song.Key))
                    return 0;
                int.TryParse(BsrKey, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int result);
                return result;
            }
        }

        public string SongName => song.SongName;

        public string? LevelAuthorName => song.LevelAuthorName;

        public List<PlaylistSongDifficultyViewModel> Difficulties => [.. difficulties.OrderBy(d => d.Name)];

        public DateTime? Uploaded { get; private set; }

        #endregion

        public PlaylistSongViewModel(IServiceLocator serviceLocator, Song song) : base(serviceLocator)
        {
            this.song = song;

            difficulties.AddRange(song.Difficulties?.Select(d => new PlaylistSongDifficultyViewModel(serviceLocator, d)) ?? []);
            OnPropertyChanged(nameof(Difficulties));
        }

        public void UpdateData(MapDetail mapDetail)
        {
            song.Key = mapDetail.Id;
            song.SongName = mapDetail.Name;
            song.LevelAuthorName = mapDetail.Metadata?.LevelAuthorName;
            song.LevelId = mapDetail.Id;
            Uploaded = mapDetail.Uploaded;

            OnPropertyChanged(nameof(BsrKey));
            OnPropertyChanged(nameof(BsrKeyHex));
            OnPropertyChanged(nameof(SongName));
            OnPropertyChanged(nameof(LevelAuthorName));
            OnPropertyChanged(nameof(Uploaded));
        }
    }

}
