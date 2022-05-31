using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Wizards
{
    public abstract class EditWindowBaseViewModel: ObservableObject
    {
        #region Public Properties

        /// <summary>
        /// Title of the message box (shown in the title bar of the dialog).
        /// </summary>
        public abstract string Title { get; }

        public RelayCommand CancelCommand { get; }

        public RelayCommand ContinueCommand { get; }

        public abstract int Height { get; }

        public abstract int Width { get; }

        #endregion

        protected EditWindowBaseViewModel()
        {

        }

        /// <summary>
        /// Action to be executed when to close the dialog.
        /// </summary>
        public Action CloseAction { get; set; }

        #region Helper methods

        private void Cancel()
        {

        }

        public void CanCancel()
        {

        }

        public void Continue()
        {

        }

        public void CanContinue()
        {

        }

        #endregion
    }
}
