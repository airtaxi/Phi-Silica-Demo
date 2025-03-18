using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.AI.Generative;
using System;
using System.IO;
using TextControlBoxNS;

namespace PhiSilicaDemo.Pages.Menus;

public sealed partial class RewritePage : Page
{
    public RewritePage()
    {
        InitializeComponent();

        MainTextControlBox.SelectSyntaxHighlightingById(SyntaxHighlightID.CSharp);
        MainTextControlBox.FindDescendant<TextBox>().IsTabStop = false;

        var script = File.ReadAllText("Assets/Scripts/Rewrite.txt");
        MainTextControlBox.LoadText(script);

        var diary = File.ReadAllText("Assets/Scripts/RewriteTarget.txt");
        OriginalTextBox.Text = diary;
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

        ResultTextBox.Text = string.Empty;
        var op = languageModel.GenerateResponseWithProgressAsync(new LanguageModelOptions()
        {
            Skill = LanguageModelSkill.Rewrite
        }, OriginalTextBox.Text);
        op.Progress += (info, delta) => DispatcherQueue.TryEnqueue(() => ResultTextBox.Text += delta);
        await op;

        MainProgressBar.Visibility = Visibility.Collapsed;
        MainWindow.IsEnabled = true;
    }
}
