﻿using System;
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
            Slider slider = sender as Slider;
            Thumb thumb = get_thumb(slider);

            // If thumb cannot be retrieved, stop here
            if(thumb == null)
            {
                return;
            }

            // Get position of thumb relative to slider
            Point thumb_relative_location = thumb.TranslatePoint(new Point(0, 0), slider);

            // Calculate and set line height to new value
            double slider_height = slider.Height;
            double thumb_y = thumb_relative_location.Y;
            double thumb_height = thumb.ActualHeight / 2;
            set_line_height(thumb_y + thumb_height);

            // Set title
            Point pointToWindow = Mouse.GetPosition(this);
            Point pointToScreen = PointToScreen(pointToWindow);
            Title = "slider_height = " + slider_height + ", thumb_y = " + Math.Round(thumb_y) + ", thumb_height = " + thumb_height;
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

        private void set_line_height(double new_height) 
        {
            var line = horizon_guide as Line;
            line.Y1 = new_height;
            line.Y2 = new_height;
        }
    }
}
