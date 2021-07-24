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

        private void LineHeightSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double thumb_height = calculate_thumb_height(LineHeightSlider, e.NewValue);
            set_line_height(thumb_height, HorizonGuide);
        }


        private static Thumb get_thumb(Slider slider)
        {
            ControlTemplate slider_template = slider.Template;

            // Because changing the slider's initial value makes the template null for a split second at the beginning
            if(slider_template == null)
            {
                return null;
            }

            var track = slider_template.FindName("PART_Track", slider) as Track;
            return track == null ? null : track.Thumb;
        }

        private double calculate_thumb_height(Slider slider, double value)
        {
            // Calculate actual thumb height based on the given thumb value
            // Get half of thumb height
            Thumb thumb = get_thumb(slider);
            double thumb_center_height = 0.0;

            if (thumb != null)
            {
                thumb_center_height = thumb.ActualHeight / 2;
            }

            // Find ratio of thumb y-position to slider height
            double slider_range = slider.Maximum - slider.Minimum;
            double thumb_ratio = (value * 1.0) / slider_range;
            double thumb_center_height_ratio = thumb_center_height * thumb_ratio;

            // Multiply thumb ratio by slider height
            double slider_real_height = slider.ActualHeight - 5.5;
            double thumb_relative_y = slider_real_height * thumb_ratio;
            double thumb_actual_y = slider_real_height - thumb_relative_y + thumb_center_height_ratio;



            // For testing
            // Title = "slider.Height = " + Math.Round(slider.Height, 4) + "; thumb_relative_y = " + Math.Round(thumb_relative_y, 2) + "; thumb_center_height = " + Math.Round(thumb_center_height, 4);

            return thumb_actual_y;
        }

        private void set_line_height(double new_height, Line horizon) 
        {
            if(horizon == null)
            {
                return;
            }

            horizon.Y1 = new_height;
            horizon.Y2 = new_height;

            // For testing
            Title = "line_height = " + new_height;
        }

        private void HorizonGuide_OnLoad(object sender, RoutedEventArgs e)
        {
            // Adjust line's x-values so it covers the entire screen length-wise
            Line horizon = sender as Line;
            horizon.X2 = FirstWindow.Width;
        }

        private void LineHeightSlider_OnLoad(object sender, RoutedEventArgs e)
        {
            // Adjust slider's height so it covers the entire screen length-wise
            Slider height_slider = sender as Slider;
            height_slider.Height = FirstWindow.Height;
        }

        private void LineVisibilityButton_OnClick(object sender, RoutedEventArgs e)
        {
            Button visibility_button = sender as Button;

            // Find out line's current visibility setting
            Visibility line_visibility = HorizonGuide.Visibility;

            // If line if visible, then hide it
            if(line_visibility == Visibility.Visible)
            {
                HorizonGuide.Visibility = Visibility.Hidden;
                visibility_button.Content = FindResource("Show");

            }

            if(line_visibility == Visibility.Hidden)
            {
                HorizonGuide.Visibility = Visibility.Visible;
                visibility_button.Content = FindResource("Hide");
            }
        }

        private void LineColorPicker_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            HorizonGuide.Stroke = new SolidColorBrush(LineColorPicker.SelectedColor.Value);
        }

        private void LineThicknessButton_OnClick(object sender, RoutedEventArgs e)
        {
            // TODO Open number drop-down list and when value changes, update line thickness
            Window1 line_thickness_window = new Window1();
            line_thickness_window.ShowDialog();
        }

        public void UpdateLineThickness(int new_thickness)
        {
            // Update line's thickness using new value
            HorizonGuide.StrokeThickness = new_thickness;
        }

        private void FirstWindow_ContentRendered(object sender, EventArgs e)
        {
            if (LineHeightSlider != null)
            {
                double thumb_height = calculate_thumb_height(LineHeightSlider, LineHeightSlider.Value);
                set_line_height(thumb_height, HorizonGuide);
            }
        }
    }
}
