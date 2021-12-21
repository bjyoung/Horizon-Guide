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
using System.Windows.Shapes;

namespace Horizontal_Guide
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
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

        // TODO Copy of MainWindow's CanExecute, there should be a way to prevent repetition
        // Only close if information window is active
        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsActive;
        }

        // TODO Copy of MainWindow's Executed, there should be a way to prevent repetition
        // Close window when close command is executed
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
