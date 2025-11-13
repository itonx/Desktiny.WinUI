using Microsoft.UI.Xaml.Controls;

namespace Desktiny.WinUI.Tools
{
    public static class DesktinyExtensions
    {
        public static Grid GetAppTitleBar(this Winston winston)
        {
            return WinUI3Helper.FindChildElementByName(winston, "AppTitleBar") as Grid;
        }

        public static Grid GetClientContainer(this Winston winston)
        {
            return WinUI3Helper.FindChildElementByName(winston, "ClientContainer") as Grid;
        }

        public static Grid GetWindowContainer(this Winston winston)
        {
            return WinUI3Helper.FindChildElementByName(winston, "WindowContainer") as Grid;
        }
    }
}
