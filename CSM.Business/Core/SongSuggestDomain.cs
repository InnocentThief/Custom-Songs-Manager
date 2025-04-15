using CSM.Business.Interfaces;
using Settings;
using SongSuggestNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.Business.Core
{
    internal class SongSuggestDomain(IUserConfigDomain userConfigDomain) : ISongSuggestDomain
    {
        #region Private fields

        private SongSuggest? songSuggest;

        private readonly IUserConfigDomain userConfigDomain = userConfigDomain;

        #endregion

        public async Task InitializeAsync()
        {
            var settings = new CoreSettings()
            {
                FilePathSettings = new FilePathSettings(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Custom Songs Manager", "SongSuggest")),
                
            };

            songSuggest = new SongSuggest(settings);
            //await Task.Run(songSuggest.Initialize();
        }
    }
}
