using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.AI.Generative;
using System;
using System.IO;
using TextControlBoxNS;

namespace PhiSilicaDemo.Pages.Menus;

public sealed partial class PromptGenerationPage : Page
{
    public PromptGenerationPage()
    {
        InitializeComponent();

        ResultMarkdownTextBlock.Config = new();

        MainTextControlBox.SelectSyntaxHighlightingById(SyntaxHighlightID.CSharp);
        MainTextControlBox.FindDescendant<TextBox>().IsTabStop = false;

        var script = File.ReadAllText("Assets/Scripts/PromptGeneration.txt");
        MainTextControlBox.LoadText(script);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        App.CommonLanguageModel?.Dispose();
    }

    private async void OnRunButtonClicked(object sender, RoutedEventArgs e)
    {
        MainWindow.IsEnabled = false;
        MainProgressBar.Visibility = Visibility.Visible;

        using LanguageModel languageModel = await LanguageModel.CreateAsync();
        App.CommonLanguageModel = languageModel;

        string prompt = "Provide the molecular formula for paradichlorobenzene in sentence not starting with quotes.";

        ResultMarkdownTextBlock.Text = string.Empty;
        var op = languageModel.GenerateResponseWithProgressAsync(prompt);
        op.Progress += (info, delta) => DispatcherQueue.TryEnqueue(() => ResultMarkdownTextBlock.Text += delta);
        await op;

        MainProgressBar.Visibility = Visibility.Collapsed;
        MainWindow.IsEnabled = true;
    }
}
