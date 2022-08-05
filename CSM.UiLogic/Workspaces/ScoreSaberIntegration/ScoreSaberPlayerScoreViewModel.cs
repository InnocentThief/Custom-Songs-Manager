using CSM.DataAccess.Entities.Online.ScoreSaber;
using CSM.UiLogic.Wizards;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.ScoreSaberIntegration
{
    public class ScoreSaberPlayerScoreViewModel : EditWindowBaseViewModel
    {
        private PlayerScore playerScore;

        public int Rank => playerScore.Score.Rank;

        public DateTime TimeSet => DateTime.Parse(playerScore.Score.TimeSet, CultureInfo.InvariantCulture);

        public string TimeSetText
        {
            get
            {
                var timeSpan = DateTime.Now - DateTime.Parse(playerScore.Score.TimeSet, CultureInfo.InvariantCulture);
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

        public int Difficulty => playerScore.Leaderboard.Difficulty.Diff;

        public string CoverImage => playerScore.Leaderboard.CoverImage;

        public string SongColumnText => $"{playerScore.Leaderboard.SongName} {playerScore.Leaderboard.SongAuthorName} {playerScore.Leaderboard.LevelAuthorName}";

        public string SongName => playerScore.Leaderboard.SongName;

        public string SongAuthorName => playerScore.Leaderboard.SongAuthorName;

        public string LevelAuthorName => playerScore.Leaderboard.LevelAuthorName;

        public string Score => playerScore.Score.BaseScore.ToString();

        public decimal Accuracy
        {
            get
            {
                if (playerScore.Leaderboard.MaxScore > 0)
                {
                    return Math.Round(playerScore.Score.BaseScore / playerScore.Leaderboard.MaxScore * 100, 2);
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool FullCombo => playerScore.Score.FullCombo;

        public int MaxCombo => playerScore.Score.MaxCombo;

        public string Modifiers => string.IsNullOrWhiteSpace(playerScore.Score.Modifiers)? "No Modifiers": playerScore.Score.Modifiers;

        public decimal PP => Math.Round(playerScore.Score.PP, 2);

        public decimal WeightPP => Math.Round(playerScore.Score.Weight * playerScore.Score.PP, 2);

        public string PPWeightPP => $"{PP} [{WeightPP}]";

        public int MissedNotes => playerScore.Score.MissedNotes;

        public int BadCuts => playerScore.Score.BadCuts;

        public string Stars => $"{playerScore.Leaderboard.Stars}*";

        public RelayCommand ShowAdditionalInfosCommand { get; }

        public override string Title => SongName;

        public override int Height => 250;

        public override int Width => 600;

        /// <summary>
        /// Initializes a new <see cref="ScoreSaberPlayerScoreViewModel"/>.
        /// </summary>
        /// <param name="playerScore">The player score fetched from ScoreSaber.</param>
        public ScoreSaberPlayerScoreViewModel(PlayerScore playerScore) : base(String.Empty, "Close")
        {
            this.playerScore = playerScore;
            ShowAdditionalInfosCommand = new RelayCommand(ShowAdditionalInfos);
        }

        private void ShowAdditionalInfos()
        {
            EditWindowController.Instance().ShowEditWindow(this);
        }
    }
}