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

namespace PhiSilicaDemo.Pages.Menus;

public sealed partial class TextRecognitionPage : Page
{
    public TextRecognitionPage()
    {
        InitializeComponent();

        MainTextControlBox.SelectSyntaxHighlightingById(SyntaxHighlightID.CSharp);
        MainTextControlBox.FindDescendant<TextBox>().IsTabStop = false;

        var script = File.ReadAllText("Assets/Scripts/TextRecognization.txt");
        MainTextControlBox.LoadText(script);
    }

    private async void OnRunButtonClicked(object sender, RoutedEventArgs e)
    {
        MainWindow.IsEnabled = false;
        MainProgressBar.Visibility = Visibility.Visible;

        using var textRecognizer = await EnsureModelIsReady();
        using var stream = File.OpenRead("Assets/OCR2.png");
        var decoder = await BitmapDecoder.CreateAsync(stream.AsRandomAccessStream());
        using var bitmap = await decoder.GetSoftwareBitmapAsync();

        var text = await RecognizeTextFromSoftwareBitmap(textRecognizer, bitmap, PolygonGrid);
        ResultTextBox.Text = text;

        MainProgressBar.Visibility = Visibility.Collapsed;
        MainWindow.IsEnabled = true;
    }

    public static async Task<string> RecognizeTextFromSoftwareBitmap(TextRecognizer textRecognizer, SoftwareBitmap bitmap, Grid grid)
    {
        var imageBuffer = ImageBuffer.CreateBufferAttachedToBitmap(bitmap);
        var result = await textRecognizer.RecognizeTextFromImageAsync(imageBuffer, new());
        var stringBuilder = new StringBuilder();

        var greenBrush = new SolidColorBrush(Microsoft.UI.Colors.Green);
        var yellowBrush = new SolidColorBrush(Microsoft.UI.Colors.Yellow);
        var redBrush = new SolidColorBrush(Microsoft.UI.Colors.Red);

        grid.Children.Clear();
        foreach (var line in result.Lines)
        {
            stringBuilder.AppendLine(line.Text);

            foreach (var word in line.Words)
            {
                var points = new PointCollection();
                var bounds = word.BoundingBox;
                points.Add(bounds.TopLeft);
                points.Add(bounds.TopRight);
                points.Add(bounds.BottomRight);
                points.Add(bounds.BottomLeft);

                var polygon = new Polygon
                {
                    Points = points,
                    StrokeThickness = 2
                };

                if (word.Confidence < 0.33) polygon.Stroke = redBrush;
                else if (word.Confidence < 0.67) polygon.Stroke = yellowBrush;
                else polygon.Stroke = greenBrush;

                grid.Children.Add(polygon);
            }
        }

        return stringBuilder.ToString();
    }

    private static async Task<TextRecognizer> EnsureModelIsReady()
    {
        if (!TextRecognizer.IsAvailable())
        {
            var loadResult = await TextRecognizer.MakeAvailableAsync();
            if (loadResult.Status != PackageDeploymentStatus.CompletedSuccess)
            {
                throw new Exception(loadResult.ExtendedError.Message);
            }
        }

        return await TextRecognizer.CreateAsync();
    }
}
