using Challenge4.ViewModel;

using System.Windows;

namespace Challenge4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MemberViewModel vm { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            vm = (MemberViewModel)DataContext;
            vm.ExecuteChangePageState(State.PageState.MemberList);
        }
    }
}
