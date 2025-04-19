using Connection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SorterSpheroids
{
    
    public partial class AutoForm : Form
    {
        MainForm mainForm;
        public AutoForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            gen_buts_cells();
        }
        void gen_buts_cells()
        {
            var ps = new List<Point>();
            var ps_st = new Point(10,30);
            var dx = 80;
            var dy = 80;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    gen_button_cells(new Point(ps_st.X + dx * i, ps_st.Y + dy * j),i+" "+j);
                }
            }

            groupBox2.SendToBack();
        }
        Button gen_button_cells(Point point, string acc_name)
        {
            var but1 = new CircularButton(new Size(60,60));
            but1.Location = point;
            but1.Text = "";
            but1.AccessibleName = acc_name;
            but1.Click += but_choose_cell_Click;
            groupBox2.Controls.Add(but1);

            return but1;
        }

        private void but_scan_cell_Click(object sender, EventArgs e)
        {
            var p_beg = mainForm.get_cur_pos();
            var p_cur = p_beg.Clone();

            var vel_xy = 0.5;
            var delt_time = 2000;
            var dist_x = 3;
            var dist_y = 3;
            var dx = 0.3;
            var poses = new List<GFrame>();
            for (double x = 0; p_cur.x - p_beg.x < dist_x;)
            {
                p_cur.y += dist_y;
                poses.Add(p_cur.Clone());
                p_cur.x += dx; 
                poses.Add(p_cur.Clone());
                p_cur.y -= dist_y;
                poses.Add(p_cur.Clone());
                p_cur.x += dx;
                poses.Add(p_cur.Clone());
            }
            mainForm.scan_thread(poses.ToArray(), vel_xy, delt_time);
        }

        private void but_choose_cell_Click(object sender, EventArgs e)
        {
            var but = (Button)sender;
            Console.WriteLine(but.AccessibleName);
        }
    }

    public class CircularButton : Button
    {
        private bool isMouseOver = false;

        public CircularButton(Size size)
        {
            // Set default size
            this.Size = size;
            // Make the button flat
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;

            // Subscribe to mouse events
            this.MouseEnter += CircularButton_MouseEnter;
            this.MouseLeave += CircularButton_MouseLeave;

            // Set double buffering to reduce flickering
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            // Clear the background
            pevent.Graphics.Clear(this.BackColor);

            // Create circular path
            GraphicsPath graphicsPath = new GraphicsPath();
            Rectangle bounds = new Rectangle(0,0, this.Width - 1, this.Height - 1 );
            graphicsPath.AddEllipse(bounds);
            this.Region = new Region(graphicsPath);

            // Draw the button background
            Pen pen1 = new Pen(Color.Black, 2);
            using (SolidBrush brush = new SolidBrush(Color.AliceBlue))
            {
                pevent.Graphics.FillEllipse(brush, bounds);
            }

            // Draw text
            TextRenderer.DrawText(pevent.Graphics, this.Text, this.Font,
                bounds, this.ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            // Draw black border when mouse is over
            if (isMouseOver)
            {
                using (Pen pen = new Pen(Color.Blue, 4))
                {
                    pen.LineJoin = LineJoin.Round;
                    pevent.Graphics.DrawEllipse(pen, bounds);
                }
            }


           // base.OnPaint(pevent);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.Width != this.Height)
            {
                this.Height = this.Width;
            }
            this.Invalidate(); // Redraw on resize
        }

        private void CircularButton_MouseEnter(object sender, EventArgs e)
        {
            isMouseOver = true;
            this.Invalidate();
        }

        private void CircularButton_MouseLeave(object sender, EventArgs e)
        {
            isMouseOver = false;
            this.Invalidate();
        }
    }
}
