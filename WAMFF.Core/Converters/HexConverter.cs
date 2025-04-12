using Microsoft.UI.Xaml.Media;
using System.Globalization;
using Windows.UI;

namespace WAMFF.Core.Converters;

public static class HexConverter
{
    public static Color FromHex(this string hex) {
        try {
            hex = hex.Replace("#", "");
            byte a = 255, r = 0, g = 0, b = 0;

            if (hex.Length == 6) // RGB
            {
                r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            }
            else if (hex.Length == 8) // ARGB
            {
                a = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                r = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                g = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                b = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
            }

            return Color.FromArgb(a, r, g, b);
        }
        catch {
            return Color.FromArgb(0, 0, 0, 0);
        }
    }

    public static SolidColorBrush ConvertHex(string hex) {
        try {
            hex = hex.Replace("#", "");
            byte a = 255, r = 0, g = 0, b = 0;

            if (hex.Length == 6) // RGB
            {
                r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            }
            else if (hex.Length == 8) // ARGB
            {
                a = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                r = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                g = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                b = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
            }

            return new(Windows.UI.Color.FromArgb(a, r, g, b));
        }
        catch {
            return new(Windows.UI.Color.FromArgb(0, 0, 0, 0));
        }
    }
}