using Microsoft.UI.Xaml.Media.Imaging;
using System.Drawing;

namespace WAMFF.Core.Utilities;

public static class IconExtensions
{
    public static BitmapImage ToBitMapImage(this Icon icon) {
        Bitmap bmp = icon.ToBitmap();
        BitmapImage bitmapImage = new();
        using MemoryStream stream = new();
        bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        stream.Position = 0;
        bitmapImage.SetSource(stream.AsRandomAccessStream());
        return bitmapImage;
    }
}