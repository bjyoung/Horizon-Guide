using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace Horizontal_Guide
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void line_height_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Get slider as reference
            Slider height_slider = sender as Slider;
            match_line_to_slider(horizon_guide, height_slider);
        }

        private void match_line_to_slider(Line line, Slider slider)
        {
            // Get slider as reference
            Thumb thumb = get_thumb(slider);

            // If thumb cannot be retrieved, stop here
            if (thumb == null)
            {
                return;
            }

            // Get position of thumb relative to slider
            Point thumb_relative_location = thumb.TranslatePoint(new Point(0, 0), slider);

            // Calculate and set line height to new value
            double slider_height = slider.Height;
            double thumb_y = thumb_relative_location.Y;
            double thumb_height = thumb.ActualHeight / 2;
            set_line_height(thumb_y + thumb_height, line);
        }

        private static Thumb get_thumb(Slider slider)
        {
            var slider_template = slider.Template;

            // Because changing the slider's initial value makes the template null for a split second at the beginning
            if(slider_template == null)
            {
                return null;
            }

            var track = slider_template.FindName("PART_Track", slider) as Track;
            return track == null ? null : track.Thumb;
        }

        private void set_line_height(double new_height, Line horizon) 
        {
            horizon.Y1 = new_height;
            horizon.Y2 = new_height;
        }

        private void horizon_guide_OnLoad(object sender, RoutedEventArgs e)
        {
            // Adjust line's x-values so it covers the entire screen length-wise
            Line horizon = sender as Line;
            horizon.X2 = FirstWindow.Width;

            // Move line to where thumb currently is
            match_line_to_slider(horizon, line_height_slider);
        }

        private void line_height_slider_OnLoad(object sender, RoutedEventArgs e)
        {
            // Adjust slider's height so it covers the entire screen length-wise
            Slider height_slider = sender as Slider;
            height_slider.Height = FirstWindow.Height;
        }

        private void line_visibility_button_OnClick(object sender, RoutedEventArgs e)
        {
            Button visibility_button = sender as Button;

            // Find out line's current visibility setting
            Visibility line_visibility = horizon_guide.Visibility;

            // If line if visible, then hide it
            if(line_visibility == Visibility.Visible)
            {
                horizon_guide.Visibility = Visibility.Hidden;
                visibility_button.Content = FindResource("Show");

            }

            if(line_visibility == Visibility.Hidden)
            {
                horizon_guide.Visibility = Visibility.Visible;
                visibility_button.Content = FindResource("Hide");
            }
        }

        private void line_color_button_OnClick(object sender, RoutedEventArgs e)
        {
            // Grab color picker
        }
    }
}
