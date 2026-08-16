using System;
using System.Linq;
using System.Threading.Tasks;
using Desktiny.WinUI.Services;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Windows.Devices.Display;
using Windows.Devices.Enumeration;
using WinRT.Interop;

namespace Desktiny.WinUI.Tools
{
    public static class WindowHelper
    {
        private static AppWindow _currentAppWindow;
        private static Window _mainWindow;
        public static AppWindow CurrentAppWindow => GetAppWindow();
        public static Window CurrentMainWindow => GetMainWindow();

        private static AppWindow GetAppWindow()
        {
            if (_currentAppWindow != null)
                return _currentAppWindow;
            Window mainWindow = CurrentMainWindow;
            if (mainWindow == null)
                return null;
            IntPtr hWnd = WindowNative.GetWindowHandle(mainWindow);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            _currentAppWindow = AppWindow.GetFromWindowId(wndId);
            return _currentAppWindow;
        }

        private static Window GetMainWindow()
        {
            if (_mainWindow != null)
                return _mainWindow;
            _mainWindow = (Application.Current as IAppWindow)?.MainWindow;
            return _mainWindow;
        }

        public static nint GetWindowHandle(Window window)
        {
            return WinRT.Interop.WindowNative.GetWindowHandle(window);
        }

        public static WindowId GetWindowId(Window window)
        {
            var hwnd = GetWindowHandle(window);
            return Win32Interop.GetWindowIdFromWindow(hwnd);
        }

        public static AppWindow GetAppWindow(Window window)
        {
            return AppWindow.GetFromWindowId(GetWindowId(window));
        }

        public static void HideAppWindow(Window window)
        {
            var appW = GetAppWindow(window);
            appW.Hide();
        }

        public static void MinimizeAppWindow(Window window)
        {
            var appWindow = GetAppWindow(window);
            if (appWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.Minimize();
            }
        }

        public static async Task SetWindowPositionToCenter(Window window)
        {
            var appWindow = GetAppWindow(window);

            var displayList = await DeviceInformation.FindAllAsync(
                DisplayMonitor.GetDeviceSelector()
            );

            if (!displayList.Any())
                return;

            var monitorInfo = await DisplayMonitor.FromInterfaceIdAsync(displayList[0].Id);

            var height = monitorInfo.NativeResolutionInRawPixels.Height;
            var width = monitorInfo.NativeResolutionInRawPixels.Width;
            var centeredPosition = appWindow.Position;
            centeredPosition.X = (width - appWindow.Size.Width) / 2;
            centeredPosition.Y = (height - appWindow.Size.Height) / 2;

            appWindow.Move(centeredPosition);
        }
    }
}
