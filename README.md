# Desktiny.WinUI

Boost your WinUI app development with the features offered by Desktiny.WinUI:

- Quick navigation setup.
- Add an option to change the theme of your app at runtime.
- Fast setup for multi-language apps.
- Add a progress bar or a custom control in front of the app content.
- Simple dialog service ready to use.

These and more features are available in Desktiny.WinUI. Using this library will make you to focus on your app and forget about complicated initial setups.

# Initial Setup

Implement `IAppWindow` interface in `App.xaml.cs`:

```csharp
public partial class App : Application, IAppWindow
{
    public Window MainWindow { get; private set; }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        MainWindow = new MainWindow();
        MainWindow.Activate();
    }
}
```

Add `Winston` in `MainWindow.xaml`:

```xml
<Window x:Class="YourProject.MainWindow"
    ...>

    <!--Replace default content with Winston-->
    <winui:Winston>
        <TextBlock
            HorizontalAlignment="Center"
            VerticalAlignment="Center"
            FontSize="20">
            Hello World!
        </TextBlock>
    </winui:Winston>

</Window>
```

# Additional Setup

## Maximize app at startup

Set `MaximizeAtStartup` to `True`.

```xml
    <winui:Winston MaximizeAtStartup="True">
        <TextBlock
            HorizontalAlignment="Center"
            VerticalAlignment="Center"
            FontSize="20">
            Hello World!
        </TextBlock>
    </winui:Winston>
```

## Set up multi-language

The next steps are required for all WinUI 3 applications that need to provide multi-language support. Some steps are described in [Localize your WinUI 3 app](https://learn.microsoft.com/en-us/windows/apps/winui/winui3/localize-winui3-app) however the full implementation is not there.

The next steps will help you to support multi-language in your app.

Replace the `<Resource Language="x-generate"/>` with the supported languages for your app in `Package.appxmanifest`. For example:

```xml
<Resources>
    <Resource Language="en-US"/>
    <Resource Language="es-MX"/>
</Resources>
```

> You must open the `Package.appxmanifest` using your editor to edit the XML.

Add a `Strings` folder in your project. Then create a folder for each language you added in the `Package.appxmanifest` with the name of the language. Each language folder must contain a `Resources.resx` file:

```
ProjectFolder
├───Strings
    ├───en-US
    │       Resources.resw
    │
    └───es-MX
            Resources.resw
```

> Create the `Resources.resw` file using the option in Visual Studio: Right click on the language folder > Add > New Item > Installed > C# Items > WinUI > Resources File (.resw). If you don't see those options, you need to install `WinUI` components using Visual Studio Installer.

Now, you can add all language entries in your `Resources.resw` files. Visual Studio allows you to add entries easily. All entries must be in the form of `ENTRYID.PROPERTY`:

- ENTRYID is an arbitrary value.
- PROPERTY is the property name of the WinUI component where you'are going to use the value.

If you're going to use the value in a `TextBlock`, a valid entry will look like this: `WelcomeMessage.Text`. In this example I use `Text` because that's the property used in WinUI to set the text in a `TextBlock`. Other components might use a different property to set the text. For example, a `Button` uses the `Content`, so the previous entry for a `Button` must be `WelcomeMessage.Content`.

> Entries must be added in the `Resources.resw` file for each language. Otherwise, you app might display empty values for a language.

### DialogLangService and DialogLang

After setting up multi-language in your app you can use `DialogLangService` or `DialogLang` to display simple dialogs that support multi-language. Both classes contain the same methods that received the entry name in your `Resources.resw`. `DialogLangService` must be instanciated.

```csharp
//Replace 'DialogTest/Text' with the entry in your Resources.resw
//Dots must be replaced by slashes when using an entry in the code. This is default behavior in WinUI 3.
bool continueProcess = await DialogLang.ShowYesNoAsync("DialogTest/Text");
this.YesNoResult.Text = continueProcess.ToString();

await DialogLang.ShowInformationAsync("DialogOKTest/Text");
```

The next entries are provided by default in `Desktiny.WinUI` for `en-US` and `es-MX` languages:

- DialogTitleDefault.Text
- DialogCloseDefault.Text
- DialogCloseNo.Text
- DialogCloseYes.Text

You can override the values by creating the same entries in the `Resources.resw` files of your project. Make sure you set up your app to support multi-language.
