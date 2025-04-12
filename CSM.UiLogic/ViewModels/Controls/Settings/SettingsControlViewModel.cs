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

        public TwitchSettingsViewModel TwitchSettings { get; }

        public ScoreSaberSettingsViewModel ScoreSaberSettings { get; }

        public BeatLeaderSettingsViewModel BeatLeaderSettings { get; }

        public SettingsControlViewModel(IServiceLocator serviceLocator) : base(serviceLocator, "Cancel", EditViewModelCommandColor.Detault, "Save", EditViewModelCommandColor.Detault)
        {
            var userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();

            GeneralSettings = new GeneralSettingsViewModel(serviceLocator, userConfigDomain.Config!);
            CustomLevelsSettings = new CustomLevelsSettingsViewModel(serviceLocator, userConfigDomain.Config!);
            PlaylistsSettings = new PlaylistsSettingsViewModel(serviceLocator, userConfigDomain.Config!);
            TwitchSettings = new TwitchSettingsViewModel(serviceLocator, userConfigDomain.Config!);
            ScoreSaberSettings = new ScoreSaberSettingsViewModel(serviceLocator, userConfigDomain.Config!);
            BeatLeaderSettings = new BeatLeaderSettingsViewModel(serviceLocator, userConfigDomain.Config!);
        }
    }
}
