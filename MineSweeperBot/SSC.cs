using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

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

        public void DrawImage()
        {
            Random rnd = new Random(DateTime.Now.Second);
            int w = 100;
            int h = 100;
            float penThickness = 20f;
            Brush br;
            Font myFont = new Font(FontFamily.GenericSansSerif,12f,FontStyle.Bold);
            br = new SolidBrush(Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255)));
            Pen myPen = new Pen(br, penThickness); 
            PointF txtPoint = new PointF(20f,20f);

            var bmp = new Bitmap(w,h,PixelFormat.Format32bppArgb);
            var gfx = Graphics.FromImage(bmp);
            gfx.DrawRectangle(myPen, 0, 0, w, h);
            gfx.DrawString("hello", myFont, br, txtPoint,StringFormat.GenericTypographic);
            bmp.Save("Image.bmp", ImageFormat.Bmp);

        }

        public void DrawLayout(Point UpperLeft, Point BottomRight, int numOfCol, int numOfRaws)
        {
            Brush b = new SolidBrush(Color.Black);
            int margin = 20;
            Font myFont = new Font(FontFamily.GenericSansSerif, 18f, FontStyle.Bold);

            float xInc = (float)(BottomRight.X - UpperLeft.X) / numOfCol;
            float firstX = xInc / 2;

            float yInc = (float)(BottomRight.Y - UpperLeft.Y) / numOfRaws;
            float firstY = yInc / 2;

            var bmp = new Bitmap((BottomRight.X - UpperLeft.X) + margin, (BottomRight.Y - UpperLeft.Y) + margin, PixelFormat.Format32bppArgb);
            var gfx = Graphics.FromImage(bmp);

            gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            for (int x = 1; x <= numOfCol; x++)
            {
                gfx.DrawString(x.ToString(), myFont, b, new PointF((xInc*(x-1) + firstX + margin), 0f), StringFormat.GenericTypographic);
            }

            gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
            for (int y = 1; y <= numOfRaws; y++)
            {
                gfx.DrawString(y.ToString(), myFont, b, new PointF(0f, (yInc * (y - 1) + firstY + margin)), StringFormat.GenericTypographic);
            }

            bmp.Save("Layout.png", ImageFormat.Png);

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
