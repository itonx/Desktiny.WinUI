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

## Maximize the app at startup

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
