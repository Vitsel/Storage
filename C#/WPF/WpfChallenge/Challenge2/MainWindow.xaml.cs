using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Challenge2
{
    static class ListRectangleExtension
    {
        public static Rect? GetContains(this List<Rect> list, Point point)
        {
            foreach (var area in list)
            {
                if (area.Contains(point))
                    return area;
            }

            return null;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum MoveTo
        {
            Up,
            Down,
            Left,
            Right
        }

        private List<Rect> areas;

        public MainWindow()
        {
            InitializeComponent();

            MinHeight = 220 + SystemParameters.CaptionHeight;
            MinWidth = 380 + SystemParameters.BorderWidth;

            areas = new List<Rect>();
            double factor;
            using (var src = new HwndSource(new HwndSourceParameters()))
                factor = src.CompositionTarget.TransformFromDevice.M11;

            foreach (var screen in Screen.AllScreens)
                areas.Add(new Rect(screen.WorkingArea.X / factor, screen.WorkingArea.Y * factor, screen.WorkingArea.Width * factor, screen.WorkingArea.Height * factor));
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            Top -= GetCanMove(MoveTo.Up, 10);
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            Top += GetCanMove(MoveTo.Down, 10);
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            Left -= GetCanMove(MoveTo.Left, 10);
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            Left += GetCanMove(MoveTo.Right, 10);
        }

        private void btnBig_Click(object sender, RoutedEventArgs e)
        {
            var canTop = GetCanMove(MoveTo.Up, 100);
            var canLeft = GetCanMove(MoveTo.Left, 100);
            var canRight = GetCanMove(MoveTo.Right, 100);
            var canBottom = GetCanMove(MoveTo.Down, 100);

            double moveTop = 100 > (canTop + canBottom) ? canTop : Math.Min(canTop, Math.Max(50, 100 - canBottom));
            double moveLeft = 100 > (canLeft + canRight) ? canLeft : Math.Min(canLeft, Math.Max(50, 100 - canRight)); ;
            
            Top -= moveTop;
            Left -= moveLeft;
            Height += Math.Min(100, canBottom + moveTop);
            Width += Math.Min(100, canRight + moveLeft);
        }

        private void btnSmall_Click(object sender, RoutedEventArgs e)
        {
            var resizeWidth = Width - Math.Max(Width - 100, MinWidth);
            var resizeHeight = Height - Math.Max(Height - 100, MinHeight);

            Top += resizeHeight / 2;
            Left += resizeWidth / 2;
            Width -= resizeWidth;
            Height -= resizeHeight;
        }

        private double GetCanMove(MoveTo moveTo, double length)
        {
            try
            {
                Rect tlRect = (Rect)areas.GetContains(new Point(Left, Top));
                Rect trRect = (Rect)areas.GetContains(new Point(Left + Width, Top));
                Rect blRect = (Rect)areas.GetContains(new Point(Left, Top + Height));
                Rect brRect = (Rect)areas.GetContains(new Point(Left + Width, Top + Height));
                Rect rect1;
                Rect rect2;
                double offset1;
                double offset2;

                switch (moveTo)
                {
                    case MoveTo.Up:
                        rect1 = areas.GetContains(new Point(Left, Top - 10)) ?? tlRect;
                        rect2 = areas.GetContains(new Point(Left + Width, Top - 10)) ?? trRect;
                        offset1 = Math.Abs(rect1.Top - Top);
                        offset2 = Math.Abs(rect2.Top - Top);

                        return Math.Min(Math.Min(offset1, offset2), length);

                    case MoveTo.Down:
                        rect1 = areas.GetContains(new Point(Left, Top + Height + 10)) ?? blRect;
                        rect2 = areas.GetContains(new Point(Left + Width, Top + Height + 10)) ?? brRect;
                        offset1 = Math.Abs(rect1.Bottom - (Top + Height));
                        offset2 = Math.Abs(rect2.Bottom - (Top + Height));

                        return Math.Min(Math.Min(offset1, offset2), length);

                    case MoveTo.Left:
                        rect1 = areas.GetContains(new Point(Left - 10, Top)) ?? tlRect;
                        rect2 = areas.GetContains(new Point(Left - 10, Top + Height)) ?? blRect;
                        offset1 = Math.Abs(rect1.Left - Left);
                        offset2 = Math.Abs(rect2.Left - Left);

                        return Math.Min(Math.Min(offset1, offset2), length);

                    case MoveTo.Right:
                        rect1 = areas.GetContains(new Point(Left + Width + 10, Top)) ?? trRect;
                        rect2 = areas.GetContains(new Point(Left + Width + 10, Top + Height)) ?? brRect;
                        offset1 = Math.Abs(rect1.Right - (Left + Width));
                        offset2 = Math.Abs(rect2.Right - (Left + Width));

                        return Math.Min(Math.Min(offset1, offset2), length);

                    default:
                        return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}
