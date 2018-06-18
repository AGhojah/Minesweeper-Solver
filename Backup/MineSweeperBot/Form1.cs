using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Threading;

namespace MineSweeperBot
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("user32.dll")]
        static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);
        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        Random rnd = new Random();
        SSC ssc = new SSC();
        Thread xxt;
        bool on = false;
        int gg = 0,xMine = 30,yMine = 16;
        Point pntr1, pntr2;

        public Form1()
        {
            InitializeComponent();
            TransparencyKey = Color.BlueViolet;
            this.BackColor = Color.BlueViolet;
            this.TopMost = true;
            textBox3.Text = yMine.ToString();
            textBox4.Text = xMine.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!on)
            {
                xxt = new Thread(worker);
                //this.Hide();
                on = true;
                xxt.Start();
            }
            else
            {
                on = false;
                xxt.Abort();
            }
        }
        public void worker()
        {
            int xx,yy,cc = 0,dd=0;
            Color clor = new Color();

            while (on)
            {
                //cc++;
                xx = Cursor.Position.X;
                yy = Cursor.Position.Y;
                clor = GetPixelColor(xx, yy);
                label1.Invoke((MethodInvoker)(() => label1.Text = clor.ToString()));
                label2.Invoke((MethodInvoker)(() => label2.Text = xx.ToString() + ", "+ yy.ToString()));
                /*if (cc >= 100)
                {
                    DoMouseClick();
                    cc = 0;
                    dd++;
                    label2.Invoke((MethodInvoker)(() => label2.Text = "click"+dd));
                }*/

            }
        }

        private Color GetPixelColor(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                         (int)(pixel & 0x0000FF00) >> 8,
                         (int)(pixel & 0x00FF0000) >> 16);
            return color;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (on)
                xxt.Abort();
            on = false;
        }

        public void DoMouseClick()
        {
            //Call the imported function with the cursor's current position
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)X, (uint)Y, 0, 0);
        }
        public void DoRightClick()
        {
            //Call the imported function with the cursor's current position
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, (uint)X, (uint)Y, 0, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //ssc.TakeASnap(300, 300, new Size(300, 300));
            if (!(gg > 0))
            {
                MouseHook.Start();
                MouseHook.MouseAction += new EventHandler(Event);
            }
        }


        private void Event(object sender, EventArgs e) 
        {
            gg++;
            //label1.Text = "Left mouse click!";
            if (gg == 1)
            {
                pntr1 = Cursor.Position;
                textBox1.Text = Cursor.Position.ToString();
            }
            if (gg == 2)
            {
                pntr2 = Cursor.Position;
                textBox2.Text = Cursor.Position.ToString();
            }
            if (gg > 2)
            {
                MouseHook.MouseAction -= new EventHandler(Event);
                MouseHook.stop();
                gg = 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            xMine = Convert.ToInt32(textBox4.Text);
            yMine = Convert.ToInt32(textBox3.Text);
            int xadd = 0, yadd = 0;
            double[] ccc = ssc.BoxDim(pntr1, pntr2, xMine, yMine);
            for (int yctr = 0; yctr < yMine; yctr++)
            {
                for (int cntr = 0; cntr < xMine; cntr++)
                {
                    xadd = Convert.ToInt32((ccc[0] / 2) + ccc[0] * cntr);
                    yadd = Convert.ToInt32((ccc[1] / 2) + ccc[1] * yctr);
                    Cursor.Position = new Point((pntr1.X + xadd), (pntr1.Y + yadd));
                    DoRightClick();
                    Thread.Sleep(20);
                    
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            xMine = Convert.ToInt32(textBox4.Text);
            yMine = Convert.ToInt32(textBox3.Text);
            MessageBox.Show(xMine.ToString() + " " + yMine.ToString());
            double[] ccc = ssc.BoxDim(pntr1, pntr2, xMine, yMine);
            MessageBox.Show(ccc[0].ToString() +" "+ ccc[1].ToString());
            /*double wewe = 0;
            for (int f = 0; f < 1280; f++)
            {
                wewe++;
                //Cursor.Position = new Point(rnd.Next(0, 1200), rnd.Next(0, 800));
                //Thread.Sleep(200);
                Cursor.Position = new Point(340 + Convert.ToInt16(200 * Math.Cos(wewe / 100)), 400 + Convert.ToInt16(200 * Math.Sin(wewe / 100)));
                Thread.Sleep(10);
            }*/
        }
    }
}




/*
namespace TriggerBotTest
{
    public partial class Form1 : Form
    {        
        Point pp;

        public Form1()
        {
            //this.FormBorderStyle = FormBorderStyle.None;
            //this.Bounds = Screen.PrimaryScreen.Bounds;

            pp = new Point();
            pp.X = Screen.PrimaryScreen.WorkingArea.Width / 2;
            pp.Y = Screen.PrimaryScreen.WorkingArea.Height / 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread xx = new Thread(worker);
            this.Hide();
            xx.Start();
        }

        public void worker()
        {
            Color cr = new Color();
            cr = Color.FromArgb(0,0,0);
            Color nextCr = new Color();
            nextCr = Color.FromArgb(2, 4, 7);
            while (cr != nextCr)
            {
                Thread.Sleep(10);
                nextCr = GetPixelColor(pp.X,pp.Y);
            }
            Thread.Sleep(1000);
            DoMouseClick();
            //button1.Invoke((MethodInvoker)(() => button1.PerformClick()));
            this.Invoke((MethodInvoker)(() => this.Close()));
        }
        public void DoMouseClick()
        {
            //Call the imported function with the cursor's current position
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)X, (uint)Y, 0, 0);
        }

    }
}

*/