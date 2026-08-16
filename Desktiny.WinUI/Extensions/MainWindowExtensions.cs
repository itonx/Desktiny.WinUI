using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

namespace Desktiny.WinUI.Extensions
{
    public static class MainWindowExtensions
    {
        public static nint GetWindowHandle(this Window window)
        {
            return Tools.WindowHelper.GetWindowHandle(window);
        }

        public static WindowId GetWindowId(this Window window)
        {
            return Tools.WindowHelper.GetWindowId(window);
        }

        public static AppWindow GetAppWindow(this Window window)
        {
            return Tools.WindowHelper.GetAppWindow(window);
        }

        public static void HideAppWindow(this Window window)
        {
            Tools.WindowHelper.HideAppWindow(window);
        }

        public static void MinimizeAppWindow(this Window window)
        {
            Tools.WindowHelper.MinimizeAppWindow(window);
        }

        public static async Task SetWindowPositionToCenter(this Window window)
        {
            await Tools.WindowHelper.SetWindowPositionToCenter(window);
        }
    }
}
