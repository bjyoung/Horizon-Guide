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
            Slider slider = sender as Slider;

            // Get thumb position
            Thumb thumb = slider.Template.FindName("thumb", slider) as Thumb;

            // Get value
            double value = slider.Value;
            double slider_height = slider.ActualHeight;
            set_line_height(value + slider_height);

            // Set title
            Title = "Value: " + value.ToString("0.0") + "/" + slider.Maximum;
        }

        private void set_line_height(double new_height) 
        {
            var line = horizon_guide as Line;
            line.Y1 = new_height;
            line.Y2 = new_height;
        }
    }
}
