<p align='center'>
    <img src="https://github.com/itonx/Desktiny.WinUI/blob/main/assets/desktiny.png"/>
</p>

# Desktiny.WinUI

Boost your WinUI app development with the features offered by Desktiny.WinUI:

- Quick navigation setup.
- Add an option to change the theme of your app at runtime.
- Fast setup for multi-language apps.
- Add a progress bar or a custom control in front of the app content.
- Simple dialog service ready to use.

These and more features are available in Desktiny.WinUI. Using this library will make you to focus on your app and forget about complicated initial setups.

# Install the package

You can install Desktiny.WinUI by using the NuGet UI included in Visual Studio or by using the next command:

```bash
dotnet add package Desktiny.WinUI --version 1.0.0
```

NuGet page: <a href="https://www.nuget.org/packages/Desktiny.WinUI/" target="_blank">Desktiny.WinUI</a>

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
    xmlns:desktiny="using:Desktiny.WinUI"
    ...>

    <!--Replace default content with Winston-->
    <desktiny:Winston>
        <TextBlock
            HorizontalAlignment="Center"
            VerticalAlignment="Center"
            FontSize="20">
            Hello World!
        </TextBlock>
    </desktiny:Winston>

</Window>
```

# Additional Setup

## Maximize app at startup

Set `MaximizeAtStartup` to `True`.

```xml
    <desktiny:Winston MaximizeAtStartup="True">
        <TextBlock
            HorizontalAlignment="Center"
            VerticalAlignment="Center"
            FontSize="20">
            Hello World!
        </TextBlock>
    </desktiny:Winston>
```

## Nocturne

`Winston` contains the `NocturneContent` property that you can use to display a control on top of your app. All the content in the window will reduce the opacity while `Nocturne` is visible. This is useful for scenarios that need to display a progress bar while loading stuff.

```xml
<desktiny:Winston IsNocturneVisible="False" MaximizeAtStartup="True">
    <StackPanel>
        <!--  Window content here ...  -->
    </StackPanel>
    <!--  Nocturne  -->
    <desktiny:Winston.NocturneContent>
        <StackPanel
            HorizontalAlignment="Center"
            VerticalAlignment="Center"
            Orientation="Horizontal"
            Spacing="10">
            <ProgressRing
                Width="50"
                Height="50"
                HorizontalAlignment="Center"
                VerticalAlignment="Center"
                IsIndeterminate="True" />
            <TextBlock VerticalAlignment="Center" FontSize="30">Loading...</TextBlock>
        </StackPanel>
    </desktiny:Winston.NocturneContent>
</desktiny:Winston>
```

Set `IsNocturneVisible` to True/False to display or hide `Nocturne`.

<img src="https://github.com/itonx/Desktiny.WinUI/blob/main/assets/NocturneHidden.png">
<img src="https://github.com/itonx/Desktiny.WinUI/blob/main/assets/NocturneVisible.png">

> The `IsNocturneVisible` property supports binding.

## EnableTitleBarThemeButtons

WinUI 3 sets the style for the TitleBar buttons based on the `Accent` style for your Windows. You can enable `EnableTitleBarThemeButtons` to disable the `Accent` style and set the background according to the App's theme (light/dark).

Default `Accent` style:

<img src="https://github.com/itonx/Desktiny.WinUI/blob/main/assets/titlebar_accent.png">

`EnableTitleBarThemeButtons` style:

> Note: It uses WinUI's `ApplicationPageBackgroundThemeBrush` background.

<img src="https://github.com/itonx/Desktiny.WinUI/blob/main/assets/titlebar_theme.png">

## Set up multi-language

The next steps are required for all WinUI 3 applications that need to provide multi-language support. Some steps are described in [Localize your WinUI 3 app](https://learn.microsoft.com/en-us/windows/apps/winui/winui3/localize-winui3-app). However, the full implementation is not there.

The next steps will help you to support multi-language in your app.

> Note: `Desktiny` will save the configuration for your app using the `Settings.Lang` key.

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

> Entries must be added in the `Resources.resw` file for each language. Otherwise, your app might display empty values for a language.

Then, you can use these entries in your app by setting the `x:Uid` in the control where you want to use an entry. Following up the previous example for `WelcomeMessage.Text`, we can use the entry in a TextBlock as follows:

```xml
<TextBlock x:Uid="WelcomeMessage"/>
```

The same applies to `WelcomeMessage.Content` for a `Button`:

```xml
<Button x:Uid="WelcomeMessage" />
```

After setting up multi-language in your app, you can use one of the language classes included in Desktiny to use multi-language in code-behind.

### LanguageService and LangService

This classes help you to retrieve the language values added in your app, so that you can use them in code-behind.

```csharp
string defaultTitle = LangService.GetLangValue("DialogTitleDefault/Text");

//or

LanguageServie languageService = new LanguageService();
string defaultTitle = _languageService.GetLangValue("DialogTitleDefault/Text");

//Later, you can use defaultTitle to set a control's text or bind a property
```

> NOTE: Dots must be replaced by slashes when using an entry in code-behind. This is default behavior in WinUI 3.

### DialogLangService and DialogLang

These classes help you to display simple dialogs for Yes/No options and simple information dialogs that support multi-language.

```csharp
//Replace 'DialogTest/Text' with the entry in your Resources.resw
bool continueProcess = await DialogLang.ShowYesNoAsync("DialogTest/Text");

await DialogLang.ShowInformationAsync("DialogOKTest/Text");
```

> `DialogLangService` contains the same methods but it has to be instanciated.

The next entries are provided by default in `Desktiny.WinUI` for `en-US` and `es-MX` languages:

- DialogTitleDefault.Text
- DialogCloseDefault.Text
- DialogCloseNo.Text
- DialogCloseYes.Text
- DialogTest.Text
- DialogOKTest.Text

You can override the values by creating the same entries in the `Resources.resw` files of your project. Make sure you set up your app to support multi-language.

### ComboBoxChangeLanguageBehavior

This behavior can be attached to a ComboBox. It detects the languages added in your `Package.appxmanifest`. It will change the language of you app when a different language is selected. The app will restart after changing the language (this is required for WinUI 3 apps).

You must add entry values for each language option in your `Resources.resw` files using the following naming convention: LangOptionEN-US/Content

> `EN-US` must be replaced by the language you added in your app (in uppercase).

Default languages included in Desktiny are: `en-US` and `es-MX`. The next entries are provided by default:

- LangOptionEN-US.Content
- LangOptionES-MX.Content

The `ComboBoxChangeLanguageBehavior` will use the values from those entries to display the language in the ComboBox as options.

Use behavior:

```xml
<Window
    ...
    xmlns:desktiny="using:Desktiny.WinUI"
    xmlns:behaviors="using:Desktiny.WinUI.Behaviors"
    xmlns:interactivity="using:Microsoft.Xaml.Interactivity">
    <desktiny:Winston>
        <ComboBox>
            <interactivity:Interaction.Behaviors>
                <behaviors:ComboBoxChangeLanguageBehavior />
            </interactivity:Interaction.Behaviors>
        </ComboBox>
    </desktiny:Winston>
</Window>
```

# Set up themes

The `AppThemeModel` was created to facilitate theme management.
The next configuration works for a MVVM configuration. You can use a library like `CommunityToolkit.Mvvm` to set up your application for MVVM.

> Note: `Desktiny` will save the configuration for your app using the `Settings.Theme` key.

1. Add an `AppThemeModel` property in your view model (for binding).

```csharp
public partial class MainViewModel : ObservableObject
{
    //CommunityToolkit.Mvvm will create the `CurrentAppTheme` that can be used in XAML
    [ObservableProperty]
    private AppThemeModel _currentAppTheme;
}
```

2. Create a list of `AppThemeModel`s and register them using `ThemeService.RegisterThemes`. This is the list of all themes that users can use in your app.

> Note: Light and Dark themes are default themes in WinUI 3. If you need to override the default styles, add them to the list and provide the path of the resource dictionaries where you override the styles. `ThemeService.RegisterThemes` will add all themes sorted by name.

```csharp
public MainWindow()
{
    InitializeComponent();
    List<AppThemeModel> themes = new()
    {
        new AppThemeModel("Light", ElementTheme.Light, "Resources/OverrideWinUITheme.xaml", "\uE793"),
        new AppThemeModel("Dark", ElementTheme.Dark, "Resources/OverrideWinUITheme.xaml", "\uF0CE"),
        new AppThemeModel("Neuromancer", ElementTheme.Dark, "Resources/NeuromancerTheme.xaml", "\uE950")
    };
    ThemeService.RegisterThemes(themes);
}
```

The class `AppThemeModel` receives the next parameters:

- Name: the name of your theme.
- ElementTheme: the style of your theme (light or dark).
  - ElementTheme is the default theme resource provided by WinUI 3. Desktiny extends the functionality that allows you to create custom themes. All themes must specify whether they're going to be in light style, dark style, or both.
- Resource: the resource dictionary that contains the styles for your theme or override default styles. IMPORTANT: All theme dictionaries must end in `Theme.xaml`.
- Icon: the string icon that can be use in a FontIcon.Glyph (this is used by the `ThemeButton` component provided by Desktiny).

3. Use `ThemeService.GetTheme` to get the current theme for your app (the first time is executed, it will return the first theme in the list of `AppThemeModel`s) and set the property in your view model.

```csharp
public MainWindow()
{
    //...
    ThemeService.RegisterThemes(themes);
    AppThemeModel theme = ThemeService.GetTheme();
    var vm = new MainViewModel();
    vm.CurrentAppTheme = theme;

    this.WinstonContainer.DataContext = vm;
}
```

4. Use the view model's property to set the `AppTheme` property for `Winston`.

> Note: All theme's resources are loaded by `Winston` every time the `AppTheme` is changed.

```xml
<desktiny:Winston AppTheme="{Binding CurrentAppTheme, Mode=OneWay}">
<!--  More XALM here  -->
</desktiny:Winston>
```

### Resources used for this example.

Neuromancer:

> Note: Notice `Neuromancer` was registered for dark theme only, so there's no implementation for light theme. However, both keys are specified in the dictionary to align the design with WinUI 3's best practices.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ResourceDictionary
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="using:Desktiny.WinUI.Demo.Resources">
    <ResourceDictionary.ThemeDictionaries>
        <ResourceDictionary x:Key="Light" />
        <ResourceDictionary x:Key="Dark">
            <!--  Override Default styles or add yours  -->
            <SolidColorBrush x:Key="TextFillColorPrimary" Color="#00FF05" />
        </ResourceDictionary>
    </ResourceDictionary.ThemeDictionaries>
</ResourceDictionary>
```

Custom resource for Light/Dark themes (default WinUI 3). One dictionary can be used for light/dark default themes:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ResourceDictionary
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="using:Desktiny.WinUI.Demo.Resources">
    <ResourceDictionary.ThemeDictionaries>
        <ResourceDictionary x:Key="Light">
            <!--  Override Default styles for Light theme  -->
        </ResourceDictionary>
        <ResourceDictionary x:Key="Dark">
            <!--  Override Default styles for Dark theme  -->
        </ResourceDictionary>
    </ResourceDictionary.ThemeDictionaries>
</ResourceDictionary>

```

## ThemeButton control

`Desktiny` provides the `ThemeButton` control to change the themes you registered for your app. Following up the previous configuration, you can use the view model's property to implement the control.

> Note: `ThemeToIconConverter` is provided by `Desktiny` to set the icon for `FontIcon`.

```xml
<Window
    ...
    xmlns:behaviors="using:Desktiny.WinUI.Behaviors"
    xmlns:converters="using:Desktiny.WinUI.Converters">
    <StackPanel>
        <StackPanel.Resources>
            <converters:ThemeToIconConverter x:Key="ThemeToIcon" />
        </StackPanel.Resources>
        <TextBlock
            VerticalAlignment="Center"
            Foreground="{ThemeResource TextFillColorPrimary}"
            Text="{Binding CurrentAppTheme.Name}" />
        <desktiny:ThemeButton AppTheme="{Binding CurrentAppTheme, Mode=TwoWay}">
            <FontIcon Glyph="{Binding CurrentAppTheme, Converter={StaticResource ThemeToIcon}, Mode=OneWay}" />
        </desktiny:ThemeButton>
    </StackPanel>
</Window>
```

<p align='center'>
    <img src="https://github.com/itonx/Desktiny.WinUI/blob/main/assets/light.png"/>
</p>
<p align='center'>
    <img src="https://github.com/itonx/Desktiny.WinUI/blob/main/assets/dark.png"/>
</p>
<p align='center'>
    <img src="https://github.com/itonx/Desktiny.WinUI/blob/main/assets/neuromancer.png"/>
</p>
