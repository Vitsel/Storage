using Challenge5.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Challenge5.Control
{
    /// <summary>
    /// Interaction logic for ExtChartItem.xaml
    /// </summary>
    public partial class ExtChartItem : UserControl
    {
        public static readonly DependencyProperty dpMostCount = DependencyProperty.Register("MaxValue", typeof(long), typeof(ExtChartItem));
        public long MaxValue
        {
            get => (long)GetValue(dpMostCount);
            set => SetValue(dpMostCount, value);
        }

        public ExtChartItem()
        {
            InitializeComponent();
        }
    }
}