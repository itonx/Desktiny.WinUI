using Desktiny.WinUI.Attributes;
using Desktiny.WinUI.Demo.Views;

namespace Desktiny.WinUI.Demo.Enums
{
    public enum DesktinyPages
    {
        [PageType(typeof(WelcomePage))]
        Welcome,
        [PageType(typeof(DemosPage))]
        Demos,
        [PageType(typeof(BuyMeACoffeePage))]
        BuyMeACoffee,
    }
}
