using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;

namespace SerialPortService
{
    /// <summary>
    /// 串口控制服务
    /// </summary>
    public class SerialPortHelper
    {
        /// <summary>
        /// 镜头串口
        /// </summary>
        private SerialPort FilmPort;

        /// <summary>
        /// 投影机串口
        /// </summary>
        private SerialPort ProjectorPort;

        /// <summary>
        /// 阅片台控制板串口
        /// </summary>
        private SerialPort TablePort;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="ConfigPath"></param>
        public SerialPortHelper(string SerialConfigPath)
        {
            InitItems(SerialConfigPath);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ConfigPath"></param>
        private void InitItems(string SerialConfigPath)
        {
            StreamReader Reader;

            FilmPort = new SerialPort();
            ProjectorPort = new SerialPort();
            TablePort = new SerialPort();

            string ProjectorStatus;

            string[] FilmPortConfig;
            string[] ProjectorPortConfig;
            string[] TablePortConfig;

            try
            {
                Reader = new StreamReader(SerialConfigPath);

                ProjectorStatus = Reader.ReadLine();

                if (string.Equals("NoProjector",ProjectorStatus))
                {
                    try
                    {
                        FilmPortConfig = Reader.ReadLine().Split(':');
                        TablePortConfig = Reader.ReadLine().Split(':');

                        FilmPort.PortName = FilmPortConfig[0];
                        FilmPort.BaudRate = Convert.ToInt32(FilmPortConfig[1]);

                        TablePort.PortName = TablePortConfig[0];
                        TablePort.BaudRate = Convert.ToInt32(TablePortConfig[1]);
                    }
                    catch (Exception)
                    {
                        throw new Exception("串口配置文件格式错误！");
                    }
                }
                else if (string.Equals("Projector", ProjectorStatus))
                {
                    try
                    {
                        ProjectorPortConfig = Reader.ReadLine().Split(':');
                        FilmPortConfig = Reader.ReadLine().Split(':');
                        TablePortConfig = Reader.ReadLine().Split(':');

                        ProjectorPort.PortName = ProjectorPortConfig[0];
                        ProjectorPort.BaudRate = Convert.ToInt32(ProjectorPortConfig[1]);

                        FilmPort.PortName = FilmPortConfig[0];
                        FilmPort.BaudRate = Convert.ToInt32(FilmPortConfig[1]);

                        TablePort.PortName = TablePortConfig[0];
                        TablePort.BaudRate = Convert.ToInt32(TablePortConfig[1]);
                    }
                    catch (Exception)
                    {
                        throw new Exception("串口配置文件格式错误！");
                    }
                }
                else
                {
                    throw new Exception("串口配置文件格式错误！");
                }
            }
            catch (Exception)
            {
                throw new Exception("串口配置文件不存在！");
            }
        }

        /// <summary>
        /// 发送指令到投影仪
        /// </summary>
        /// <param name="Command"></param>
        public void SendCommandToProject(byte[] Command)
        {
            if (!ProjectorPort.IsOpen)
            {
                try
                {
                    ProjectorPort.Open();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            try
            {
                ProjectorPort.Write(Command, 0, Command.Length);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 发送指令到镜头
        /// </summary>
        /// <param name="Command"></param>
        public void SendCommandToFilm(byte[] Command)
        {
            if (!FilmPort.IsOpen)
            {
                try
                {
                    FilmPort.Open();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            try
            {
                FilmPort.Write(Command, 0, Command.Length);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 发送指令到阅片台控制器
        /// </summary>
        /// <param name="Command"></param>
        public void SendCommandToTable(byte[] Command)
        {
            if (!TablePort.IsOpen)
            {
                try
                {
                    TablePort.Open();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            try
            {
                TablePort.Write(Command, 0, Command.Length);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
