using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SorterSpheroids
{
    public partial class MainForm : Form
    {
        AutoForm auto_form;
        ManualForm manual_form;
        CameraForm camera_form;
        Point[] ps_loc = new Point[2];
        public MainForm()
        {
            InitializeComponent();
            ps_loc = new Point[] { new Point(10,40), new Point(1970, 120), new Point(1970, 120), };//camera, manual, auto
            manual_form = new ManualForm(this);
            auto_form = new AutoForm(this);
            camera_form = new CameraForm(this);
            Console.WriteLine("constr: " + this.Location.X + " " + this.Location.Y);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            camera_form.Location = add_p(ps_loc[0], this.Location);
            camera_form.Show(this);

            manual_form.Location = add_p(ps_loc[1],this.Location);
            manual_form.Show(this);

            auto_form.Location = add_p(ps_loc[2], this.Location);
            // auto_form.Show(this);
           // Console.WriteLine("load: " + this.Location.X + " " + this.Location.Y);
            this.Refresh();
            Application.DoEvents();
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            if(manual_form == null || camera_form == null) return;
            camera_form.Location = add_p(ps_loc[0], this.Location);
            manual_form.Location = add_p(ps_loc[1], this.Location);
            auto_form.Location = add_p(ps_loc[2], this.Location);
           //Console.WriteLine("move: "+this.Location.X + " " + this.Location.Y);
        }

        Point add_p(Point p1,Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        private void but_manual_Click(object sender, EventArgs e)
        {
            manual_form.Visible = false;
            manual_form.Show(this);
            auto_form.Hide();
           
        }

        private void but_auto_Click(object sender, EventArgs e)
        {
            auto_form.Visible = false;
            auto_form.Show(this);
            manual_form.Hide();
           
        }

        public static double to_double_textbox(TextBox textBox, double min, double max)
        {
            var val = to_double(textBox.Text);
            if (val == double.NaN)
            {
                val = min;
            }
            if (val < min)
            {
                val = min;
            }
            if (val > max)
            {
                val = max;
            }
            return val;
        }

        public static double to_double(string val)
        {
            if (val == null) return 0;
            if (val.Length == 0) return 0;
            val = val.Replace(',', '.');
            try
            {
                return Convert.ToDouble(val);
            }
            catch
            {
                return double.NaN;
            }
            //return 
        }

    }
}
