using CSM.DataAccess.Entities.Online.ScoreSaber;
using CSM.Services;
using CSM.UiLogic.Properties;
using CSM.UiLogic.Wizards;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CSM.UiLogic.Workspaces.ScoreSaberIntegration
{
    public class ScoreSaberPlayerScoreViewModel : EditWindowBaseViewModel
    {
        public PlayerScore PlayerScore { get; }

        public int Rank => PlayerScore.Score.Rank;

        public DateTime TimeSet => DateTime.Parse(PlayerScore.Score.TimeSet, CultureInfo.CurrentCulture).Date;

        public string TimeSetText
        {
            get
            {
                var timeSpan = DateTime.Now - DateTime.Parse(PlayerScore.Score.TimeSet, CultureInfo.InvariantCulture);
                if (timeSpan.Days > 365)
                {
                    return $"{timeSpan.Days / 365}y ago";
                }
                else if (timeSpan.Days > 30)
                {
                    return $"{timeSpan.Days / 30}mo ago";
                }
                else
                {
                    return $"{timeSpan.Days}d ago";
                }
            }
        }

        public int Difficulty => PlayerScore.Leaderboard.Difficulty.Diff;

        public string CoverImage => PlayerScore.Leaderboard.CoverImage;

        public string SongColumnText => $"{PlayerScore.Leaderboard.SongName} {PlayerScore.Leaderboard.SongAuthorName} {PlayerScore.Leaderboard.LevelAuthorName}";

        public string SongName => PlayerScore.Leaderboard.SongName;

        public string SongAuthorName => PlayerScore.Leaderboard.SongAuthorName;

        public string LevelAuthorName => PlayerScore.Leaderboard.LevelAuthorName;

        public string Score => PlayerScore.Score.BaseScore.ToString();

        public decimal Accuracy
        {
            get
            {
                if (PlayerScore.Leaderboard.MaxScore > 0)
                {
                    return Math.Round(PlayerScore.Score.BaseScore / PlayerScore.Leaderboard.MaxScore * 100, 2);
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool FullCombo => PlayerScore.Score.FullCombo;

        public int MaxCombo => PlayerScore.Score.MaxCombo;

        public string Modifiers => string.IsNullOrWhiteSpace(PlayerScore.Score.Modifiers) ? "No Modifiers" : PlayerScore.Score.Modifiers;

        public decimal PP => Math.Round(PlayerScore.Score.PP, 2);

        public decimal WeightPP => Math.Round(PlayerScore.Score.Weight * PlayerScore.Score.PP, 2);

        public string PPWeightPP => $"{PP} [{WeightPP}]";

        public int MissedNotes => PlayerScore.Score.MissedNotes;

        public int BadCuts => PlayerScore.Score.BadCuts;

        public string Stars => $"{PlayerScore.Leaderboard.Stars}*";

        public RelayCommand ShowAdditionalInfosCommand { get; }

        public RelayCommand CopyBsrKeyCommand { get; }

        public override string Title => SongName;

        public override int Height => 250;

        public override int Width => 600;

        /// <summary>
        /// Initializes a new <see cref="ScoreSaberPlayerScoreViewModel"/>.
        /// </summary>
        /// <param name="playerScore">The player score fetched from ScoreSaber.</param>
        public ScoreSaberPlayerScoreViewModel(PlayerScore playerScore) : base(String.Empty, "Close")
        {
            PlayerScore = playerScore;
            ShowAdditionalInfosCommand = new RelayCommand(ShowAdditionalInfos);
            CopyBsrKeyCommand = new RelayCommand(CopyBsrKey);
        }

        private void ShowAdditionalInfos()
        {
            EditWindowController.Instance().ShowEditWindow(this);
        }

        private async void CopyBsrKey()
        {
            var beatmapService = new BeatMapService("maps/hash");
            var beatmap = await beatmapService.GetBeatMapDataAsync(PlayerScore.Leaderboard.SongHash);
            if (beatmap != null)
            {
                try
                {
                    Clipboard.SetText($"!bsr {beatmap.Id}");
                }
                catch (Exception)
                {
                    var messageBoxViewModel = new MessageBoxViewModel(Resources.OK, MessageBoxButtonColor.Default, String.Empty, MessageBoxButtonColor.Default)
                    {
                        Title = Resources.SongDetail_CopyBSR_Error_Title,
                        Message = Resources.SongDetail_CopyBSR_Error_Message,
                        MessageBoxType = DataAccess.Entities.Types.MessageBoxTypes.Information
                    };
                    MessageBoxController.Instance().ShowMessageBox(messageBoxViewModel);
                }
            }
        }
    }
}