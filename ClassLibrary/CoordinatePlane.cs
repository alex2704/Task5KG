using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassLibrary
{
    public class CoordinatePlane
    {
        public int Step { get; set; } = 0;
        static int I1 = 0, J1 = 0, I2, J2;
        public double x1 = -0.1, y1 = -1, x2 = 3.1, y2 = 16;
        public delegate int IJ(double x);

        public Pen MyPen1;
        public Pen MyPen2;
        Font myFont;

        public CoordinatePlane()
        {
            MyPen1 = new Pen(Brushes.Black, 1);
            MyPen1.Color = Color.Black;
            MyPen2 = new Pen(Brushes.Black, 1);
            MyPen2.Color = Color.Black;
        }

        public int II(double x)//прямой метод масштабирования горизонтальных координат
        {
            return I1 + (int)((x - x1) * (I2 - I1) / (x2 - x1));
        }
        public double XX(int I)//обратный метод масштабирования горизонтальных координат
        {
            return x1 + (I - I1) * (x2 - x1) / (I2 - I1);
        }
        public int JJ(double y)//прямой метод масштабирования вертикальных координат
        {
            return J2 + (int)((y - y1) * (J1 - J2) / (y2 - y1));
        }

        public double YY(int J)//прямой метод масштабирования вертикальных координат
        {
            return y1 + (J - J2) * (y2 - y1) / (J1 - J2);
        }
        private double HH(double a1, double a2)
        {
            double Result = 1;
            while (Math.Abs(a2 - a1) / Result < 1)
                Result /= 10.0;
            while (Math.Abs(a2 - a1) / Result >= 10)
                Result *= 10.0;
            if (Math.Abs(a2 - a1) / Result < 2.0)
                Result /= 5.0;
            if (Math.Abs(a2 - a1) / Result < 5.0)
                Result /= 2.0;
            return Result;
        }
        private byte GetDigits(double dx)
        {
            byte Result;
            if (dx >= 5) Result = 0;
            else
                if (dx >= 0.5) Result = 1;
                else
                    if (dx >= 0.05) Result = 2;
                    else
                        if (dx >= 0.005) Result = 3;
                        else
                            if (dx >= 0.0005) Result = 4;
                                else Result = 5;
            return Result;
        }
        private void OX(IJ II, IJ JJ, Graphics g)
        {
            g.DrawLine(Pens.LightBlue, II(x1), JJ(0), II(x2), JJ(0));
            double h1 = HH(x1,x2);
            int k1 = (int)Math.Round(x1 / h1) - 1;
            int k2 = (int)Math.Round(x2 / h1);
            byte Digits = GetDigits(Math.Abs(x2 - x1));
            for (int i = k1; i <= k2; i++)
            {
                g.DrawLine(MyPen2, II(i * h1), JJ(0) - 7, II(i * h1), JJ(0) + 7);
                for (int j = 1; j <= 9; j++)
                    g.DrawLine(MyPen2, II(i * h1 + j * h1 / 10), JJ(0) - 3, II(i * h1 + j * h1 / 10), JJ(0) + 3);
                string s = Convert.ToString(Math.Round(h1 * i, Digits));
                g.DrawString(s, myFont, Brushes.Black, II(i * h1) - 5, JJ(0) - 13);
            }
        }
        private void OY(IJ II, IJ JJ, Graphics g)
        {
            g.DrawLine(Pens.LightBlue, II(0), JJ(y1), II(0), JJ(y2));
            double h1 = HH(y1, y2); int k1 = (int)Math.Round(y1 / h1) - 1;
            int k2 = (int)Math.Round(y2 / h1);
            int Digits = GetDigits(Math.Abs(y2 - y1));
            for (int i = k1; i <= k2; i++)
            {
                g.DrawLine(MyPen2, II(0) - 7, JJ(i * h1), II(0) + 7, JJ(i * h1));
                for (int j = 1; j <= 9; j++)
                    g.DrawLine(MyPen2, II(0) - 3, JJ(i * h1 + j * h1 / 10), II(0) + 3, JJ(i * h1 + j * h1 / 10));
                string s = Convert.ToString(Math.Round(h1 * i, Digits));
                g.DrawString(s, myFont, Brushes.Black, II(0) + 5, JJ(i * h1) - 5);
            }
        }
        public void Draw(Graphics g, PictureBox pictureBox, int step)
        {
            I2 = pictureBox.Width;
            J2 = pictureBox.Height;
            try
            {
                Color c1 = Color.FromArgb(255, 255, 255);
                g.Clear(c1);
                //Оси OX и OY
                myFont = new Font("Arial", 7, FontStyle.Bold);
                OX(II,JJ,g);
                OY(II,JJ,g);
                PointF p1 = new PointF(II(0), JJ(0));
                PointF p2 = new PointF(II(5), JJ(10));
                PointF p3 = new PointF(II(10 ), JJ(5));
                PointF p4 = new PointF(II(20 + step), JJ(20 + step));
                PointF[] points = { p1, p2, p3, p4 };
                g.DrawCurve(Pens.Black, points);
                myFont.Dispose();
            }
            finally
            {
            }
        }
        public double FuncModel(int v, double x)
        {
            switch (v)
            {
                case 1:
                    return Math.Pow(x,2);
                case 2:
                    return Math.Pow(x,3);
                case 3:
                    return Math.Pow(x,1/2);
                default:
                    return 0;
            }
        }
        public void DrawModel(int v, double x)
        {
            PointF p1, p2 = new PointF();
            double y = FuncModel(v, x + Step);
            p1 = new PointF(II(x + Step),JJ(y));
            p2 = new PointF(II(x + Step + 1), JJ(FuncModel(v, x + Step + 1)));
        }
      
    }
}
