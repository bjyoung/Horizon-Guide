using System.Windows.Input;

namespace Horizontal_Guide
{
    // A collection of custom commands for Horizon Guide
    public static class CustomCommands
    {
        // Command closes window when triggered
        // Shortcut: CTRL + W
        public static readonly RoutedCommand Close = new RoutedCommand
            (
                "Close",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.W, ModifierKeys.Control)
                }
            );
    }
}
