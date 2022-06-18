using System;

namespace CSM.DataAccess.Entities.Offline
{
    public class ReceivedBeatmap
    {
        public string ChannelName { get; set; }

        public DateTime ReceivedAt { get; set; }

        public string Key { get; set; }

        public string Hash { get; set; }

        public string SongName { get; set; }

        public string LevelAuthorName { get; set; }

        public string SongAuthorName { get; set; }
    }
}