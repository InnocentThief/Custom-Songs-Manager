using CSM.DataAccess.Entities.Online;
using CSM.Framework.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CSM.UiLogic.Workspaces.CustomLevels
{
    public class CustomLevelDifficultyViewModel
    {
        private Difficulty difficulty;

        #region Public Properties

        [Display(Name = "Note Jump Speed", Order = 0)]
        public decimal NoteJumpMovementSpeed => difficulty.NoteJumpMovementSpeed;

        [Display(Name = "Note Jump Offset", Order = 1)]
        public decimal NoteJumpStartBeatOffset => difficulty.NoteJumpStartBeatOffset;

        [Display(Name = "Notes Pers Second", Order = 2)]
        public decimal NotesPerSecond => difficulty.Nps;

        [Display(Name = "Characteristic", Order = 2)]
        public string Characteristic => difficulty.Characteristic;

        [Display(Name = "Difficulty", Order = 3)]
        public string Difficulty => difficulty.Diff;

        [Display(Name = "Chroma", Order = 4)]
        public bool Chroma => difficulty.Chroma;

        [Display(Name = "Noodle", Order = 5)]
        public bool Noodle => difficulty.Noodle;

        [Display(Name = "MappingExtensions", Order = 5)]
        public bool MappingExtension => difficulty.MappingExtension;

        public string Stars
        {
            get
            {
                return difficulty.Stars > 0 ? $"{difficulty.Stars}*" : Difficulty;
            }
        }

        public string CharacteristicDifficultyCombination => $"{Characteristic} / {Difficulty}";

        public string NoteInformation => $"NPS: {NotesPerSecond} / NJS: {NoteJumpMovementSpeed} / NJO: {NoteJumpStartBeatOffset}";

        public string ExtensionInformation
        {
            get
            {
                var extensions = new List<string>();
                if (Chroma) extensions.Add("Chroma");
                if (Noodle) extensions.Add("Noodle");
                if (MappingExtension) extensions.Add("Mapping Ext.");
                return string.Join(" / ", extensions);
            }
        }

        public bool ShowExtensionInformation => Chroma || Noodle || MappingExtension;

        #endregion

        public CustomLevelDifficultyViewModel(Difficulty difficulty)
        {
            this.difficulty = difficulty;
        }
    }
}