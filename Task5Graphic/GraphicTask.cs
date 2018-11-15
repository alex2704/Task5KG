using ClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task5Graphic
{
    public partial class GraphicTask : Form
    {
        static Graphics g;
        Bitmap bitmap;
        MouseEventArgs e0;
        double coeff = 1;
        bool drawing = false;
        CoordinatePlane coordinatePlane = new CoordinatePlane();
        public GraphicTask()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox.Width,pictureBox.Height);
            g = Graphics.FromImage(bitmap);
        }
        public void MyDraw(Graphics g)
        {
            coordinatePlane.Draw(g, pictureBox, coordinatePlane.Step);
            pictureBox.Image = bitmap;
        }
        private void GraphicTask_Paint(object sender, PaintEventArgs e)
        {
            MyDraw(g);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            coordinatePlane.y1 -= 0.1;
            coordinatePlane.y2 += 0.1;
            MyDraw(g);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            coordinatePlane.y1 -= 0.1;
            coordinatePlane.y2 -= 0.1;
            MyDraw(g);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int fl = Convert.ToInt32((sender as Button).Tag);
            switch (fl)
            {
                case 0:
                    coordinatePlane.x1 += 0.1; coordinatePlane.x2 += 0.1;
                    break;
                case 1:
                    coordinatePlane.x1 -= 0.1; coordinatePlane.x2 -= 0.1;
                    break;
                case 2:
                    coordinatePlane.y1 += 0.1; coordinatePlane.y2 += 0.1;
                    break;
                case 3:
                    coordinatePlane.y1 -= 0.1; coordinatePlane.y2 -= 0.1;
                    break;
            }
            MyDraw(g);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            coordinatePlane.x1 -= 0.1;
            coordinatePlane.x2 -= 0.1;
            MyDraw(g);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            coordinatePlane.y1 += 0.1;
            coordinatePlane.y2 += 0.1;
            MyDraw(g);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            coordinatePlane.x1 += 0.1;
            coordinatePlane.x2 -= 0.1;
            MyDraw(g);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            coordinatePlane.x1 -= 0.1;
            coordinatePlane.x2 += 0.1;
            MyDraw(g);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            coordinatePlane.y1 += 0.1;
            coordinatePlane.y2 -= 0.1;
            MyDraw(g);
        }

        private void GraphicTask_MouseDown(object sender, MouseEventArgs e)
        {
            drawing = true;
            e0 = e;
        }

        private void GraphicTask_MouseUp(object sender, MouseEventArgs e)
        {
            drawing = false;
        }

        private void GraphicTask_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                double dx = coordinatePlane.XX(e.X) - coordinatePlane.XX(e0.X);
                double dy = coordinatePlane.YY(e.Y) - coordinatePlane.XX(e0.X);
                e0 = e;
                coordinatePlane.x1 -= dx;
                coordinatePlane.y1 -= dy;
                coordinatePlane.x2 -= dx;
                coordinatePlane.y2 -= dy;
                MyDraw(g);
            }
        }

        private void GraphicTask_Load(object sender, EventArgs e)
        {
            MouseWheel += new MouseEventHandler(GraphicTask_MouseWheel);
        }
        private void GraphicTask_MouseWheel(object sender, MouseEventArgs e)
        {
            double x = coordinatePlane.XX(e.X);
            double y = coordinatePlane.YY(e.Y);
            if (e.Delta < 0)
                coeff = 1.03;
            else
                coeff = 0.97;
            coordinatePlane.x1 = x - (x - coordinatePlane.x1) * coeff;
            coordinatePlane.x2 = x + (coordinatePlane.x2 - x) * coeff;
            coordinatePlane.y1 = x - (y - coordinatePlane.y1) * coeff;
            coordinatePlane.y2 = x + (coordinatePlane.y2 - y) * coeff;
            MyDraw(g);
        }

        private void Start_btn_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            coordinatePlane.Step += 1;
            //coordinatePlane.DrawModel(1,0);
            //MyDraw(g);
            // g.DrawCurve(,20+coordinatePlane.Step);
            coordinatePlane.Draw(g, pictureBox, coordinatePlane.Step);
            pictureBox.Image = bitmap;
            pictureBox.Update();
            Invalidate();
        }
        RadioButton GetSelectedRadioButton(GroupBox groupBox)
        {
            foreach (Control control in groupBox.Controls)
            {
                RadioButton radioButton = control as RadioButton;
                if (radioButton != null && radioButton.Checked)
                    return radioButton;
            }
            return null;
        }
    }
}
