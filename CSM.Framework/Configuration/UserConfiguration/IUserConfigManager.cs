namespace CSM.Framework.Configuration.UserConfiguration
{
    /// <summary>
    /// Interface for the <see cref="UserConfigManager"/>.
    /// </summary>
    public interface IUserConfigManager
    {
        /// <summary>
        /// UserConfig holded by the <see cref=" UserConfigManager"/>.
        /// </summary>
        UserConfig Config { get; }
    }
}