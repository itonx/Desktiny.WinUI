using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Desktiny.WinUI.Tools;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Windows.Graphics;

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

        public static void MakeSplashScreen(this Window window, int width, int height)
        {
            var hwnd = Tools.WindowHelper.GetWindowHandle(window);
            WindowId winId = Win32Interop.GetWindowIdFromWindow(hwnd);
            AppWindow appW = AppWindow.GetFromWindowId(winId);

            OverlappedPresenter presenter = appW.Presenter as OverlappedPresenter;
            presenter.IsAlwaysOnTop = true;
            presenter.IsMaximizable = false;
            presenter.IsMinimizable = false;
            presenter.IsResizable = false;
            appW.IsShownInSwitchers = false;
            appW.Resize(new SizeInt32(width, height));

            window.Title = "";
            presenter.SetBorderAndTitleBar(false, false);

            DwmHelper.RemoveBorder(hwnd);
            DwmHelper.StripStyles(hwnd);
        }
    }
}
