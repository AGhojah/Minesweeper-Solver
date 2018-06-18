using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MineSweeperBot
{
    public static class BlockAnalyzer
    {

        public static int Analyze()
        {

            return -1;
        }

        /// <summary>
        /// Returns the actual number on the bitmap box
        /// </summary>
        /// <param name="bm">The bitmap image of a single square</param>
        /// <returns></returns>
        public static int Analyze(Bitmap bm,string nameeOfImage)
        {
            SSC.TakeASnap(bm, nameeOfImage);
            int w = bm.Size.Width;
            int h = bm.Size.Height;

            int orgW = w;
            w = w - 4;      // width itterater

            if (PixelInt.B(bm, w / 3, h / 3) == 250 && PixelInt.B(bm, w / 4, h / 4) == 250 && PixelInt.B(bm, (2*w) / 5, h / 4) == 250)
                return 0;

            for (; w <= orgW + 4; w+= 4)
                for (int x = 14; x < h-10; x += 7)
                {
                    if (PixelInt.B(bm, w / 2, x) > 180 && PixelInt.R(bm, w / 2, x) > 175 && PixelInt.G(bm, w / 2, x) > 180)
                        continue;
                    else if ((int)bm.GetPixel(w / 2, x).B > 180 && (int)bm.GetPixel(w / 2, x).R < 90 && (int)bm.GetPixel(w / 2, x).G < 100)
                    {
                        if ((int)bm.GetPixel(orgW / 3, h / 2).R < 170 && PixelInt.G(bm, orgW / 3, h / 2) < 180) //&& PixelInt.B(bm, orgW / 3, h/2) < 210)
                            return -8;          //Blue unopened square
                        else
                            return 1;
                    }
                    else if ((int)bm.GetPixel(w / 2, x).B < 20 && (int)bm.GetPixel(w / 2, x).R < 50 && (int)bm.GetPixel(w / 2, x).G > 100)
                        return 2;
                    else if ((int)bm.GetPixel(w / 2, x).B < 20 && (int)bm.GetPixel(w / 2, x).R > 150 && (int)bm.GetPixel(w / 2, x).G < 20)
                    {
                        if ((int)bm.GetPixel(w / 2 - 4, h / 2).B < 10 && (int)bm.GetPixel(w / 2 - 4, h / 2).R > 150 && (int)bm.GetPixel(w / 2 - 4, h / 2).G < 10)
                            return 3;
                        else
                            return 7;
                    }
                    else if ((int)bm.GetPixel(w / 2, x).B > 120 && (int)bm.GetPixel(w / 2, x).B < 140 && (int)bm.GetPixel(w / 2, x).R < 10 && (int)bm.GetPixel(w / 2, x).G < 10)
                        return 4;
                    else if ((int)bm.GetPixel(w / 2, x).B < 10 && (int)bm.GetPixel(w / 2, x).R > 110 && (int)bm.GetPixel(w / 2, x).R < 140 && (int)bm.GetPixel(w / 2, x).G < 10)
                        return 5;
                    else if ((int)bm.GetPixel(w / 2, x).B > 110 && (int)bm.GetPixel(w / 2, x).R < 10 && (int)bm.GetPixel(w / 2, x).G > 110)
                        return 6;
                    else if ((int)bm.GetPixel(orgW / 3, x).B > 220 && (int)bm.GetPixel(orgW / 3, x).R < 200)
                        return -8;          //Blue unopened square
                }

            return 0;
        }

        public static int[,] ReturnBlocksStatus(Bitmap bm, int MineRows, int MineColumns)
        {
            double boxWidth = bm.Width/(double)MineColumns;
            double boxHeight = bm.Height / (double)MineRows;
            int counter = 0;

            int[,] ar = new int[MineRows, MineColumns];

            for (int rows = 0; rows < MineRows; rows++)
            {
                for (int cols = 0; cols < MineColumns; cols++)
                {
                    ar[rows, cols] = Analyze(bm.Clone(new Rectangle((int)(cols * boxWidth), (int)(rows * boxHeight), (int)boxWidth, (int)boxHeight), bm.PixelFormat), "00" + counter++.ToString());
                }
            }
            return ar;
        }

        private static class PixelInt
        {
            public static int B(Bitmap bm, int w, int h)
            {
                return (int)bm.GetPixel(w, h).B;
            }
            public static int G(Bitmap bm, int w, int h)
            {
                return (int)bm.GetPixel(w, h).G;
            }
            public static int R(Bitmap bm, int w, int h)
            {
                return (int)bm.GetPixel(w, h).R;
            }

        }
    }
}
