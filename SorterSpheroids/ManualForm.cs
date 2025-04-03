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
       
        public ManualForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void but_ard_con_Click(object sender, EventArgs e)
        {
            var port = (string) comboBox_ports.SelectedItem;
            Sorter = new DeviceMarlin(port);
            Sorter?.sendCommand("G91");
            Sorter?.sendCommand("M302 S0");
            Sorter?.sendCommand("M154 S50");
            timer_cur_pos.Enabled = true;
        }

        private void but_find_ports_Click(object sender, EventArgs e)
        {
            DeviceArduino.find_ports(comboBox_ports);
            
        }

        #region position control
        private void but_home_XYZA_Click(object sender, EventArgs e)
        {
            Sorter?.sendCommand("G28 ZA");
            Sorter?.sendCommand("G28 XY");
        }

        private void but_home_X_Click(object sender, EventArgs e)
        {
            Sorter?.sendCommand("G28 X");
        }

        private void but_home_Y_Click(object sender, EventArgs e)
        {
            Sorter?.sendCommand("G28 Y");
        }

        private void but_home_Z_Click(object sender, EventArgs e)
        {
            Sorter?.sendCommand("G28 Z");
        }

        private void but_home_A_Click(object sender, EventArgs e)
        {
            Sorter?.sendCommand("G28 A");
        }

        private void but_y_pos_Click(object sender, EventArgs e)
        {
            Sorter?.sendCommand("G1", new string[] { "Y", "F" }, new object[] { dist_xy, vel_xy });
        }

        private void but_x_pos_Click(object sender, EventArgs e)
        {
            Sorter?.sendCommand("G1", new string[] { "X", "F" }, new object[] { dist_xy, vel_xy });
        }

        private void but_y_neg_Click(object sender, EventArgs e)
        {
            Sorter?.sendCommand("G1", new string[] { "Y", "F" }, new object[] { -dist_xy, vel_xy });
        }

        private void but_x_neg_Click(object sender, EventArgs e)
        {
            Sorter?.sendCommand("G1", new string[] { "X", "F" }, new object[] { -dist_xy, vel_xy });
        }

        private void but_zd_pos_Click(object sender, EventArgs e)
        {
            Sorter?.sendCommand("G1", new string[] { "Z", "F" }, new object[] { dist_z, vel_z });
        }

        private void but_zd_neg_Click(object sender, EventArgs e)
        {
            Sorter?.sendCommand("G1", new string[] { "Z", "F" }, new object[] { -dist_z, vel_z });
        }

        private void but_zm_pos_Click(object sender, EventArgs e)
        {
            Sorter? .sendCommand("G1", new string[] { "A", "F" }, new object[] { dist_z, vel_z });
        }

        private void but_zm_neg_Click(object sender, EventArgs e)
        {
            Sorter?.sendCommand("G1", new string[] { "A", "F" }, new object[] { -dist_z, vel_z });
        }

        private void but_zdm_pos_Click(object sender, EventArgs e)
        {
            Sorter? .sendCommand("G1", new string[] { "Z", "A", "F" }, new object[] { dist_z, dist_z, vel_z });
        }

        private void but_zdm_neg_Click(object sender, EventArgs e)
        {
            Sorter?.sendCommand("G1", new string[] { "Z", "A", "F" }, new object[] { -dist_z, -dist_z, vel_z });
        }

        private void but_e_neg_Click(object sender, EventArgs e)
        {
            Sorter?.sendCommand("G1", new string[] { "E", "F" }, new object[] { -dist_e, vel_e });
        }

        private void but_e_pos_Click(object sender, EventArgs e)
        {
            Sorter?.sendCommand("G1", new string[] { "E", "F" }, new object[] { dist_e, vel_e });
        }
        double dist_xy = 1;
        double dist_z = 1;
        double dist_e = 0.1;
      
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

        double vel_xy = 1.5 * 60;
        double vel_z = 1.5 * 60;
        double vel_e = 5 * 60;
        private void textBox_xy_vel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                vel_xy =60* MainForm.to_double_textbox(textBox_xy_vel, 0.1, 8);
            }
        }

        private void textBox_z_vel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                vel_z = 60 * MainForm.to_double_textbox(textBox_z_vel, 0.1, 8);
            }
        }

        private void textBox_e_vel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                vel_e = 60 * MainForm.to_double_textbox(textBox_e_vel, 0.1, 8);
            }
        }
        #endregion


        double[] cur_pos = new double[5];
        private void timer_printer_pos_Tick(object sender, EventArgs e)
        {
            var res = Sorter.reseav();
           /* if (ConnectionData.Value.prog != null && !ConnectionData.Value.prog_loaded)
            {
                Auto.set_g_code(ConnectionData.Value.prog);
                ConnectionData.Value.prog_loaded = true;
            }
            */
            if (res != null)
            {
                if (res.Length != 0)
                {
                    //Console.Write(res);
                    var res_spl = res.Split('\n');
                    for (int i = 0; i < res_spl.Length; i++)
                    {
                        var res_spl_2 = res_spl[i].Split(' ');
                        if (res_spl_2.Length > 10)
                        {
                            if (res_spl_2[0].Contains("cur_pos"))
                            {
                                if (res_spl_2.Length >= 5)
                                { 
                                    
                                    label_cur_pos.Text = "X: " + res_spl_2[0] + "\nY: " + res_spl_2[1] + "\nZd: " + res_spl_2[2] + "\nZm: " + res_spl_2[3] + "\nE: " + res_spl_2[4];
                                    for(int k=0; k<5;k++)
                                    {
                                        cur_pos[i] = MainForm.to_double(res_spl_2[i]);
                                    }
                                }
                            }
                        }
                        if (res_spl_2.Length >= 4)
                        {

                            if (res_spl_2[0].Contains("SD"))
                            {
                                //Console.WriteLine(res);
                                var prog = res_spl_2[3].Split('/');
                                try
                                {
                                    var cur_pr = Convert.ToInt32(prog[0]);
                                    var all_pr = Convert.ToInt32(prog[1]);
                                    //cur_byte_sd_print = cur_pr;
                                    //Auto.set_cur_line(cur_pr);
                                    //Auto.redraw();
                                    //lab_prog_cur.Text = cur_pr + " from " + all_pr;
                                }
                                catch { }
                            }
                        }
                    }
                }

            }
        }
        double[] start_point = new double[5];
        double[] stop_point = new double[5];
        double asp_vol = 0.1;
        double dm = 8;
        double z_safe = 2;
        private void button_memorize_start_point_Click(object sender, EventArgs e)
        {
            start_point = cur_pos;
        }

        private void button_memorize_stop_point_Click(object sender, EventArgs e)
        {
            stop_point = cur_pos;
        }

        private void textBox_volume_apiration_KeyDown(object sender, KeyEventArgs e)
        {
            asp_vol = MainForm.to_double_textbox(textBox_volume_apiration, 0.001, 80);
        }

        private void button_aspiration_obj_Click(object sender, EventArgs e)
        {
            Sorter?.sendCommand("G1", new string[] { "E", "F" }, new object[] { -asp_vol, vel_e });
        }

        private void button_push_obj_Click(object sender, EventArgs e)
        {
            Sorter?.sendCommand("G1", new string[] { "E", "F" }, new object[] { asp_vol, vel_e });
        }

        private void button_replace_obj_Click(object sender, EventArgs e)
        {
            //many commands
            
        }

        private void button_set_dm_dist_Click(object sender, EventArgs e)
        {
            dm = cur_pos[3] - cur_pos[2];
        }
    }
}
