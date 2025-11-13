using Desktiny.WinUI.Tools;
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;
using System;

namespace Desktiny.WinUI.Behaviors
{
    public class MaximizeWindowAtStartupBehavior : Behavior<Winston>
    {
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObject_Loaded;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(AssociatedObject.MainWindow);
            PInvoke.MaximizeWindow(hWnd);
        }
    }
}
