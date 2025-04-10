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

        public ObservableCollection<CustomLevelViewModel> CustomLevels { get; } = [];

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

            LoadingInProgress = false;
        }

        #region Helper methods

        private async Task<List<CustomLevelViewModel>> LoadCustomLevelsAsync(string path)
        {
            var retval = new List<CustomLevelViewModel>();

            var directories = Directory.GetDirectories(path);
            foreach (var directory in directories)
            {
                var infoFile = Path.Combine(directory, "Info.dat");
                var directoryInfo = new DirectoryInfo(directory);

                if (!File.Exists(infoFile))
                    continue;

                var content = await File.ReadAllTextAsync(infoFile);
                CustomLevel? customLevel;
                try
                {
                    customLevel = JsonSerializer.Deserialize<CustomLevel>(content);

                    if (customLevel == null)
                        continue;

                    var bsrKey = directoryInfo.Name[..directoryInfo.Name.IndexOf(' ')];
                    var lastWriteTime = directoryInfo.LastWriteTime;

                    retval.Add(new CustomLevelViewModel(ServiceLocator, directory, customLevel, bsrKey, lastWriteTime));
                }
                catch (Exception)
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
