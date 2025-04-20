using CSM.Business.Core.SongSelection;
using CSM.Business.Interfaces;
using CSM.DataAccess.CustomLevels;
using CSM.Framework.Extensions;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.ViewModels.Common.CustomLevels;
using CSM.UiLogic.ViewModels.Controls.SongSources;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace CSM.UiLogic.ViewModels.Controls.CustomLevels
{
    internal class CustomLevelsControlViewModel(IServiceLocator serviceLocator) : BaseViewModel(serviceLocator), ISongSourceViewModel
    {
        #region Private fields

        private IRelayCommand? openInFileExplorerCommand;
        private IRelayCommand? refreshCommand;
        private ICustomLevelViewModel? selectedCustomLevel;
        private IRelayCommand? deleteCustomLevelCommand;

        private readonly ILogger<CustomLevelsControlViewModel> logger = serviceLocator.GetService<ILogger<CustomLevelsControlViewModel>>();
        private readonly IBeatSaverService beatSaverService = serviceLocator.GetService<IBeatSaverService>();
        private readonly ISongSelectionDomain songSelectionDomain = serviceLocator.GetService<ISongSelectionDomain>();
        private readonly IUserConfigDomain userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();

        #endregion

        #region Properties

        public ObservableCollection<ICustomLevelViewModel> CustomLevels { get; } = [];

        public ICustomLevelViewModel? SelectedCustomLevel
        {
            get => selectedCustomLevel;
            set
            {
                if (value == selectedCustomLevel)
                    return;
                selectedCustomLevel = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelectedCustomLevel));
                UpdateCommands();
            }
        }

        public bool HasSelectedCustomLevel
        {
            get => SelectedCustomLevel != null;
        }

        public string CustomLevelCount => $"{CustomLevels.Count} custom levels loaded";

        public string CustomLevelPath => userConfigDomain.Config?.CustomLevelsConfig.CustomLevelPath.Path ?? "";

        public IRelayCommand OpenInFileExplorerCommand => openInFileExplorerCommand ??= CommandFactory.Create(OpenInFileExplorer, CanOpenInFileExplorer);

        public IRelayCommand RefreshCommand => refreshCommand ??= CommandFactory.CreateFromAsync(RefreshAsync, CanRefresh);

        public IRelayCommand DeleteCustomLevelCommand => deleteCustomLevelCommand ??= CommandFactory.Create(Delete, CanDelete);

        #endregion

        public async Task LoadAsync(bool refresh)
        {
            if (CustomLevels.Count > 0 && !refresh)
                return;

            SetLoadingInProgress(true, "Loading custom levels...");

            CustomLevels.Clear();
            var path = userConfigDomain?.Config?.CustomLevelsConfig.CustomLevelPath.Path;
            if (string.IsNullOrEmpty(path) || !Path.Exists(path))
            {
                SetLoadingInProgress(false, string.Empty);
                return;
            }
            CustomLevels.AddRange(await LoadCustomLevelsAsync(path));
            OnPropertyChanged(nameof(CustomLevelCount));

            SetLoadingInProgress(false, string.Empty);
        }

        public async Task LoadSelectedCustomLevelDataAsync()
        {
            if (SelectedCustomLevel == null)
                return;

            var mapDetail = await beatSaverService.GetMapDetailAsync(SelectedCustomLevel.BsrKey);
            if (mapDetail == null)
                return;

            // todo: which hash to use? latest? based on what?
            var hashes = mapDetail.Versions.OrderBy(v => v.CreatedAt).Select(v => v.Hash).ToList();
            if (hashes.Count == 0)
                return;

            songSelectionDomain.SetSongHash(hashes.Last(), SongSelectionType.Right);
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
                    logger.LogError(ex, $"Error loading custom level from {infoFile}");
                    continue;
                }
            }

            return retval;
        }

        private void OpenInFileExplorer()
        {
            string path = userConfigDomain.Config?.CustomLevelsConfig.CustomLevelPath.Path ?? "";
            if (string.IsNullOrWhiteSpace(path))
                return;

            if (selectedCustomLevel != null)
                path = selectedCustomLevel.Path;

            var psi = new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = $"/select,\"{path}\"",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private bool CanOpenInFileExplorer()
        {
            var directory = userConfigDomain.Config?.CustomLevelsConfig.CustomLevelPath.Path;
            return !string.IsNullOrWhiteSpace(directory);
        }

        private async Task RefreshAsync()
        {
            await LoadAsync(true);
        }

        private bool CanRefresh()
        {
            return true;
        }

        private void Delete()
        {
            if (selectedCustomLevel is ICustomLevelViewModel customLevelViewModel)
            {
                if (!Directory.Exists(customLevelViewModel.Path))
                    return;

                Directory.Delete(customLevelViewModel.Path, true);
                CustomLevels.Remove(customLevelViewModel);

                SelectedCustomLevel = null;
            }
        }

        private bool CanDelete()
        {
            return selectedCustomLevel != null;
        }

        private void UpdateCommands()
        {
            OpenInFileExplorerCommand.RaiseCanExecuteChanged();
            DeleteCustomLevelCommand.RaiseCanExecuteChanged();
        }

        #endregion
    }
}
