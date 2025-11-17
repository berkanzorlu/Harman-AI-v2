using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace HarmanAI.Desktop.Services;

public class ScreenAnalyzer
{
    public byte[] CapturePrimaryScreen()
    {
        var bounds = System.Windows.Forms.Screen.PrimaryScreen!.Bounds;
        using var bmp = new Bitmap(bounds.Width, bounds.Height);
        using var g = Graphics.FromImage(bmp);
        g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
        using var ms = new MemoryStream();
        bmp.Save(ms, ImageFormat.Png);
        return ms.ToArray();
    }
}
