using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Connection
{
    public class GFrame
    {
        public double x, y, z, e, a, b,f;
        public GFrame(double x = 0, double y = 0, double z = 0, double e = 0, double a = 0, double b = 0, double f = 0) 
        { 
            this.x = x;
            this.y = y;
            this.z = z;
            this.e = e;
            this.a = a;
            this.b = b;
            this.f = f;
        }

        public GFrame(string data)
        {
            var data_s = data.Split(' ');
            this.x = Convert.ToDouble(data_s[0]);
            this.y = Convert.ToDouble(data_s[1]);
            this.z = Convert.ToDouble(data_s[2]);
            this.e = Convert.ToDouble(data_s[3]);
            this.a = Convert.ToDouble(data_s[4]);
            this.b = Convert.ToDouble(data_s[5]);
            this.f = Convert.ToDouble(data_s[6]);
        }
        public GFrame(double[] vals)
        {
            if (vals == null) return;
            if (vals.Length < 5) return;
            this.x = vals[0];
            this.y = vals[1];
            this.z = vals[2];
            this.a = vals[3];
            this.e = vals[4];
        }
        public GFrame Clone()
        {
            return new GFrame(x,y,z,e,a,b,f);
        }

        public double norm_movm()
        {
            return Math.Sqrt(x*x + y*y + z*z + a*a + b*b);
        }
        public double norm_all()
        {
            return Math.Sqrt(x * x + y * y + z * z + e * e + a * a + b * b);
        }
        static public GFrame operator- (GFrame left, GFrame right)
        {
            return new GFrame(
                left.x - right.x,
                left.y - right.y,
                left.z - right.z,
                left.e - right.e,
                left.a - right.a,
                left.b - right.b,
                left.f);
        }

        static public GFrame operator +(GFrame left, GFrame right)
        {
            return new GFrame(
                left.x + right.x,
                left.y + right.y,
                left.z + right.z,
                left.e + right.e,
                left.a + right.a,
                left.b + right.b, left.f);
        }

        static public GFrame operator -(GFrame left)
        {
            return new GFrame(
                -left.x ,
                -left.y ,
                -left.z ,
                -left.e ,
                -left.a ,
                -left.b , left.f);
        }

        public override string ToString()
        {
            return x + " " + y + " " + z + " " + e + " " + a + " " + b + " " + f;
        }
    }
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
            Console.Write(command_marl);
            serialPort.Write(command_marl);
            cur_com++;
            
            int k = 0;
            reseav();
            var resv = buff.ToString();
            while ((resv.Contains("ok") == false)&&
                (resv.Contains("Re") == false) &&
                (k<50))
            {
                try {
                    sleep100mks();
                    reseav();
                    resv = buff.ToString();
                    k++;

                }
                catch (Exception e) { }
                

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
