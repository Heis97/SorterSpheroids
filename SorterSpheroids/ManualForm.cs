using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Connection;

namespace SorterSpheroids
{
    public partial class ManualForm : Form
    {
        MainForm mainForm;
        DeviceMarlin Sorter;
        double dist_xy = 1;
        double dist_z = 1;
        double dist_e = 1;
        double vel_xy = 50;
        double vel_z = 50;
        double vel_e = 600;
        public ManualForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

      

        private void but_ard_con_Click(object sender, EventArgs e)
        {
            var port =(string) comboBox_ports.SelectedItem;
            Sorter = new DeviceMarlin(port);
        }

        private void but_find_ports_Click(object sender, EventArgs e)
        {
            DeviceArduino.find_ports(comboBox_ports);
            Sorter.sendCommand("G91");
            Sorter.sendCommand("GM302 S0");
        }

        private void but_home_XYZA_Click(object sender, EventArgs e)
        {
            Sorter.sendCommand("G28 ZA");
            Sorter.sendCommand("G28 XY");
        }

        private void but_home_X_Click(object sender, EventArgs e)
        {
            Sorter.sendCommand("G28 X");
        }

        private void but_home_Y_Click(object sender, EventArgs e)
        {
            Sorter.sendCommand("G28 Y");
        }

        private void but_home_Z_Click(object sender, EventArgs e)
        {
            Sorter.sendCommand("G28 Z");
        }

        private void but_home_A_Click(object sender, EventArgs e)
        {
            Sorter.sendCommand("G28 A");
        }

        private void but_y_pos_Click(object sender, EventArgs e)
        {
            Sorter.sendCommand("G1",new string[] {"Y", "F" }, new object[] { dist_xy , vel_xy});
        }

        private void but_x_pos_Click(object sender, EventArgs e)
        {
            Sorter.sendCommand("G1", new string[] { "X", "F" }, new object[] { dist_xy, vel_xy });
        }

        private void but_y_neg_Click(object sender, EventArgs e)
        {
            Sorter.sendCommand("G1", new string[] { "Y", "F" }, new object[] { -dist_xy, vel_xy });
        }

        private void but_x_neg_Click(object sender, EventArgs e)
        {
            Sorter.sendCommand("G1", new string[] { "Y", "F" }, new object[] { dist_xy, vel_xy });
        }

        private void but_zd_pos_Click(object sender, EventArgs e)
        {
            Sorter.sendCommand("G1", new string[] { "Z", "F" }, new object[] { dist_z, vel_z });
        }

        private void but_zd_neg_Click(object sender, EventArgs e)
        {
            Sorter.sendCommand("G1", new string[] { "Z", "F" }, new object[] { -dist_z, vel_z });
        }

        private void but_zm_pos_Click(object sender, EventArgs e)
        {
            Sorter.sendCommand("G1", new string[] { "A", "F" }, new object[] { dist_z, vel_z });
        }

        private void but_zm_neg_Click(object sender, EventArgs e)
        {
            Sorter.sendCommand("G1", new string[] { "A", "F" }, new object[] { -dist_z, vel_z });
        }

        private void but_zdm_pos_Click(object sender, EventArgs e)
        {
            Sorter.sendCommand("G1", new string[] { "Z", "A", "F" }, new object[] { dist_z, dist_z, vel_z });
        }

        private void but_zdm_neg_Click(object sender, EventArgs e)
        {
            Sorter.sendCommand("G1", new string[] { "Z", "A", "F" }, new object[] { -dist_z, -dist_z, vel_z });
        }

        private void but_e_neg_Click(object sender, EventArgs e)
        {
            Sorter.sendCommand("G1", new string[] { "E", "F" }, new object[] { dist_e, vel_e });
        }

        private void but_e_pos_Click(object sender, EventArgs e)
        {
            Sorter.sendCommand("G1", new string[] { "E", "F" }, new object[] { -dist_e, vel_e });
        }

        private void rb_z_mm_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton checkBox = (RadioButton)sender;
            if (checkBox.Checked == true)
            {
                dist_z = Convert.ToDouble(checkBox.AccessibleName);
            }
        }

        private void rb_e_mm_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton checkBox = (RadioButton)sender;
            if (checkBox.Checked == true)
            {
                dist_e = Convert.ToDouble(checkBox.AccessibleName);
            }
        }

        private void rb_xy_mm_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton checkBox = (RadioButton)sender;
            if (checkBox.Checked == true)
            {
                dist_xy = Convert.ToDouble(checkBox.AccessibleName);
            }
        }

        double to_double_textbox(TextBox textBox, double min, double max)
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

        double to_double(string val)
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

        private void textBox_xy_vel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                vel_xy = to_double_textbox(textBox_xy_vel, 0.1, 8);
            }
        }

        private void textBox_z_vel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                vel_z = to_double_textbox(textBox_z_vel, 0.1, 8);
            }
        }

        private void textBox_e_vel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                vel_e = to_double_textbox(textBox_e_vel, 0.1, 8);
            }
        }
    }
}
