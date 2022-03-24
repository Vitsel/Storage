using System.ComponentModel;
using System.Windows.Controls;

namespace Challenge1
{
    public partial class InputBox : UserControl
    {
        [Category("Title")]
        public string Title { get => (string)title.Content; set => title.Content = value; }

        public InputBox()
        {
            InitializeComponent();
        }
    }
}
