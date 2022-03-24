using Challenge3.Model;
using Challenge3.ViewModel;

using System.Windows;

namespace Challenge3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserListViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = DataContext as UserListViewModel;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //var newUser = new User();
            var addUser = new AddUserDlg();
            //addUser.DataContext = newUser;

            if(addUser.ShowDialog() == true)
                viewModel.AddUser(addUser.User);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (UserList.SelectedItem == null)
                return;

            viewModel.RemoveUser((User)UserList.SelectedItem);
        }
    }
}