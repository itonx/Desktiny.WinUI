using Microsoft.UI.Dispatching;

namespace Desktiny.WinUI.Tools
{
    public static class AppDispatcher
    {
        public static DispatcherQueue UIThreadDispatcher { get; set; }
    }
}
