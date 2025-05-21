using CSM.UiLogic.AbstractBase;
using System.Windows;
using Telerik.Windows.Controls;

namespace CSM.App.Views.Windows
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : RadWindow
    {
        private IBaseEditViewModel? viewModel;

        public IBaseEditViewModel? ViewModel
        {
            get => viewModel;
            set
            {
                UnwireEvents();
                viewModel = value;
                WireEvents();
                DataContext = viewModel;
            }
        }

        public EditWindow()
        {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
        }

        private void UnwireEvents()
        {
            if (viewModel != null)
            {
                viewModel.Close -= Close;
            }
        }

        private void WireEvents()
        {
            if (viewModel != null)
            {
                viewModel.Close += Close;
            }
        }

        private void Close(object? sender, EventArgs e)
        {
            UnwireEvents();
            Close();
        }
    }
}
