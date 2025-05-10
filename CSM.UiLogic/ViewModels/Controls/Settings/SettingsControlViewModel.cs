using CSM.Business.Interfaces;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Controls.Settings
{
    internal class SettingsControlViewModel : BaseEditViewModel
    {
        public override string Title => "Custom Songs Manager - Settings";

        public GeneralSettingsViewModel GeneralSettings { get; }

        public CustomLevelsSettingsViewModel CustomLevelsSettings { get; }

        public PlaylistsSettingsViewModel PlaylistsSettings { get; }

        public LeaderboardsSettingsViewModel LeaderboardsSettings { get; }

        public SettingsControlViewModel(IServiceLocator serviceLocator) : base(serviceLocator, "Cancel", EditViewModelCommandColor.Default, "Save", EditViewModelCommandColor.Default)
        {
            var userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();

            GeneralSettings = new GeneralSettingsViewModel(serviceLocator, userConfigDomain.Config!);
            CustomLevelsSettings = new CustomLevelsSettingsViewModel(serviceLocator, userConfigDomain.Config!);
            PlaylistsSettings = new PlaylistsSettingsViewModel(serviceLocator, userConfigDomain.Config!);
            LeaderboardsSettings = new LeaderboardsSettingsViewModel(serviceLocator, userConfigDomain.Config!);
        }
    }
}
