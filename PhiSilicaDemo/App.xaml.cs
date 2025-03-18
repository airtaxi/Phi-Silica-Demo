using Microsoft.UI.Xaml;
using Microsoft.Windows.AI.Generative;
using System;
using System.IO;

namespace PhiSilicaDemo;

public partial class App : Application
{
    public static LanguageModel CommonLanguageModel { get; set; }
    public static MainWindow MainWindow { get; private set; }

    public App()
    {
        // Could be different if app is launched from shortcut or command line
        Environment.CurrentDirectory = AppContext.BaseDirectory;

        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        MainWindow = new MainWindow();
        MainWindow.Activate();
    }
}
