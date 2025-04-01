using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connection
{
    public class Tensom : DeviceArduino
    {
        string port;
        int baudrate = 250000;
        int dir_move = 0, 
            vel_move = 1,
            force_dest = 2,
            force_en=3, 
            cycle_type = 4, 
            sensor_up=5, 
            sensor_down=6,
            force_max = 7,
            time_force = 8,
            time_relax = 9,
            light_en = 10;

        int on = 1, off = 0;
        int up = 0, stop = 1, down = 2;
        int cycle_stop = 0, cycle_meandr = 1, cycle_triangle = 2, cycle_sinus = 3;
      
        public Tensom(string _port)
        {
            port = _port;
            connect(port, baudrate);
        }
        public bool connectStart()
        {
            return connect(port, baudrate);
        }
        public void connectStop()
        {
            close();
        }
        public void sensorUp()
        {
            send(on, sensor_up);
        }
        public void sensorDown()
        {
            send(on, sensor_down);
        }
        public void setForce(int _force)
        {
            send(_force, force_dest);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_enable">0 - выкл, 1 - вкл</param>
        public void enableForce(int _enable)
        {
            send(_enable, force_en);
        }
        /// <summary>
        /// Установка параметров цикла сжатия
        /// </summary>
        /// <param name="_time_force"> время усилия, мс</param>
        /// <param name="_time_relax"> время без усилия, мс</param>
        /// <param name="_force_max"> максимальная амплитуда сжатия, граммы</param>
        public void setSettings(int _time_force, int _time_relax, int _force_max)
        {
            send(_time_force, time_force);
            send(_time_relax, time_relax);
            send(_force_max, force_max);
        }
        /// <summary>
        /// Установка типа цикла
        /// </summary>
        /// <param name="_cycle_type">
        /// 0 - выключен, 1 - меандр, 2 - треугольник, 3 - синус
        /// </param>
        public void setCycle(int _cycle_type)
        {
            send(_cycle_type, cycle_type);
        }

        public void enableLight(int _enable)
        {
            send(_enable, light_en);
        }

        

    }
}
