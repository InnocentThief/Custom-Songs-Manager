using CSM.DataAccess.Entities.Online;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;

namespace CSM.UiLogic.Workspaces.CustomLevels
{
    /// <summary>
    /// Represents one difficulty.
    /// </summary>
    public class CustomLevelDifficultyViewModel
    {
        private readonly Difficulty difficulty;

        #region Public Properties

        public decimal NoteJumpMovementSpeed => difficulty.NoteJumpMovementSpeed;

        public decimal NoteJumpStartBeatOffset => difficulty.NoteJumpStartBeatOffset;

        public decimal NotesPerSecond => difficulty.Nps;

        public string Characteristic => difficulty.Characteristic;

        public string Difficulty => difficulty.Diff;

        public bool Chroma => difficulty.Chroma;

        public bool Noodle => difficulty.Noodle;

        public bool MappingExtension => difficulty.MappingExtension;

        /// <summary>
        /// Gets the start rating of a difficulty.
        /// </summary>
        /// <remarks>The difficulty string if not ranked; otherwise the ranking.</remarks>
        public string Stars
        {
            get
            {
                return difficulty.Stars > 0 ? $"{difficulty.Stars}*" : Difficulty;
            }
        }

        public string Label
        {
            get
            {
                return difficulty.Label;
            }
        }

        public string DisplayText
        {
            get
            {
                if (difficulty.Stars > 0 && !string.IsNullOrEmpty(difficulty.Label)) return $"{difficulty.Stars}* {Label}";
                if (difficulty.Stars > 0) return $"{difficulty.Stars}* {Difficulty}";
                if (!string.IsNullOrEmpty(difficulty.Label)) return difficulty.Label;
                return Difficulty;
            }
        }

        /// <summary>
        /// Gets a string that contains the characteristc combined with the difficulty.
        /// </summary>
        public string CharacteristicDifficultyCombination => $"{Characteristic} / {Difficulty}";

        /// <summary>
        /// Gets a string that contains the song speed information (NPS, NJS, NJO).
        /// </summary>
        public string NoteInformation => $"NPS: {NotesPerSecond} / NJS: {NoteJumpMovementSpeed} / NJO: {NoteJumpStartBeatOffset}";

        /// <summary>
        /// Gets a string that contains the extension information (Chroma/Noodle/Mapping Extension).
        /// </summary>
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

        /// <summary>
        /// Initializes a new <see cref="CustomLevelDifficultyViewModel"/>.
        /// </summary>
        /// <param name="difficulty">The <see cref="Difficulty"/>.</param>
        public CustomLevelDifficultyViewModel(Difficulty difficulty)
        {
            this.difficulty = difficulty;
        }
    }
}