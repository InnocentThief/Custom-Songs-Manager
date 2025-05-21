using CSM.Business.Interfaces;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace CSM.UiLogic.ViewModels
{
    internal sealed class MainWindowViewModel : BaseViewModel
    {
        #region Public Properties

        public NavigationViewModel Navigation { get; }

        public ObservableCollection<WorkspaceViewModel> Workspaces { get; } = [];

        public bool CanRun { get; private set; } = true;

        #endregion

        public MainWindowViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            var userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();
            userConfigDomain.LoadOrCreateUserConfig();

            Navigation = new NavigationViewModel(serviceLocator);
            Navigation.SelectionChanged += NavigationSelectionChanged;
        }

        public void NavigateToDefaultWorkspace()
        {
            Navigation.NavigateToDefaultWorkspace();
        }

        private async void NavigationSelectionChanged(object? sender, EventArgs e)
        {
            if (sender is NavigationItemViewModel navigationItem)
            {
                var workspace = Workspaces.FirstOrDefault(ws => ws.GetType() == navigationItem.ViewModelType);
                if (workspace == null && navigationItem.ViewModelType != null)
                {
                    workspace = ServiceLocator.GetService(navigationItem.ViewModelType) as WorkspaceViewModel;
                    if (workspace != null)
                    {
                        Workspaces.Add(workspace);
                    }
                }

                var collectionView = CollectionViewSource.GetDefaultView(Workspaces);
                if (collectionView.CurrentItem != null)
                {
                    // deactivate ??
                }
                if (workspace != null)
                {
                    collectionView.MoveCurrentTo(workspace);
                    await workspace.ActivateAsync();
                }
            }
        }
    }
}
