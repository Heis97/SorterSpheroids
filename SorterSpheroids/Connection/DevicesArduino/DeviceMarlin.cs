using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Connection
{
    public class DeviceMarlin : DeviceArduino
    {
        public string port;
        int baudrate = 250000;
        int cur_com = 1;

        
        public DeviceMarlin(string _port) : base()
        {

            port = _port;
            serialPort.RtsEnable = true;
            serialPort.DtrEnable = true;
            connect(port, baudrate);
            
        }

        int calcSum(string command)
        {
            int sum = 0;
            foreach (var symb in command) sum ^= symb;
            return sum;
        }

        public void sendCommand(string com, string[] vars, object[] vals)
        {
            if (vars.Length != vals.Length)
            {
                return;
            }
            var command = com;
            for (int i = 0; i < vars.Length; i++)
            {
                command += " " + vars[i] + vals[i].ToString();
            }
            sendCommand(command);
        }
        void sleep100mks()
        {
            for (int i = 0; i < 2000; i++)// 20000 - ~1 ms real
            {
                SpinWait sw = new SpinWait();
                sw.SpinOnce();
            }
        }
        public void sendCommand(string command)
        {
            if (!serialPort.IsOpen) return;
            var command_marl = "N" + cur_com.ToString() + " " + command;
            command_marl+= "*" + calcSum(command_marl).ToString() + "\n";

            serialPort.Write(command_marl);
            cur_com++;
            
            int k = 0;
            reseav();
            var resv = buff.ToString();
            while ((resv.Contains("ok") == false)&&
                (resv.Contains("Re") == false) &&
                (k<50))
            {
                sleep100mks();
                reseav();
                resv = buff.ToString();
                k++;

            };
            //Console.WriteLine("k: " + k);
            
            bool err = false;
           // Console.WriteLine("res_all: "+res_all+" end");
            try
            {
                var res_all_arr = buff.ToString().Split('\n');
                foreach(var res in res_all_arr)
                {
                   // Console.WriteLine("res "+res+" end");
                    if (res.Contains("Resend"))
                    {
                        var ind_err = res.IndexOf("Resend");
                        var res_sub = res.Substring(ind_err);
                        var res_spl = res_sub.Split(':');
                       
                        var err_n = Convert.ToInt32(res_spl[1].Trim());
                       // Console.WriteLine("err :" + err_n); 
                        cur_com = err_n;
                        err = true;
                        buff = new StringBuilder();
                    }
                    else
                    {
                        buff = new StringBuilder();
                    }
                }
                if(err)
                {

                    sendCommand(command);
                }
                
            }
            catch
            {

            }
            
        }
        public void sendCommand_leg(string command)
        {
            var command_marl = "N" + cur_com.ToString() + " " + command;
            command_marl += "*" + calcSum(command_marl).ToString() + "\n";

            serialPort.Write(command_marl);
            cur_com++;
            for (int i = 0; i < 20000; i++)// 20000 - ~1 ms real
            {
                SpinWait sw = new SpinWait();
                sw.SpinOnce();
            }

            reseav();

            bool err = false;
            // Console.WriteLine("res_all: "+res_all+" end");
            try
            {
                var res_all_arr = buff.ToString().Split('\n');
                foreach (var res in res_all_arr)
                {
                    Console.WriteLine("res " + res + " end");
                    if (res.Contains("Resend"))
                    {
                        var ind_err = res.IndexOf("Resend");
                        var res_sub = res.Substring(ind_err);
                        var res_spl = res_sub.Split(':');

                        var err_n = Convert.ToInt32(res_spl[1].Trim());
                        Console.WriteLine("err :" + err_n);
                        cur_com = err_n;
                        err = true;
                        buff = new StringBuilder();
                    }
                    else
                    {
                        buff = new StringBuilder();
                    }
                }
                if (err)
                {

                    sendCommand(command);
                }

            }
            catch
            {

            }

        }
        void sendXpos(double pos)
        {
            sendCommand("M92", new string[] { "X"}, new object[] { 1 });
            sendCommand("G1",new string[] { "X","F" }, new object[] { pos,24000 });
        }


        public bool connectStart()
        {
            return connect(port, baudrate);
        }
        public void connectStop()
        {
            close();
        }

        public void setShvpPos(double _pos)
        {
            sendXpos(_pos);
        }

    }
}
