using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.AI.Generative;
using Microsoft.Windows.Management.Deployment;
using Microsoft.Windows.Vision;
using System;
using System.IO;
using System.Threading.Tasks;
using TextControlBoxNS;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.Storage;
using Microsoft.Graphics.Imaging;
using System.Text;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Media.Imaging;

namespace PhiSilicaDemo.Pages.Menus;

public sealed partial class DescribeImagePage : Page
{
    public DescribeImagePage()
    {
        InitializeComponent();

        MainTextControlBox.SelectSyntaxHighlightingById(SyntaxHighlightID.CSharp);
        MainTextControlBox.FindDescendant<TextBox>().IsTabStop = false;

        var script = File.ReadAllText("Assets/Scripts/DescribeImage.txt");
        MainTextControlBox.LoadText(script);
    }

    private async void OnRunButtonClicked(object sender, RoutedEventArgs e)
    {
        MainWindow.IsEnabled = false;
        MainProgressBar.Visibility = Visibility.Visible;

        using var imageDescriptionGenerator = await EnsureModelIsReady();
        using var stream = File.OpenRead("Assets/Describe.png");
        var decoder = await BitmapDecoder.CreateAsync(stream.AsRandomAccessStream());
        using var bitmap = await decoder.GetSoftwareBitmapAsync();

        var inputImage = ImageBuffer.CreateCopyFromBitmap(bitmap);
        var result = await imageDescriptionGenerator.DescribeAsync(inputImage, ImageDescriptionScenario.Caption);
        ResultTextBox.Text = result.Response;

        MainProgressBar.Visibility = Visibility.Collapsed;
        MainWindow.IsEnabled = true;
    }

    private static async Task<ImageDescriptionGenerator> EnsureModelIsReady()
    {
        if (!ImageDescriptionGenerator.IsAvailable())
        {
            var result = await ImageDescriptionGenerator.MakeAvailableAsync();
            if (result.Status != PackageDeploymentStatus.CompletedSuccess)
            {
                throw result.ExtendedError;
            }
        }

        return await ImageDescriptionGenerator.CreateAsync();
    }
}
