using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.ViewModels.Controls.SongSources.Twitch
{
    internal class TwitchChannelViewModel : BaseViewModel
    {
        private IRelayCommand? joinCommand, leaveCommand, removeCommand;
        private bool joined = false;
        private string name = string.Empty;

        public IRelayCommand? JoinCommand => joinCommand ??= CommandFactory.CreateFromAsync(Join, CanJoin);

        public IRelayCommand? LeaveCommand => leaveCommand ??= CommandFactory.Create(Leave, CanLeave);

        public IRelayCommand? RemoveCommand => removeCommand ??= CommandFactory.Create(Remove, CanRemove);

        public bool Joined
        {
            get => joined;
            set
            {
                if (value == joined)
                    return;
                joined = value;
                OnPropertyChanged();
                JoinCommand?.RaiseCanExecuteChanged();
                LeaveCommand?.RaiseCanExecuteChanged();
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if (value == name)
                    return;
                name = value;
                OnPropertyChanged();
                JoinCommand?.RaiseCanExecuteChanged();    
            }
        }

        public TwitchChannelViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        #region Helper methods

        private async Task Join()
        {
            Joined = true;
            await Task.CompletedTask;
        }

        private bool CanJoin()
        {
            return !Joined && !string.IsNullOrWhiteSpace(Name);
        }

        private void Leave()
        {
            Joined = false;
        }

        private bool CanLeave()
        {
            return Joined;
        }

        private void Remove()
        {

        }

        private bool CanRemove()
        {
            return true;
        }

        #endregion
    }
}
