using Challenge4.Model;
using Challenge4.ViewModel;

using System.Windows;
using System.Windows.Controls;

namespace Challenge4.View
{
    /// <summary>
    /// Interaction logic for MemberListView.xaml
    /// </summary>
    public partial class MemberListView : UserControl
    {
        public MemberListView()
        {
            InitializeComponent();
        }

        private void treeMemberList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is Member member)
                (DataContext as MemberViewModel)?.SetSelectedMember(member);
        }
    }
}
