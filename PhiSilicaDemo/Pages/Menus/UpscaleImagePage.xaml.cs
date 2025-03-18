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

public sealed partial class UpscaleImagePage : Page
{
    public UpscaleImagePage()
    {
        InitializeComponent();

        MainTextControlBox.SelectSyntaxHighlightingById(SyntaxHighlightID.CSharp);
        MainTextControlBox.FindDescendant<TextBox>().IsTabStop = false;

        var script = File.ReadAllText("Assets/Scripts/UpscaleImage.txt");
        MainTextControlBox.LoadText(script);
    }

    private async void OnRunButtonClicked(object sender, RoutedEventArgs e)
    {
        MainWindow.IsEnabled = false;
        MainProgressBar.Visibility = Visibility.Visible;

        using var imageScaler = await EnsureModelIsReady();
        using var stream = File.OpenRead("Assets/Upscale.jpg");
        var decoder = await BitmapDecoder.CreateAsync(stream.AsRandomAccessStream());
        using var bitmap = await decoder.GetSoftwareBitmapAsync();

        using var upscaled = imageScaler.ScaleSoftwareBitmap(bitmap, bitmap.PixelWidth * 4, bitmap.PixelHeight * 4);
        using var finalImage = SoftwareBitmap.Convert(upscaled, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
        var source = new SoftwareBitmapSource();
        await source.SetBitmapAsync(finalImage);
        UpscaledImage.Source = source;

        MainProgressBar.Visibility = Visibility.Collapsed;
        MainWindow.IsEnabled = true;
    }

    private static async Task<ImageScaler> EnsureModelIsReady()
    {
        if (!ImageScaler.IsAvailable())
        {
            var result = await ImageScaler.MakeAvailableAsync();
            if (result.Status != PackageDeploymentStatus.CompletedSuccess)
            {
                throw result.ExtendedError;
            }
        }

        return await ImageScaler.CreateAsync();
    }
}
