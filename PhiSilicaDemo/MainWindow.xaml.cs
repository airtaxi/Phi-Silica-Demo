using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using WinUIEx;
using PhiSilicaDemo.Pages;
using Microsoft.Windows.AI.Generative;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PhiSilicaDemo
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WindowEx
    {
        public static EventHandler MainTitleBarPaneToggleRequested;
        public static EventHandler MainTitleBarRunButtonClicked;

        public MainWindow()
        {
            InitializeComponent();

            this.CenterOnScreen();

            AppWindow.SetIcon("logo.ico");

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(MainTitleBar);

            SystemBackdrop = new MicaBackdrop();

            ContentFrame.Navigate(typeof(MainPage));
        }

        public static bool IsEnabled
        {
            get => App.MainWindow.ContentFrame.IsEnabled;
            set => App.MainWindow.ContentFrame.IsEnabled = value;
        }

        private void OnMainTitleBarPaneToggleRequested(Microsoft.UI.Xaml.Controls.TitleBar sender, object args) => MainTitleBarPaneToggleRequested?.Invoke(this, EventArgs.Empty);
        private void OnMainTitleBarRunButtonClicked(object sender, RoutedEventArgs e) => MainTitleBarRunButtonClicked?.Invoke(this, EventArgs.Empty);

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!LanguageModel.IsAvailable()) await LanguageModel.MakeAvailableAsync();
        }

        private void OnClosed(object sender, WindowEventArgs args)
        {
            args.Handled = true;
            App.CommonLanguageModel?.Dispose();
            Process.GetCurrentProcess().Kill();
        }
    }
}
