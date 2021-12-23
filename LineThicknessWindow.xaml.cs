using System;
using System.Windows;
using System.Windows.Controls;

namespace Horizontal_Guide
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : CloseableWindow
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void OKButton_OnClick(object sender, RoutedEventArgs e)
        {
            // Close current window when clicked and send value back to original window
            Close();
        }

        private void LineThicknessCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If user selects a new value from the list, update the line thickness in main
            ComboBoxItem combo_box_item = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            string selected_text = combo_box_item.Content.ToString();

            if (selected_text != "")
            {
                int new_line_thickness = int.Parse(selected_text);

                try
                {
                    ((MainWindow)Application.Current.MainWindow).UpdateLineThickness(new_line_thickness);

                }
                catch (Exception ex)
                {
                    // Do nothing, just to catch exception in case main window closes
                }
            }
        }
    }
}
