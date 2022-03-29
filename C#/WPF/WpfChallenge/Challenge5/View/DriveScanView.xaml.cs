using Challenge5.ViewModel;

using System.Windows.Controls;

namespace Challenge5.View
{
    /// <summary>
    /// Interaction logic for DriveScanView.xaml
    /// </summary>
    public partial class DriveScanView : UserControl
    {
        public DriveScanView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            (DataContext as MainViewModel)?.ScanDriveAsync();
        }
    }
}
