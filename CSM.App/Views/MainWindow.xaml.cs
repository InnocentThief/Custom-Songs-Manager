using System.Windows;

namespace CSM.App.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        static MainWindow()
        {
            IsWindowsThemeEnabled = false;
        }

        public MainWindow()
        {
            InitializeComponent();
            ((App)Application.Current).WindowPlace.Register(this);
        }
    }
}