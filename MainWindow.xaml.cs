using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using WpfScreenHelper;
using System.Collections.Generic;
using System.Linq;

namespace HorizontalGuide{
    // Initial window with the adjustable line and UI buttons
    public partial class MainWindow : CloseableWindow{
        // To keep track of a reference to line thickness sub-window
        private CloseableWindow _thicknessWindow = null;

        // To keep track of reference to information sub-window
        private CloseableWindow _informationWindow = null;

        // How clear should a button look when disabled
        private readonly double disabled_button_opacity = 0.35;

        public MainWindow(){
            InitializeComponent();
        }

        // Get secondary screen
        // Assumes that there are at most two screens
        private Screen GetOtherScreen(){
            IList<Screen> screen_list = Screen.AllScreens.ToList();
            Screen secondary_screen = null;
            Screen current_screen = null;

            foreach (Screen screen in screen_list) {
                if (IsCurrentScreen(screen)) {
                    current_screen = screen;
                    break;
                }
            }

            if(current_screen != null) {
                secondary_screen = GetNextScreen(screen_list, current_screen);
            } else {
                Console.WriteLine("No secondary screen found");
            }

            return secondary_screen;
        }

        // Given list of screens and the current screen, get the next screen in the list
        private static Screen GetNextScreen(IList<Screen> screen_list, Screen current_screen) {
            int current_screen_index = screen_list.IndexOf(current_screen);
            int next_screen_index = current_screen_index + 1 >= screen_list.Count ? 0 : current_screen_index + 1;
            return screen_list[next_screen_index];
        }

        // When line thumb is moved, move line to match its height
        private void LineHeightSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e){
            double thumb_height = CalculateThumbHeight(LineHeightSlider, e.NewValue);
            SetLineHeight(thumb_height, HorizonGuide);
        }

        // Get thumb part of slider
        private static Thumb GetThumb(Slider slider){
            ControlTemplate slider_template = slider.Template;

            // Because changing the slider's initial value makes the template null for a split second at the beginning
            if (slider_template == null){
                return null;
            }

            var track = slider_template.FindName("PART_Track", slider) as Track;
            return track == null ? null : track.Thumb;
        }

        // Calculate actual thumb height
        private static double CalculateThumbHeight(Slider slider, double value){
            // Get half of thumb height
            Thumb thumb = GetThumb(slider);
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
        private static void SetLineHeight(double new_height, Line horizon){
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
            Screen alternate_screen = GetOtherScreen();

            if (alternate_screen == null){
                return;
            }

            FirstWindow.WindowState = WindowState.Normal;
            MatchWindowToScreen(FirstWindow, alternate_screen);
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
                LineHeightSlider.Visibility = Visibility.Hidden;

                visibility_button.Content = FindResource("Show");

            }

            if (line_visibility == Visibility.Hidden){
                HorizonGuide.Visibility = Visibility.Visible;
                LineHeightSlider.Visibility = Visibility.Visible;
                visibility_button.Content = FindResource("Hide");
            }
        }

        // Change line color to match the color picker
        private void LineColorPicker_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e){
            HorizonGuide.Stroke = new SolidColorBrush(LineColorPicker.SelectedColor.Value);
        }

        private static CloseableWindow SetupSubwindow(CloseableWindow current_subwindow_ref, CloseableWindow new_subwindow) {
            // If sub window already exists, activate existing window
            if (!IsClosed(current_subwindow_ref)) {
                current_subwindow_ref.Activate();
                return current_subwindow_ref;
            }

            // Open number drop-down list and when value changes, update line thickness
            new_subwindow.ShowInTaskbar = false;
            new_subwindow.Owner = Application.Current.MainWindow;
            new_subwindow.Show();
            return new_subwindow;
        }

        // Open sub-window to update line thickness
        private void LineThicknessButton_OnClick(object sender, RoutedEventArgs e){
            LineThicknessWindow line_thickness_window = new();
            _thicknessWindow = SetupSubwindow(_thicknessWindow, line_thickness_window);
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
            InformationWindow info_window_temp = new();
            _informationWindow = SetupSubwindow(_informationWindow, info_window_temp);
        }

        // Slider setup once it is rendered
        private void FirstWindow_ContentRendered(object sender, EventArgs e){
            if (LineHeightSlider != null){
                double thumb_height = CalculateThumbHeight(LineHeightSlider, LineHeightSlider.Value);
                SetLineHeight(thumb_height, HorizonGuide);
            }

            // Set FirstWindow properties
            FirstWindow.WindowState = WindowState.Normal;
            MatchWindowToScreen(FirstWindow, Screen.AllScreens.First());
            FirstWindow.WindowState = WindowState.Maximized;
        }

        // Close sub-windows if main window is closing
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e){
            if (_thicknessWindow != null){
                _thicknessWindow.Close();
            }

            if (_informationWindow != null){
                _informationWindow.Close();
            }
        }

        // If there is only one monitor, then disable Change Screen button
        private void ChangeScreenButton_OnLoad(object sender, RoutedEventArgs e){
            if (HasMultipleScreens() == false) {
                DisableButton(ChangeScreenButton);
            }
        }

        // Disable the button and make it transparent
        private void DisableButton(Button button){
            button.IsEnabled = false;
            button.Opacity = disabled_button_opacity;
        }

        // Checks if given screen is the screen the app is currently on
        // FirstWindow must be set
        private Boolean IsCurrentScreen(Screen screen) {
            return screen.WorkingArea.Top == FirstWindow.Top && screen.WorkingArea.Left == FirstWindow.Left;
        }

        private static Boolean HasMultipleScreens() {
            IEnumerable<Screen> screen_list = Screen.AllScreens;
            return screen_list.Count() > 1;
        }

        private static void MatchWindowToScreen(Window window, Screen screen) {
            window.Top = screen.WorkingArea.Top;
            window.Left = screen.WorkingArea.Left;
            window.Width = screen.WorkingArea.Width;
            window.Height = screen.WorkingArea.Height;
        }
    }
}
