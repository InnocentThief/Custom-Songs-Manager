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

        public ScoreSaberSettingsViewModel ScoreSaberSettings { get; }

        public BeatLeaderSettingsViewModel BeatLeaderSettings { get; }

        public SongSuggestSettingsViewModel SongSuggestSettings { get; }

        public SettingsControlViewModel(IServiceLocator serviceLocator) : base(serviceLocator, "Cancel", EditViewModelCommandColor.Default, "Save", EditViewModelCommandColor.Default)
        {
            var userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();

            GeneralSettings = new GeneralSettingsViewModel(serviceLocator, userConfigDomain.Config!);
            CustomLevelsSettings = new CustomLevelsSettingsViewModel(serviceLocator, userConfigDomain.Config!);
            PlaylistsSettings = new PlaylistsSettingsViewModel(serviceLocator, userConfigDomain.Config!);
            ScoreSaberSettings = new ScoreSaberSettingsViewModel(serviceLocator, userConfigDomain.Config!);
            BeatLeaderSettings = new BeatLeaderSettingsViewModel(serviceLocator, userConfigDomain.Config!);
            SongSuggestSettings = new SongSuggestSettingsViewModel(serviceLocator, userConfigDomain.Config!);
        }
    }
}
