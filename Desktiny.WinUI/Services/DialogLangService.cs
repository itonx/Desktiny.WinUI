using Desktiny.WinUI.Tools;
using Desktiny.WinUI.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace Desktiny.WinUI.Services
{
    public interface IDialogLangService
    {
        Task ShowInformationAsync(string messageResourceKey, string titleResourceKey = Constants.Global.DIALOG_TITLE_DEFAULT, string primaryButtonTextResourceKey = Constants.Global.DIALOG_CLOSE_DEFAULT);
        Task<bool> ShowYesNoAsync(string messageResourceKey, string titleResourceKey = Constants.Global.DIALOG_TITLE_DEFAULT, string primaryButtonTextResourceKey = Constants.Global.DIALOG_CLOSE_NO, string secondaryButtonResourceKey = Constants.Global.DIALOG_CLOSE_YES, ContentDialogResult expectedResult = ContentDialogResult.Primary);
    }
    public class DialogLangService : IDialogLangService
    {
        private readonly ILanguageService _languageService;

        public DialogLangService()
        {
            _languageService = new LanguageService();
        }

        public async Task ShowInformationAsync(string messageResourceKey, string titleResourceKey = Constants.Global.DIALOG_TITLE_DEFAULT, string primaryButtonTextResourceKey = Constants.Global.DIALOG_CLOSE_DEFAULT)
        {
            ContentDialog dialog = BuildDialog(titleResourceKey, messageResourceKey, primaryButtonTextResourceKey);
            await dialog.ShowAsync();
        }

        public async Task<bool> ShowYesNoAsync(string messageResourceKey, string titleResourceKey = Constants.Global.DIALOG_TITLE_DEFAULT, string primaryButtonTextResourceKey = Constants.Global.DIALOG_CLOSE_YES, string secondaryButtonResourceKey = Constants.Global.DIALOG_CLOSE_NO, ContentDialogResult expectedResult = ContentDialogResult.Primary)
        {
            ContentDialog dialog = BuildDialog(titleResourceKey, messageResourceKey, primaryButtonTextResourceKey);
            dialog.SecondaryButtonText = _languageService.GetLangValue(secondaryButtonResourceKey);
            ContentDialogResult result = await dialog.ShowAsync();
            return result == expectedResult;
        }

        private ContentDialog BuildDialog(string titleResourceKey, string messageResourceKey, string primaryButtonTextResourceKey = Constants.Global.DIALOG_CLOSE_DEFAULT)
        {
            string title = _languageService.GetLangValue(titleResourceKey);
            string message = _languageService.GetLangValue(messageResourceKey);
            string primaryButtonText = _languageService.GetLangValue(primaryButtonTextResourceKey);

            Window mainWindow = WindowHelper.CurrentMainWindow;
            ContentDialog dialog = new ContentDialog();

            dialog.XamlRoot = mainWindow.Content.XamlRoot;
            dialog.RequestedTheme = (mainWindow.Content as Winston).AppTheme.ElementTheme;
            dialog.Title = title;
            dialog.PrimaryButtonText = primaryButtonText;
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = new SimpleDialogView(message);

            return dialog;
        }
    }

    public static class DialogLang
    {
        private static DialogLangService _dialogService = new();

        public static async Task ShowInformationAsync(string messageResourceKey, string titleResourceKey = Constants.Global.DIALOG_TITLE_DEFAULT, string primaryButtonTextResourceKey = Constants.Global.DIALOG_CLOSE_DEFAULT)
        {
            await _dialogService.ShowInformationAsync(messageResourceKey, titleResourceKey, primaryButtonTextResourceKey);
        }

        public static async Task<bool> ShowYesNoAsync(string messageResourceKey, string titleResourceKey = Constants.Global.DIALOG_TITLE_DEFAULT, string primaryButtonTextResourceKey = Constants.Global.DIALOG_CLOSE_YES, string secondaryButtonResourceKey = Constants.Global.DIALOG_CLOSE_NO, ContentDialogResult expectedResult = ContentDialogResult.Primary)
        {
            return await _dialogService.ShowYesNoAsync(messageResourceKey, titleResourceKey, primaryButtonTextResourceKey, secondaryButtonResourceKey, expectedResult);
        }
    }
}
