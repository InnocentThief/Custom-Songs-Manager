﻿using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Globalization;
using System.Text;

namespace CSM.UiLogic.Workspaces.Playlists
{
    /// <summary>
    /// ViewModel used for song search handling.
    /// </summary>
    public class SongSearchViewModel : ObservableObject
    {
        #region Private fields

        private string relevance;
        private string mapStyle;
        private string songStyle;
        private int currentPageIndex;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the query for the search.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Gets or sets whether to include automapper songs.
        /// </summary>
        public bool Ai { get; set; }

        /// <summary>
        /// Gets or sets whether to include ranked songs.
        /// </summary>
        public bool Ranked { get; set; }

        /// <summary>
        /// Gets or sets whether to include curated songs.
        /// </summary>
        public bool Curated { get; set; }

        /// <summary>
        /// Gets or sets whether to include only songs from verified mappers.
        /// </summary>
        public bool VerifiedMapper { get; set; }

        /// <summary>
        /// Gets or sets whether to include full spread songs.
        /// </summary>
        public bool FullSpread { get; set; }

        /// <summary>
        /// Gets or sets whether to include songs with chroma extension.
        /// </summary>
        public bool Chroma { get; set; }

        /// <summary>
        /// Gets or sets whether to include songs with noddle extension.
        /// </summary>
        public bool Noodle { get; set; }

        /// <summary>
        /// Gets or sets whether to include songs with mapping extensions.
        /// </summary>
        public bool MappingExtensions { get; set; }

        /// <summary>
        /// Gets or sets whether to include songs with cinema.
        /// </summary>
        public bool Cinema { get; set; }

        /// <summary>
        /// Gets or sets whether Relevance is set to default.
        /// </summary>
        public bool RelevanceNone { get; set; }

        /// <summary>
        /// Gets or sets whether MapStyle is set to default.
        /// </summary>
        public bool MapStyleNone { get; set; }

        /// <summary>
        /// Gets or sets whether SongStyle is set to default.
        /// </summary>
        public bool SongStyleNone { get; set; }

        /// <summary>
        /// Command used to set the Relevance.
        /// </summary>
        public RelayCommand<string> RelevanceCommand { get; }

        /// <summary>
        /// Command used to set the MapStyle.
        /// </summary>
        public RelayCommand<string> MapStyleCommand { get; }

        /// <summary>
        /// Command used to set the SongStyle.
        /// </summary>
        public RelayCommand<string> SongStyleCommand { get; }

        /// <summary>
        /// Command used to start the song search.
        /// </summary>
        public RelayCommand SearchCommand { get; }

        /// <summary>
        /// Command used to reset the search parameters.
        /// </summary>
        public RelayCommand ResetSearchParametersCommand { get; }

        /// <summary>
        /// Command used to load the next page of the search result.
        /// </summary>
        public RelayCommand ShowMeMoreCommand { get; }

        public bool SearchExpanded { get; set; }

        #endregion

        /// <summary>
        /// Occurs on song search.
        /// </summary>
        public event EventHandler<SongSearchEventArgs> SearchSongEvent;

        /// <summary>
        /// Initializes a new <see cref="SongSearchViewModel"/>.
        /// </summary>
        public SongSearchViewModel()
        {
            RelevanceCommand = new RelayCommand<string>(RelevanceClick);
            MapStyleCommand = new RelayCommand<string>(MapStyleClick);
            SongStyleCommand = new RelayCommand<string>(SongStyleClick);
            SearchCommand = new RelayCommand(StartSearch);
            ResetSearchParametersCommand = new RelayCommand(ResetSearchParameters);
            ShowMeMoreCommand = new RelayCommand(ShowMeMore);

            RelevanceNone = true;
            MapStyleNone = true;
            SongStyleNone = true;
        }

        /// <summary>
        /// Starts the song search
        /// </summary>
        /// <param name="pageIndex">The current page index.</param>
        public void Search(int pageIndex)
        {
            var searchString = new StringBuilder();

            // BSR Search
            if (!string.IsNullOrWhiteSpace(Query))
            {
                int.TryParse(Query, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int result);
                if (result > 0)
                {
                    SearchSongEvent?.Invoke(this, new SongSearchEventArgs(Query, pageIndex, true));
                    return;
                }
            }

            // Enhanced Search
            if (!string.IsNullOrWhiteSpace(Query)) searchString.Append($"q={Query}");
            if (Chroma)
            {
                if (searchString.Length == 0) searchString.Append("chroma=true");
                else searchString.Append("&chroma=true");
            }
            if (Ranked)
            {
                if (searchString.Length == 0) searchString.Append("ranked=true");
                else searchString.Append("&ranked=true");
            }
            if (Curated)
            {
                if (searchString.Length == 0) searchString.Append("curated=true");
                else searchString.Append("&curated=true");
            }
            if (VerifiedMapper)
            {
                if (searchString.Length == 0) searchString.Append("verified=true");
                else searchString.Append("&verified=true");
            }
            if (Noodle)
            {
                if (searchString.Length == 0) searchString.Append("noodle=true");
                else searchString.Append("&noodle=true");
            }
            if (MappingExtensions)
            {
                if (searchString.Length == 0) searchString.Append("me=true");
                else searchString.Append("&me=true");
            }
            if (Cinema)
            {
                if (searchString.Length == 0) searchString.Append("cinema=true");
                else searchString.Append("&cinema=true");
            }
            if (Ai)
            {
                if (searchString.Length == 0) searchString.Append("auto=true");
                else searchString.Append("&auto=true");
            }
            if (FullSpread)
            {
                if (searchString.Length == 0) searchString.Append("fullSpread=true");
                else searchString.Append("&fullSpread=true");
            }
            if (false)
            {
                if (searchString.Length == 0) searchString.Append($"maxNps={0}");
                else searchString.Append($"&maxNps={0}");
            }
            if (false)
            {
                if (searchString.Length == 0) searchString.Append($"minNps={0}");
                else searchString.Append($"&minNps={0}");
            }
            if (!string.IsNullOrWhiteSpace(relevance))
            {
                if (searchString.Length == 0) searchString.Append($"order={relevance}");
                else searchString.Append($"&order={relevance}");
            }
            if (false)
            {
                if (searchString.Length == 0) searchString.Append($"from={0}");
                else searchString.Append($"&from={0}");
            }
            if (false)
            {
                if (searchString.Length == 0) searchString.Append($"to={0}");
                else searchString.Append($"&to={0}");
            }
            if (!string.IsNullOrWhiteSpace(mapStyle) && !string.IsNullOrWhiteSpace(songStyle))
            {
                if (searchString.Length == 0) searchString.Append($"tags={mapStyle},{songStyle}");
                else searchString.Append($"&tags={mapStyle},{songStyle}");
            }
            else if (!string.IsNullOrWhiteSpace(mapStyle))
            {
                if (searchString.Length == 0) searchString.Append($"tags={mapStyle}");
                else searchString.Append($"&tags={mapStyle}");
            }
            else if (!string.IsNullOrWhiteSpace(songStyle))
            {
                if (searchString.Length == 0) searchString.Append($"tags={songStyle}");
                else searchString.Append($"&tags={songStyle}");
            }

            SearchSongEvent?.Invoke(this, new SongSearchEventArgs(searchString.ToString(), currentPageIndex, false));
        }

        /// <summary>
        /// Sets the visibility of the enhanced search parameters.
        /// </summary>
        /// <param name="visible">Indicator of the visibility.</param>
        public void SetSearchParametersVisibility(bool visible)
        {
            SearchExpanded = visible;
            OnPropertyChanged(nameof(SearchExpanded));
        }

        #region Helper methods

        private void RelevanceClick(string name)
        {
            relevance = name;
        }

        private void MapStyleClick(string name)
        {
            mapStyle = name;
        }

        private void SongStyleClick(string name)
        {
            songStyle = name;
        }

        private void ResetSearchParameters()
        {
            Query = String.Empty;
            OnPropertyChanged(nameof(Query));
            Ai = false;
            OnPropertyChanged(nameof(Ai));
            Ranked = false;
            OnPropertyChanged(nameof(Ranked));
            Curated = false;
            OnPropertyChanged(nameof(Curated));
            VerifiedMapper = false;
            OnPropertyChanged(nameof(VerifiedMapper));
            FullSpread = false;
            OnPropertyChanged(nameof(FullSpread));
            Chroma = false;
            OnPropertyChanged(nameof(Chroma));
            Noodle = false;
            OnPropertyChanged(nameof(Noodle));
            MappingExtensions = false;
            OnPropertyChanged(nameof(MappingExtensions));
            Cinema = false;
            OnPropertyChanged(nameof(Cinema));
            RelevanceNone = true;
            OnPropertyChanged(nameof(RelevanceNone));
            MapStyleNone = true;
            OnPropertyChanged(nameof(MapStyleNone));
            SongStyleNone = true;
            OnPropertyChanged(nameof(SongStyleNone));
        }

        private void StartSearch()
        {
            currentPageIndex = 0;
            Search(currentPageIndex);
        }

        private void ShowMeMore()
        {
            currentPageIndex++;
            Search(currentPageIndex);
        }

        #endregion
    }
}