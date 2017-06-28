using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SerialPortService;

namespace CommandService
{
    /// <summary>
    /// 指令处理服务
    /// </summary>
    public class CommandHelper
    {
        /// <summary> 
        /// 指令结构体
        /// </summary>
        public struct CommandList
        {
            //投影机开/关
            public byte[] CMD_ProjectPowerOn;
            public byte[] CMD_ProjectPowerOff;

            //镜头开/关
            public byte[] CMD_FilmPowerOn;
            public byte[] CMD_FilmPowerOff;

            //放大/缩小
            public byte[] CMD_FilmEnlarge;
            public byte[] CMD_FilmMinnor;

            //停止缩放
            public byte[] CMD_FilmZoomStop;

            //自动聚焦
            public byte[] CMD_FilmAutoFocus;

            //冻结/解冻
            public byte[] CMD_FilmFroze;
            public byte[] CMD_FilmUnFroze;

            //保存/下载
            public byte[] CMD_FilmSaveFilm;
            public byte[] CMD_FilmDownloadFile;

            //镜头亮度增加/减少
            public byte[] CMD_FilmLightIncrease;
            public byte[] CMD_FilmLightReduce;

            //阅片台亮度增加/减少/关闭
            public byte[] CMD_TableLightIncrease;
            public byte[] CMD_TableLightReduce;
            public byte[] CMD_TableLightClose;
        }

        /// <summary>
        /// 指令集和实例
        /// </summary>
        private CommandList Command;

        /// <summary>
        /// 串口服务实例
        /// </summary>
        private SerialPortHelper Port;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="ConfigPath"></param>
        public CommandHelper(string CommandConfigPath,string SerialConfigPath)
        {
            InitItems(CommandConfigPath,SerialConfigPath);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ConfigPath"></param>
        private void InitItems(string CommandConfigPath,string SerialConfigPath)
        {
            StreamReader Reader;

            try
            {
                Reader = new StreamReader(CommandConfigPath);
            }
            catch (Exception)
            {
                throw new Exception("指令配置文件不存在！");
            }

            try
            {
                Command.CMD_ProjectPowerOn = StrToHex(Reader.ReadLine());
                Command.CMD_ProjectPowerOff = StrToHex(Reader.ReadLine());

                Command.CMD_FilmPowerOn = StrToHex(Reader.ReadLine());
                Command.CMD_FilmPowerOff = StrToHex(Reader.ReadLine());

                Command.CMD_FilmEnlarge = StrToHex(Reader.ReadLine());
                Command.CMD_FilmMinnor = StrToHex(Reader.ReadLine());
                Command.CMD_FilmZoomStop = StrToHex(Reader.ReadLine());

                Command.CMD_FilmAutoFocus = StrToHex(Reader.ReadLine());

                Command.CMD_FilmFroze = StrToHex(Reader.ReadLine());
                Command.CMD_FilmUnFroze = StrToHex(Reader.ReadLine());

                Command.CMD_FilmSaveFilm = StrToHex(Reader.ReadLine());
                Command.CMD_FilmDownloadFile = StrToHex(Reader.ReadLine());

                Command.CMD_FilmLightIncrease = StrToHex(Reader.ReadLine());
                Command.CMD_FilmLightReduce = StrToHex(Reader.ReadLine());

                Command.CMD_TableLightIncrease = StrToHex(Reader.ReadLine());
                Command.CMD_TableLightReduce = StrToHex(Reader.ReadLine());
                Command.CMD_TableLightClose = StrToHex(Reader.ReadLine());

                Reader.Close();
            }
            catch (Exception)
            {
                throw new Exception("配置文件错误！");
            }

            try
            {
                Port = new SerialPortHelper(SerialConfigPath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 投影机开
        /// </summary>
        public void ProjectorPowerOn()
        {
            try
            {
                Port.SendCommandToProject(Command.CMD_ProjectPowerOn);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 投影机关
        /// </summary>
        public void ProjectorPowerOff()
        {
            try
            {
                Port.SendCommandToProject(Command.CMD_ProjectPowerOff);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 镜头开
        /// </summary>
        public void FilmPowerOn()
        {
            try
            {
                Port.SendCommandToFilm(Command.CMD_FilmPowerOn);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 镜头关
        /// </summary>
        public void FilmPowerOff()
        {
            try
            {
                Port.SendCommandToFilm(Command.CMD_FilmPowerOff);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 自动聚焦
        /// </summary>
        public void FilmAutoFocus()
        {
            try
            {
                Port.SendCommandToFilm(Command.CMD_FilmAutoFocus);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 放大
        /// </summary>
        public void FilmEnlarge()
        {
            try
            {
                Port.SendCommandToFilm(Command.CMD_FilmEnlarge);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 缩小
        /// </summary>
        public void FilmMinnor()
        {
            try
            {
                Port.SendCommandToFilm(Command.CMD_FilmMinnor);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 缩放停止
        /// </summary>
        public void FilmZoomStop()
        {
            try
            {
                Port.SendCommandToFilm(Command.CMD_FilmZoomStop);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 冻结
        /// </summary>
        public void FilmFroze()
        {
            try
            {
                Port.SendCommandToFilm(Command.CMD_FilmFroze);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 解冻
        /// </summary>
        public void FilmUnFroze()
        {
            try
            {
                Port.SendCommandToFilm(Command.CMD_FilmUnFroze);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 镜头亮度增加
        /// </summary>
        public void FilmBrightIncrease()
        {
            try
            {
                Port.SendCommandToFilm(Command.CMD_FilmLightIncrease);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 镜头亮度减少
        /// </summary>
        public void FilmBrightReduce()
        {
            try
            {
                Port.SendCommandToFilm(Command.CMD_FilmLightReduce);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 阅片台亮度增加
        /// </summary>
        public void TableBrightIncrese()
        {
            try
            {
                Port.SendCommandToTable(Command.CMD_TableLightIncrease);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 阅片台亮度减少
        /// </summary>
        public void TableBrightReduce()
        {
            try
            {
                Port.SendCommandToTable(Command.CMD_TableLightReduce);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void TableBrightClose()
        {
            try
            {
                Port.SendCommandToTable(Command.CMD_TableLightClose);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 字符串转字节数组
        /// </summary>
        /// <param name="str"></param>
        private byte[] StrToHex(string CommandStr)
        {
            try
            {
                CommandStr = CommandStr.Replace("0x", "");
                CommandStr = CommandStr.Replace("0X", "");
                CommandStr = CommandStr.Replace(" ", "");
                CommandStr = CommandStr.Replace("\n", "");

                string[] StrArray = CommandStr.Split(',');

                int Len = StrArray.Length;

                byte[] ReturnHex = new byte[Len];

                for (int i = 0; i < Len; i++)
                {
                    ReturnHex[i] = Convert.ToByte(StrArray[i], 16);
                }
                return ReturnHex;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
