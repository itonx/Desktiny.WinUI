using Desktiny.WinUI.Demo.Enums;
using Desktiny.WinUI.EventMessages;
using Desktiny.WinUI.Managers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Desktiny.WinUI.Demo.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BuyMeACoffeePage : Page
    {
        public BuyMeACoffeePage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EventManager.Publish(new EnumNavigation(DesktinyPages.Welcome));
        }
    }
}
