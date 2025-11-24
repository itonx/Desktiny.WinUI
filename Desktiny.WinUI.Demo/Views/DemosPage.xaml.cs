using Desktiny.WinUI.Demo.Models;
using Desktiny.WinUI.Managers;
using Desktiny.WinUI.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Desktiny.WinUI.Demo.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DemosPage : Page
    {
        public DemosPage()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            bool continueProcess = await DialogLang.ShowYesNoAsync("DialogTest/Text");
            this.YesNoResult.Text = continueProcess.ToString();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await DialogLang.ShowInformationAsync("DialogOKTest/Text");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            EventManager.Publish(new DestinyPageMessage(Enums.DesktinyPages.Welcome));
        }
    }
}
