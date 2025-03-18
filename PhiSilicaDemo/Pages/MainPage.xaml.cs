using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PhiSilicaDemo.Pages.Menus;
using System;

namespace PhiSilicaDemo.Pages;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        MainWindow.MainTitleBarPaneToggleRequested += OnMainTitleBarPaneToggleRequested;
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        MainWindow.MainTitleBarPaneToggleRequested -= OnMainTitleBarPaneToggleRequested;
    }

    private void OnMainTitleBarPaneToggleRequested(object sender, EventArgs e) => MainNavigationView.IsPaneOpen = !MainNavigationView.IsPaneOpen;

    private void OnNavigationViewSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        var selectedItem = args.SelectedItem as NavigationViewItem;
        if (selectedItem == PromptGenerationNavigationViewItem) ContentFrame.Navigate(typeof(PromptGenerationPage));
        else if (selectedItem == SummaryNavigationViewItem) ContentFrame.Navigate(typeof(SummaryPage));
        else if (selectedItem == RewriteNavigationViewItem) ContentFrame.Navigate(typeof(RewritePage));
        else if (selectedItem == TextRecognitionNavigationViewItem) ContentFrame.Navigate(typeof(TextRecognitionPage));
        else if (selectedItem == UpscaleImageNavigationViewItem) ContentFrame.Navigate(typeof(UpscaleImagePage));
        else if (selectedItem == DescribeImageNavigationViewItem) ContentFrame.Navigate(typeof(DescribeImagePage));
    }
}
