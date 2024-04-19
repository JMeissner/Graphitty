using Graphitty.ViewModel;
using MahApps.Metro.Controls;
using System.Windows;

namespace Graphitty.View
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainView : MetroWindow
    {
        #region Public Constructors

        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        #endregion Public Constructors

        #region Private Methods

        private void toggleSettingsFlyout(object sender, RoutedEventArgs e)
        {
            Settings.IsOpen = !Settings.IsOpen;
        }

        #endregion Private Methods
    }
}