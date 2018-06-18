using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace MineSweeperBot
{
    class SSC //ScreenShotClass
    {
        public SSC()
        {

        }
        public void TakeSS()
        {
            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                           Screen.PrimaryScreen.Bounds.Height,
                                           PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                        Screen.PrimaryScreen.Bounds.Y,
                                        0,
                                        0,
                                        Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);

            // Save the screenshot to the specified path that the user has chosen.
            bmpScreenshot.Save("Screenshot.png", ImageFormat.Png);
        }
        public void TakeSS(Point startPoint, Point endPoint)
        {
            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(endPoint.X - startPoint.X,
                                           endPoint.Y - startPoint.Y,
                                           PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(startPoint,new Point (0,0),Screen.PrimaryScreen.Bounds.Size);

            // Save the screenshot to the specified path that the user has chosen.
            bmpScreenshot.Save("ScreenshotCropped.png", ImageFormat.Png);
        }

        public Bitmap TakeBitmap(Point startPoint, Point endPoint)
        {
            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(endPoint.X - startPoint.X,
                                           endPoint.Y - startPoint.Y,
                                           PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(startPoint, new Point(0, 0), Screen.PrimaryScreen.Bounds.Size);
            bmpScreenshot.Save("SHOT.png", ImageFormat.Png);

            // Save the screenshot to the specified path that the user has chosen.
            return bmpScreenshot;
        }

        public void TakeASnap(int xOff, int yOff, Size s)
        {
            var bmpScreenshot = new Bitmap(s.Width,s.Height,PixelFormat.Format32bppArgb);
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            gfxScreenshot.CopyFromScreen(xOff, yOff, 0, 0, s, CopyPixelOperation.SourceCopy);
            bmpScreenshot.Save("Screenshot.png", ImageFormat.Png);
        }
        public double[] BoxDim(Point upper, Point lower, int xSquares, int ySquares)
        {
            double[] bb = new double[2];
            double BoxWdth = Convert.ToDouble(lower.X - upper.X) / xSquares;
            double BoxHeight = Convert.ToDouble(lower.Y - upper.Y) / ySquares;
            bb[0] = BoxWdth;
            bb[1] = BoxHeight;
            return bb;
        }

        public static void TakeASnap(Bitmap bm, string name)
        {
            bm.Save("Images/" + name + ".png", ImageFormat.Png);
        }
    }
}
