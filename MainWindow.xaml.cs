using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using WpfScreenHelper;
using System.Collections.Generic;

namespace Horizontal_Guide{
    // Initial window with the adjustable line and UI buttons
    public partial class MainWindow : CloseableWindow{
        // To keep track of a reference to line thickness sub-window
        private Window thickness_window = null;

        // To keep track of reference to information sub-window
        private Window information_window = null;

        // How clear should a button look when disabled
        private double disabled_button_opacity = 0.35;

        // Constructor
        public MainWindow(){
            InitializeComponent();
        }

        // TODO make this work for any number of screens
        // Get secondary screen
        // Assumes that there are at most two screens
        private Screen get_other_screen(){
            IEnumerable<Screen> screen_list = Screen.AllScreens;
            Screen secondary_screen = null;

            foreach (Screen screen in screen_list){
                if (screen.WorkingArea.Top != FirstWindow.Top || screen.WorkingArea.Left != FirstWindow.Left){
                    secondary_screen = screen;
                    break;
                }
            }

            if (secondary_screen == null){
                Console.WriteLine("No secondary screen found");
            }

            return secondary_screen;
        }

        // When line thumb is moved, move line to match its height
        private void LineHeightSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e){
            double thumb_height = calculate_thumb_height(LineHeightSlider, e.NewValue);
            set_line_height(thumb_height, HorizonGuide);
        }

        // Get thumb part of slider
        private static Thumb get_thumb(Slider slider){
            ControlTemplate slider_template = slider.Template;

            // Because changing the slider's initial value makes the template null for a split second at the beginning
            if (slider_template == null){
                return null;
            }

            var track = slider_template.FindName("PART_Track", slider) as Track;
            return track == null ? null : track.Thumb;
        }

        // Calculate actual thumb height
        private static double calculate_thumb_height(Slider slider, double value){
            // Get half of thumb height
            Thumb thumb = get_thumb(slider);
            double thumb_center_height = 0.0;

            if (thumb != null){
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

            return thumb_actual_y;
        }

        // Set line height
        private static void set_line_height(double new_height, Line horizon){
            if (horizon == null){
                return;
            }

            horizon.Y1 = new_height;
            horizon.Y2 = new_height;
        }

        // Adjust line's x-values so it covers the entire screen length-wise
        private void HorizonGuide_OnLoad(object sender, RoutedEventArgs e){
            Line horizon = sender as Line;
            horizon.X2 = FirstWindow.Width;
        }

        // Adjust slider's height so it covers the entire screen length-wise
        private void LineHeightSlider_OnLoad(object sender, RoutedEventArgs e){
            Slider height_slider = sender as Slider;
            height_slider.Height = FirstWindow.Height;
        }

        // Switch to the next screen, if it exists
        private void ChangeScreenButton_OnClick(Object sender, RoutedEventArgs e){
            Screen alternate_screen = get_other_screen();

            if (alternate_screen == null){
                return;
            }

            FirstWindow.WindowState = WindowState.Normal;
            FirstWindow.Top = alternate_screen.WorkingArea.Top;
            FirstWindow.Left = alternate_screen.WorkingArea.Left;
            FirstWindow.Width = alternate_screen.WorkingArea.Width;
            FirstWindow.Height = alternate_screen.WorkingArea.Height;
            FirstWindow.WindowState = WindowState.Maximized;
        }

        // Make line visible or invisible
        private void LineVisibilityButton_OnClick(object sender, RoutedEventArgs e){
            Button visibility_button = sender as Button;

            // Find out line's current visibility setting
            Visibility line_visibility = HorizonGuide.Visibility;

            // If line if visible, then hide it
            if (line_visibility == Visibility.Visible){
                HorizonGuide.Visibility = Visibility.Hidden;
                visibility_button.Content = FindResource("Show");

            }

            if (line_visibility == Visibility.Hidden){
                HorizonGuide.Visibility = Visibility.Visible;
                visibility_button.Content = FindResource("Hide");
            }
        }

        // Change line color to match the color picker
        private void LineColorPicker_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e){
            HorizonGuide.Stroke = new SolidColorBrush(LineColorPicker.SelectedColor.Value);
        }

        // 
        private void LineThicknessButton_OnClick(object sender, RoutedEventArgs e){
            if (IsClosed(thickness_window)){
                thickness_window = null;
            }

            // If line thickness window was created already, make it active instead of creating a window
            if (thickness_window != null){
                thickness_window.Activate();
                return;
            }

            // Open number drop-down list and when value changes, update line thickness
            LineThicknessWindow line_thickness_window = new();
            line_thickness_window.ShowInTaskbar = false;
            line_thickness_window.Owner = Application.Current.MainWindow;
            thickness_window = line_thickness_window;
            line_thickness_window.Show();
        }

        // Check if window is closed or not
        private static bool IsClosed(Window window){
            return window == null || window.IsLoaded != true;
        }

        public void UpdateLineThickness(int new_thickness){
            HorizonGuide.StrokeThickness = new_thickness;
        }

        // Open information sub-window
        private void InformationButton_OnClick(object sender, RoutedEventArgs e){
            // TODO InformationButton_OnClick and LineThicknessButton_OnClick are really similar and could be simplified
            // If information window window is already closed, then set its tracker variable to null
            if (IsClosed(information_window)){
                information_window = null;
            }

            // If line thickness window was created already, make it active instead of creating a window
            if (information_window != null){
                information_window.Activate();
                return;
            }

            // Open information window
            InformationWindow info_window_temp = new();
            info_window_temp.ShowInTaskbar = false;
            info_window_temp.Owner = Application.Current.MainWindow;
            information_window = info_window_temp;
            info_window_temp.Show();
        }

        // Slider setup once it is rendered
        private void FirstWindow_ContentRendered(object sender, EventArgs e){
            if (LineHeightSlider != null){
                double thumb_height = calculate_thumb_height(LineHeightSlider, LineHeightSlider.Value);
                set_line_height(thumb_height, HorizonGuide);
            }
        }

        // Close sub-windows if main window is closing
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e){
            if (thickness_window != null){
                thickness_window.Close();
            }

            if (information_window != null){
                information_window.Close();
            }
        }

        // If there is only one monitor, then disable Change Screen button
        private void ChangeScreenButton_OnLoad(object sender, RoutedEventArgs e){
            if (get_other_screen() == null){
                disable_button(ChangeScreenButton);
            }
        }

        // Disable the button and make it transparent
        private void disable_button(Button button){
            button.IsEnabled = false;
            button.Opacity = disabled_button_opacity;
        }
    }
}
