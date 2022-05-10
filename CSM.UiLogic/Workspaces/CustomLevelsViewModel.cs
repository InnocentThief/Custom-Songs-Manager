using CSM.Services;
using CSM.UiLogic.Workspaces.CustomLevels;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CSM.UiLogic.Workspaces
{
    /// <summary>
    /// ViewModel for the Custom Levels workspace.
    /// </summary>
    public class CustomLevelsViewModel : BaseWorkspaceViewModel
    {
        #region Private fields

        private ListCollectionView itemsCollection;
        private ObservableCollection<CustomLevelViewModel> itemsObservable;
        private CustomLevelViewModel selectedCustomLevel;
        private CustomLevelDetailViewModel customLevelDetail;
        private BeatMapService beatMapService;

        #endregion

        #region Properties

        /// <summary>
        /// Contains all the custom levels sorted by default sort as defined in <see cref="DefaultSort"/>.
        /// </summary>
        public ListCollectionView CustomLevels => itemsCollection;

        /// <summary>
        /// Gets the custom level count.
        /// </summary>
        public string CustomLevelCount
        {
            get
            {
                if (CustomLevels.Count == 1) return $"1 custom level loaded.";
                return $"{CustomLevels.Count} custom levels loaded";
            }
        }

        /// <summary>
        /// Gets or sets the currently selected custom level.
        /// </summary>
        public CustomLevelViewModel SelectedCustomLevel
        {
            get => selectedCustomLevel;
            set
            {
                if (value != selectedCustomLevel) return;
                selectedCustomLevel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the viewmodel for the detail area.
        /// </summary>
        public CustomLevelDetailViewModel CustomLevelDetail
        {
            get => customLevelDetail;
            set
            {
                if (value != customLevelDetail) return;
                customLevelDetail = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Command used to refresh the workspace data.
        /// </summary>
        public AsyncRelayCommand RefreshCommand { get; }

        /// <summary>
        /// Gets the workspace type.
        /// </summary>
        public override WorkspaceType WorkspaceType => WorkspaceType.CustomLevels;

        #endregion

        /// <summary>
        /// Initializes a new <see cref="CustomLevelsViewModel"/>.
        /// </summary>
        public CustomLevelsViewModel()
        {
            beatMapService = new BeatMapService();
            itemsObservable = new ObservableCollection<CustomLevelViewModel>();
            itemsCollection = DefaultSort();
            RefreshCommand = new AsyncRelayCommand(RefreshAsync);
        }

        /// <summary>
        /// Loads the data in asynchronous fashion.
        /// </summary>
        /// <returns></returns>
        public override async Task LoadDataAsync()
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Unloads the data.
        /// </summary>
        public override void UnloadData()
        {

        }

        #region Helper methods

        private ListCollectionView DefaultSort()
        {
            var collection = new ListCollectionView(itemsObservable);
            collection.SortDescriptions.Add(new SortDescription("", ListSortDirection.Descending));
            return collection;
        }

        private async Task RefreshAsync()
        {
            await LoadDataAsync();
        }

        #endregion
    }
}