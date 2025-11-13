using Desktiny.WinUI.Services;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Desktiny.WinUI.Demo
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
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
    }
}
