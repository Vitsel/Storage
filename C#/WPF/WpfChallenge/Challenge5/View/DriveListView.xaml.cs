using Challenge5.Model;
using Challenge5.ViewModel;
using Challenge5.Control;


using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace Challenge5.View
{
    /// <summary>
    /// Interaction logic for DriveListView.xaml
    /// </summary>
    public partial class DriveListView : UserControl
    {
        public DriveListView()
        {
            InitializeComponent();
        }

        private void DriveItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(DataContext is MainViewModel vm))
                return;

            vm.SelectedDrive = ((DriveItem)e.Source).DataContext as Drive;

            if (vm.SelectedDrive.IsValid)
                vm.SetPage(State.PageState.DriveScan);
            else
                MessageBox.Show($"분석할 수 없는 드라이브 입니다.");
        }
    }
}
