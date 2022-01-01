using System.Windows;
using System.Windows.Input;

namespace Horizontal_Guide
{
    // Window that can be closed using CTRL + W shortcut
    public partial class CloseableWindow : Window
    {
        // Only close if window is active
        public void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsActive;
        }

        // Close window when close command is executed
        public void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
