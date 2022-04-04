using Challenge5.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Challenge5.Control
{
    /// <summary>
    /// Interaction logic for ExtChartItem.xaml
    /// </summary>
    public partial class ExtChartItem : UserControl
    {
        public static readonly DependencyProperty dpMaxValue = DependencyProperty.Register("MaxValue", typeof(long), typeof(ExtChartItem));
        public static readonly DependencyProperty dpContentBackground = DependencyProperty.Register("ContentBackground", typeof(Brush), typeof(ExtChartItem));
        public long MaxValue
        {
            get => (long)GetValue(dpMaxValue);
            set => SetValue(dpMaxValue, value);
        }

        public Brush ContentBackground
        {
            get => (Brush)GetValue(dpContentBackground);
            set => SetValue(dpContentBackground, value);
        }

        public ExtChartItem()
        {
            InitializeComponent();
        }
    }
}