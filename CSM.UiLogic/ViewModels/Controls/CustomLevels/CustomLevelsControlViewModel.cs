using CSM.Business.Interfaces;
using CSM.DataAccess.CustomLevels;
using CSM.Framework.Extensions;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Common.CustomLevels;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace CSM.UiLogic.ViewModels.Controls.CustomLevels
{
    internal class CustomLevelsControlViewModel(IServiceLocator serviceLocator) : BaseViewModel(serviceLocator)
    {
        private readonly IUserConfigDomain userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();

        public ObservableCollection<ICustomLevelViewModel> CustomLevels { get; } = [];

        public string CustomLevelCount => $"{CustomLevels.Count} custom levels loaded";

        public async Task LoadAsync(bool refresh)
        {
            if (CustomLevels.Count > 0 && !refresh)
                return;

            LoadingInProgress = true;

            var path = userConfigDomain?.Config?.CustomLevelPaths.First().Path;
            if (string.IsNullOrEmpty(path))
                return;
            if (!Path.Exists(path))
                return;
            CustomLevels.AddRange(await LoadCustomLevelsAsync(path));
            OnPropertyChanged(nameof(CustomLevelCount));

            LoadingInProgress = false;
        }

        #region Helper methods

        private async Task<List<ICustomLevelViewModel>> LoadCustomLevelsAsync(string path)
        {
            var retval = new List<ICustomLevelViewModel>();

            var directories = Directory.GetDirectories(path);
            foreach (var directory in directories)
            {
                var infoFile = Path.Combine(directory, "Info.dat");
                var directoryInfo = new DirectoryInfo(directory);

                if (!File.Exists(infoFile))
                    continue;

                var content = await File.ReadAllTextAsync(infoFile);

                try
                {
                    var bsrKey = directoryInfo.Name[..directoryInfo.Name.IndexOf(' ')];
                    var lastWriteTime = directoryInfo.LastWriteTime;

                    if (content.Contains("_version"))
                    {
                        var customLevel = JsonSerializer.Deserialize<InfoV2>(content);
                        if (customLevel == null)
                            continue;
                        retval.Add(new CustomLevelV2ViewModel(ServiceLocator, customLevel, directory, bsrKey, lastWriteTime));

                    }
                    else
                    {
                        var customLevel = JsonSerializer.Deserialize<InfoV4>(content);
                        if (customLevel == null)
                            continue;
                        retval.Add(new CustomLevelV4ViewModel(ServiceLocator, customLevel, directory, bsrKey, lastWriteTime));
                    }
                }
                catch (Exception ex)
                {
                    // todo Log error
                    continue;
                }
            }

            return retval;
        }

        #endregion
    }
}
