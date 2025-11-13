using Desktiny.WinUI.Tools;
using System;

namespace Desktiny.WinUI.Services
{
    public interface IUIThreadService
    {
        void Execute(Action rutine);
    }

    public class UIThreadService : IUIThreadService
    {
        public void Execute(Action rutine)
        {
            AppDispatcher.UIThreadDispatcher?.TryEnqueue(() =>
            {
                rutine.Invoke();
            });
        }
    }
}
