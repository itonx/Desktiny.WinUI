using Desktiny.WinUI.Tools;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

namespace Desktiny.WinUI.Services
{
    public interface IAppWindow
    {
        Window MainWindow { get; }
    }

    public interface IWindowService
    {
        public Window GetMainWindow();
        public AppWindow GetAppWindowWindow();
    }

    public class WindowService : IWindowService
    {
        public AppWindow GetAppWindowWindow()
        {
            return WindowHelper.CurrentAppWindow;
        }

        public Window GetMainWindow()
        {
            return WindowHelper.CurrentMainWindow;
        }
    }
}
