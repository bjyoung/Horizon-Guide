using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfScreenHelper;

namespace HorizontalGuide{
    // Initial window with the adjustable line and UI buttons
    public partial class MainWindow : CloseableWindow{

        private CloseableWindow _thicknessWindow = null;

        private CloseableWindow _informationWindow = null;

        private const double _disabledButtonOpacity = 0.35;

        public MainWindow() {
            InitializeComponent();
        }

        // Get next screen besides the one the app is currently on
        private Screen GetOtherScreen() {
            IList<Screen> screenList = Screen.AllScreens.ToList();
            Screen secondaryScreen = null;
            Screen currentScreen = null;

            foreach (Screen screen in screenList) {
                if (IsCurrentScreen(screen)) {
                    currentScreen = screen;
                    break;
                }
            }

            if (currentScreen != null) {
                secondaryScreen = GetNextScreen(screenList, currentScreen);
            } else {
                Console.WriteLine("No secondary screen found");
            }

            return secondaryScreen;
        }

        private static Screen GetNextScreen(IList<Screen> screenList, Screen currentScreen) {
            int currentScreenIndex = screenList.IndexOf(currentScreen);
            int nextScreenIndex = currentScreenIndex + 1 >= screenList.Count ? 0 : currentScreenIndex + 1;
            return screenList[nextScreenIndex];
        }

        // When line thumb is moved, move line to match its height
        private void LineHeightSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            double thumbHeight = CalculateThumbHeight(LineHeightSlider, e.NewValue);
            SetLineHeight(thumbHeight, HorizonGuide);
        }

        private static Thumb GetThumb(Slider slider) {
            ControlTemplate sliderTemplate = slider.Template;

            // Because changing the slider's initial value makes the template null for a split second at the beginning
            if (sliderTemplate == null) {
                return null;
            }

            Track track = sliderTemplate.FindName("PART_Track", slider) as Track;
            return track == null ? null : track.Thumb;
        }

        private static double CalculateThumbHeight(Slider slider, double value) {
            // Get half of thumb height
            Thumb thumb = GetThumb(slider);
            double thumbCenterHeight = 0.0;

            if (thumb != null) {
                thumbCenterHeight = thumb.ActualHeight / 2;
            }

            // Find ratio of thumb y-position to slider height
            double sliderRange = slider.Maximum - slider.Minimum;
            double thumbRatio = (value * 1.0) / sliderRange;
            double thumbCenterHeightRatio = thumbCenterHeight * thumbRatio;

            // Multiply thumb ratio by slider height
            double sliderRealHeight = slider.ActualHeight - 5.5;
            double thumbRelativeY = sliderRealHeight * thumbRatio;
            double thumbActualY = sliderRealHeight - thumbRelativeY + thumbCenterHeightRatio;

            return thumbActualY;
        }

        private static void SetLineHeight(double newHeight, Line horizon) {
            if (horizon == null) {
                return;
            }

            horizon.Y1 = newHeight;
            horizon.Y2 = newHeight;
        }

        // Adjust line's x-values so it covers the entire screen length-wise
        private void HorizonGuide_OnLoad(object sender, RoutedEventArgs e) {
            Line horizon = sender as Line;
            horizon.X2 = FirstWindow.Width;
        }

        // Adjust slider's height so it covers the entire screen length-wise
        private void LineHeightSlider_OnLoad(object sender, RoutedEventArgs e) {
            Slider heightSlider = sender as Slider;
            heightSlider.Height = FirstWindow.Height;
        }

        // Switch to the next screen, if it exists
        private void ChangeScreenButton_OnClick(Object sender, RoutedEventArgs e) {
            Screen alternateScreen = GetOtherScreen();

            if (alternateScreen == null) {
                return;
            }

            FirstWindow.WindowState = WindowState.Normal;
            MatchWindowToScreen(FirstWindow, alternateScreen);
            FirstWindow.WindowState = WindowState.Maximized;
        }

        // Make line visible or invisible
        private void LineVisibilityButton_OnClick(object sender, RoutedEventArgs e) {
            Button visibilityButton = sender as Button;

            // Find out line's current visibility setting
            Visibility lineVisibility = HorizonGuide.Visibility;

            // If line if visible, then hide it
            if (lineVisibility == Visibility.Visible) {
                HorizonGuide.Visibility = Visibility.Hidden;
                LineHeightSlider.Visibility = Visibility.Hidden;

                visibilityButton.Content = FindResource("Show");

            }

            if (lineVisibility == Visibility.Hidden) {
                HorizonGuide.Visibility = Visibility.Visible;
                LineHeightSlider.Visibility = Visibility.Visible;
                visibilityButton.Content = FindResource("Hide");
            }
        }

        // Change line color to match the color picker
        private void LineColorPicker_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            HorizonGuide.Stroke = new SolidColorBrush(LineColorPicker.SelectedColor.Value);
        }

        private static CloseableWindow SetupSubwindow(CloseableWindow currentWindow, CloseableWindow newWindow) {
            // If sub window already exists, activate existing window
            if (!IsClosed(currentWindow)) {
                currentWindow.Activate();
                return currentWindow;
            }

            // Open number drop-down list and when value changes, update line thickness
            newWindow.ShowInTaskbar = false;
            newWindow.Owner = Application.Current.MainWindow;
            newWindow.Show();
            return newWindow;
        }

        // Open sub-window to update line thickness
        private void LineThicknessButton_OnClick(object sender, RoutedEventArgs e) {
            LineThicknessWindow lineThicknessWindow = new();
            _thicknessWindow = SetupSubwindow(_thicknessWindow, lineThicknessWindow);
        }

        // Check if window is closed or not
        private static bool IsClosed(Window window) {
            return window == null || window.IsLoaded != true;
        }

        public void UpdateLineThickness(int newThickness) {
            HorizonGuide.StrokeThickness = newThickness;
        }

        // Open information sub-window
        private void InformationButton_OnClick(object sender, RoutedEventArgs e) {
            InformationWindow infoWindowTemp = new();
            _informationWindow = SetupSubwindow(_informationWindow, infoWindowTemp);
        }

        // Slider setup once it is rendered
        private void FirstWindow_ContentRendered(object sender, EventArgs e) {
            if (LineHeightSlider != null){
                double thumbHeight = CalculateThumbHeight(LineHeightSlider, LineHeightSlider.Value);
                SetLineHeight(thumbHeight, HorizonGuide);
            }

            // Set FirstWindow properties
            FirstWindow.WindowState = WindowState.Normal;
            MatchWindowToScreen(FirstWindow, Screen.AllScreens.First());
            FirstWindow.WindowState = WindowState.Maximized;
        }

        // Close sub-windows if main window is closing
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (_thicknessWindow != null) {
                _thicknessWindow.Close();
            }

            if (_informationWindow != null) {
                _informationWindow.Close();
            }
        }

        // If there is only one monitor, then disable Change Screen button
        private void ChangeScreenButton_OnLoad(object sender, RoutedEventArgs e) {
            if (HasMultipleScreens() == false) {
                DisableButton(ChangeScreenButton);
            }
        }

        // Disable button and make it transparent
        private static void DisableButton(Button button) {
            button.IsEnabled = false;
            button.Opacity = _disabledButtonOpacity;
        }

        // Checks if given screen is the screen the app is currently on
        // FirstWindow must be set
        private Boolean IsCurrentScreen(Screen screen) {
            return screen.WorkingArea.Top == FirstWindow.Top && screen.WorkingArea.Left == FirstWindow.Left;
        }

        private static Boolean HasMultipleScreens() {
            IEnumerable<Screen> screenList = Screen.AllScreens;
            return screenList.Count() > 1;
        }

        private static void MatchWindowToScreen(Window window, Screen screen) {
            window.Top = screen.WorkingArea.Top;
            window.Left = screen.WorkingArea.Left;
            window.Width = screen.WorkingArea.Width;
            window.Height = screen.WorkingArea.Height;
        }
    }
}
