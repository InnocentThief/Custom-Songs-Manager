using CSM.DataAccess.Entities.Online;
using System.Collections.Generic;

namespace CSM.UiLogic.Workspaces.CustomLevels
{
    public class CustomLevelDifficultyViewModel
    {
        private Difficulty difficulty;

        #region Public Properties

        public decimal NoteJumpMovementSpeed => difficulty.NoteJumpMovementSpeed;

        public decimal NoteJumpStartBeatOffset => difficulty.NoteJumpStartBeatOffset;

        public decimal NotesPerSecond => difficulty.Nps;

        public string Characteristic => difficulty.Characteristic;

        public string Difficulty => difficulty.Diff;

        public bool Chroma => difficulty.Chroma;

        public bool Noodle => difficulty.Noodle;

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