using Challenge5.Model;
using Challenge5.ViewModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

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

        private void ListBoxItem_Selected(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!(DataContext is MainViewModel vm))
                return;
            
            vm.SelectFileContainer(((ListBoxItem)e.Source).DataContext as FileContainer);
        }

        private void ListBoxItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(((ListBoxItem)e.Source).DataContext is FileItem file))
                return;

            (DataContext as MainViewModel)?.OpenDirectory(System.IO.Path.GetDirectoryName(file.Name));
        }

        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ((Rectangle)btnCancel.Template.FindName("rectBtnCancel", btnCancel)).Fill = new SolidColorBrush(Colors.DarkRed);
        }
    }
}