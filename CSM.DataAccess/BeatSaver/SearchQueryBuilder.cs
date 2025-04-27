using System.Globalization;
using System.Text;

namespace CSM.DataAccess.BeatSaver
{
    internal class SearchQueryBuilder
    {
        #region Properties

        public bool? AI { get; set; } = null; // true = both, false = only AI, null = no AI

        public bool? Chroma { get; set; }

        public bool? Cinema { get; set; }

        public bool? Curated { get; set; }

        public List<Environment> Environments { get; set; } = [];

        public bool? Followed { get; set; }

        public DateTime? From { get; set; } = null;

        public bool? FullSpread { get; set; }

        public SearchParamLeaderboard Leaderboard { get; set; } = SearchParamLeaderboard.Undefined;

        public decimal? MaxBlStars { get; set; }

        public decimal? MaxBpm { get; set; }

        public int? MaxDownVotes { get; set; }

        public int? MaxDuration { get; set; }

        public decimal? MaxNps { get; set; } = 16;

        public decimal? MaxRating { get; set; }

        public decimal? MaxSsStars { get; set; }

        public int? MaxUpVotes { get; set; }

        public int? MaxVotes { get; set; }

        public bool? Me { get; set; }

        public decimal? MinBlStars { get; set; }

        public decimal? MinBpm { get; set; }

        public int? MinDownVotes { get; set; }

        public int? MinDuration { get; set; }

        public decimal? MinNps { get; set; } = 0;

        public decimal? MinRating { get; set; }

        public decimal? MinSsStars { get; set; }

        public int? MinUpVotes { get; set; }

        public int? MinVotes { get; set; }

        public bool? Noodle { get; set; }

        public SearchParamRelevance Relevance { get; set; } = SearchParamRelevance.Undefined;

        public string Query { get; set; } = string.Empty;

        public List<Tag> Tags { get; set; } = [];

        public DateTime? To { get; set; } = null;

        public bool? Verified { get; set; }

        public bool? Vivify { get; set; }

        #endregion

        public SearchQuery? GetSearchQuery(int pageIndex, int pageSize = 50)
        {
            var parameters = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Query))
                return null;

            var initialQuery = $"{pageIndex}?pageSize={pageSize}";

            // if query is a bsr key, use it as is
            int.TryParse(Query, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int result);
            if (result > 0)
            {
                parameters.Append($"q={Query}");
                return new SearchQuery(parameters.ToString(), pageIndex, true);
            }

            parameters.Append($"&q={Query}");

            switch (AI)
            {
                case true:
                    parameters.Append($"&automapper=true");
                    break;
                case false:
                    parameters.Append($"&automapper=false");
                    break;
                default:
                    break;
            }

            if (Chroma.HasValue)
            {
                parameters.Append($"&chroma={Chroma.Value.ToString().ToLower()}");
            }

            if (Cinema.HasValue)
            {
                parameters.Append($"&cinema={Cinema.Value.ToString().ToLower()}");
            }

            if (Curated.HasValue)
            {
                parameters.Append($"&curated={Curated.Value.ToString().ToLower()}");
            }

            if (Environments.Count > 0)
            {
                parameters.Append($"&environment={string.Join(",", Environments.Select(e => e.ToString().ToLower()))}");
            }

            if (Followed.HasValue)
            {
                parameters.Append($"&followed={Followed.Value.ToString().ToLower()}");
            }

            if (From.HasValue)
            {
                parameters.Append($"&from={From.Value.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture)}");
            }

            if (FullSpread.HasValue)
            {
                parameters.Append($"&fullspread={FullSpread.Value.ToString().ToLower()}");
            }

            if (Leaderboard != SearchParamLeaderboard.Undefined)
            {
                parameters.Append($"&leaderboard={Leaderboard.ToString().ToLower()}");
            }

            if (MaxBlStars.HasValue)
            {
                parameters.Append($"&maxBlStars={MaxBlStars.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            if (MaxBpm.HasValue)
            {
                parameters.Append($"&maxBpm={MaxBpm.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            if (MaxDownVotes.HasValue)
            {
                parameters.Append($"&maxDownVotes={MaxDownVotes.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            if (MaxDuration.HasValue)
            {
                parameters.Append($"&maxDuration={MaxDuration.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            if (MaxNps.HasValue && MaxNps != 16)
            {
                parameters.Append($"&maxNps={MaxNps.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            if (MaxRating.HasValue)
            {
                parameters.Append($"&maxRating={MaxRating.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            if (MaxSsStars.HasValue)
            {
                parameters.Append($"&maxSsStars={MaxSsStars.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            if (MaxUpVotes.HasValue)
            {
                parameters.Append($"&maxUpVotes={MaxUpVotes.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            if (MaxVotes.HasValue)
            {
                parameters.Append($"&maxVotes={MaxVotes.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            if (Me.HasValue)
            {
                parameters.Append($"&me={Me.Value.ToString().ToLower()}");
            }

            if (MinBlStars.HasValue)
            {
                parameters.Append($"&minBlStars={MinBlStars.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            if (MinBpm.HasValue)
            {
                parameters.Append($"&minBpm={MinBpm.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            if (MinDownVotes.HasValue)
            {
                parameters.Append($"&minDownVotes={MinDownVotes.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            if (MinDuration.HasValue)
            {
                parameters.Append($"&minDuration={MinDuration.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            if (MinNps.HasValue && MinNps > 0)
            {
                parameters.Append($"&minNps={MinNps.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            if (MinRating.HasValue)
            {
                parameters.Append($"&minRating={MinRating.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            if (MinSsStars.HasValue)
            {
                parameters.Append($"&minSsStars={MinSsStars.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            if (MinUpVotes.HasValue)
            {
                parameters.Append($"&minUpVotes={MinUpVotes.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            if (MinVotes.HasValue)
            {
                parameters.Append($"&minVotes={MinVotes.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            if (Noodle.HasValue)
            {
                parameters.Append($"&noodle={Noodle.Value.ToString().ToLower()}");
            }

            if (Relevance != SearchParamRelevance.Undefined)
            {
                parameters.Append($"&relevance={Relevance.ToString().ToLower()}");
            }

            if (Tags.Count > 0)
            {
                parameters.Append($"&tags={string.Join(",", Tags.Select(t => t.ToString().ToLower()))}");
            }

            if (To.HasValue)
            {
                parameters.Append($"&to={To.Value.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture)}");
            }

            if (Verified.HasValue)
            {
                parameters.Append($"&verified={Verified.Value.ToString().ToLower()}");
            }

            if (Vivify.HasValue)
            {
                parameters.Append($"&vivify={Vivify.Value.ToString().ToLower()}");
            }


            return new SearchQuery($"{initialQuery}{parameters.ToString()}", pageIndex, false);
        }

        public void ResetSearchParameters()
        {
            AI = true;
            Chroma = null;
            Cinema = null;
            Curated = null;
            Environments = [];
            Followed = null;
            From = new DateTime(2018, 5, 8);
            FullSpread = null;
            Leaderboard = SearchParamLeaderboard.Undefined;
            MaxBlStars = null;
            MaxBpm = null;
            MaxDownVotes = null;
            MaxDuration = null;
            MaxNps = 0;
            MaxRating = null;
            MaxSsStars = null;
            MaxUpVotes = null;
            MaxVotes = null;
            Me = null;
            MinBlStars = null;
            MinBpm = null;
            MinDownVotes = null;
            MinDuration = null;
            MinNps = 0;
            MinRating = null;
            MinSsStars = null;
            MinUpVotes = null;
            MinVotes = null;
            Noodle = null;
            Relevance = SearchParamRelevance.Undefined;
            Query = string.Empty;
            Tags = [];
            To = DateTime.Today;
            Verified = null;
            Vivify = null;
        }
    }
}
