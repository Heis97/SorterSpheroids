using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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


        GFrame cur_pos;
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
                    Console.Write(res);
                    var res_spl = res.Split('\n');
                    for (int i = 0; i < res_spl.Length; i++)
                    {
                        var res_spl_2 = res_spl[i].Split(' ');
                       
                        if (res_spl_2.Length >= 6)
                        {

                            if (res_spl_2[0] == "cur_pos")
                            {
                                var double_vals = new double[5];
                                var text = "X: "+ res_spl_2[0] + "\nY:" + res_spl_2[1] + "\nZd:" + res_spl_2[2] + "\nZm" + res_spl_2[3] + "\nE" + res_spl_2[4];
                                label_cur_pos.BeginInvoke((MethodInvoker)(() => label_cur_pos.Text = text));
                                for (int k = 0; k < 5; k++)
                                {
                                    double_vals[k] = MainForm.to_double(res_spl_2[k]);
                                }
                                cur_pos = new GFrame(double_vals);
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
        GFrame start_point;
        GFrame stop_point ;
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

       

        private void button_set_dm_dist_Click(object sender, EventArgs e)
        {
            dm = cur_pos.z - cur_pos.a;
        }
        GFrame go_to_pos(GFrame gframe_dest, GFrame gframe_cur)
        {
           
            var gframe = gframe_dest - gframe_cur;
            var vel = 1d;
            if (gframe.x != 0 || gframe.y != 0) vel = vel_xy;
            if (gframe.x == 0 && gframe.y == 0 && gframe.z != 0 && gframe.a != 0) vel = vel_z;
            if (gframe.x == 0 && gframe.y == 0 && gframe.z == 0 && gframe.a == 0 && gframe.e != 0) vel = vel_e;
            Sorter?.sendCommand("G1", new string[] { "X", "Y", "Z", "A", "E", "F" }, new object[] { gframe_dest.x, gframe_dest.y, gframe_dest.z, gframe_dest.a, gframe_dest.e, vel });
            gframe.f = vel;
            return gframe;
        }

        GFrame go_to_pos_wait(GFrame gframe_dest, GFrame gframe_cur)
        {
            var gframe = go_to_pos( gframe_dest ,gframe_cur);
            var dist = gframe.norm_all();
            var time =(int)(1000* dist / vel_sec(gframe.f));
            Thread.Sleep(time);
            return gframe;
        }
        private void button_replace_obj_Click(object sender, EventArgs e)
        {
            var repl_thr = new Thread(replace_obj);
            repl_thr.Start();
        }
        void replace_obj()
        {
            Sorter?.sendCommand("G90");
            //pos under start
            var pos_execute = start_point;
            pos_execute.z = start_point.a + dm + z_safe;
            pos_execute.e = cur_pos.e;
            go_to_pos_wait(pos_execute,cur_pos);
            //pos start
            pos_execute.z = start_point.a + dm;
            go_to_pos_wait(pos_execute, cur_pos);
            //aspir
            pos_execute.e -= asp_vol;
            go_to_pos_wait(pos_execute, cur_pos);
            //pos under start
            pos_execute.z = start_point.a + dm + z_safe;
            go_to_pos_wait(pos_execute, cur_pos);
            //pos under end
            pos_execute.z = stop_point.a + dm + z_safe;
            go_to_pos_wait(pos_execute, cur_pos);
            //pos end
            pos_execute.z = stop_point.a + dm;
            go_to_pos_wait(pos_execute, cur_pos);
            //push
            pos_execute.e += asp_vol;
            go_to_pos_wait(pos_execute, cur_pos);
            //pos under end
            pos_execute.z = stop_point.a + dm + z_safe;
            go_to_pos_wait(pos_execute, cur_pos);
            Sorter?.sendCommand("G91");
        }
        double vel_sec(double vel_minute)
        {
            return vel_minute / 60;
        }

        private void button_relative_movment_mode_Click(object sender, EventArgs e)
        {
            Sorter?.sendCommand("G91");
        }

        private void button_absolute_movment_mode_Click(object sender, EventArgs e)
        {
            Sorter?.sendCommand("G90");
        }
    }
}
