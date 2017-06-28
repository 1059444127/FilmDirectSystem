using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using CommandService;

namespace FilmDirectSystem
{
    /// <summary>
    /// 主界面
    /// </summary>
    public partial class frm_Main : Form
    {
        #region 全局变量
        /// <summary>
        /// 指令集合
        /// </summary>
        private CommandHelper Command;

        /// <summary>
        /// 冻结标志位
        /// </summary>
        private volatile bool isFrozen = false;

        /// <summary>
        /// 增加亮度标志位
        /// </summary>
        private volatile bool isFilmBrightIncrease = false;

        /// <summary>
        /// 减少亮度标志位
        /// </summary>
        private volatile bool isFilmBrightReduce = false;

        /// <summary>
        /// 调光板增加亮度标志位
        /// </summary>
        private volatile bool isTableBrightIncrease = false;

        /// <summary>
        /// 调光板减少亮度标志位
        /// </summary>
        private volatile bool isTableBrightReduce = false;

        private string JPEGFilesPath;

        private string DicomFilesPath;

        private int FilmDelayTime;

        private int TableDelayTime;
        #endregion

        #region 加载动画
        //窗体弹出或消失效果
        [DllImport("user32.dll", EntryPoint = "AnimateWindow")]
        private static extern bool AnimateWindow(IntPtr handle, int ms, int flags);
        public const Int32 AW_HOR_POSITIVE = 0x00000001;
        public const Int32 AW_HOR_NEGATIVE = 0x00000002;
        public const Int32 AW_VER_POSITIVE = 0x00000004;
        public const Int32 AW_VER_NEGATIVE = 0x00000008;
        public const Int32 AW_CENTER = 0x00000010;
        public const Int32 AW_HIDE = 0x00010000;
        public const Int32 AW_ACTIVATE = 0x00020000;
        public const Int32 AW_SLIDE = 0x00040000;
        public const Int32 AW_BLEND = 0x00080000;
        #endregion

        #region 构造器
        /// <summary>
        /// 默认构造器
        /// </summary>
        public frm_Main()
        {
            InitializeComponent();
            InitItems();
        }
        #endregion

        #region 用户事件
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_Main_Load(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 500, AW_BLEND);
            FilmPowerOn();
            StartService();
        }

        /// <summary>
        /// 按下放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBx_Enlarge_MouseDown(object sender, MouseEventArgs e)
        {
            pBx_Enlarge.Image = Properties.Resources.Enarge_Down;
            FilmEnlarge();
        }

        /// <summary>
        /// 松开放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBx_Enlarge_MouseUp(object sender, MouseEventArgs e)
        {
            pBx_Enlarge.Image = Properties.Resources.Enarge_Up;
            FilmZoomStop();
        }

        /// <summary>
        /// 按下缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBx_Narrow_MouseDown(object sender, MouseEventArgs e)
        {
            pBx_Narrow.Image = Properties.Resources.Narrow_Down;
            FilmMinnor();
        }

        /// <summary>
        /// 松开缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBx_Narrow_MouseUp(object sender, MouseEventArgs e)
        {
            pBx_Narrow.Image = Properties.Resources.Narrow_Up;
            FilmZoomStop();
        }

        /// <summary>
        /// 按下聚焦
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBx_Focus_MouseDown(object sender, MouseEventArgs e)
        {
            pBx_Focus.Image = Properties.Resources.Film_Focus_Down;
            FilmFocus();
        }

        /// <summary>
        /// 松开聚焦
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBx_Focus_MouseUp(object sender, MouseEventArgs e)
        {
            pBx_Focus.Image = Properties.Resources.Film_Focus_Up;
        }

        /// <summary>
        /// 按下镜头亮度增加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBx_Film_Bright_Incress_MouseDown(object sender, MouseEventArgs e)
        {
            pBx_Film_Bright_Incress.Image = Properties.Resources.Film_Bright_Incress_Down;
            isFilmBrightIncrease = true;
        }

        /// <summary>
        /// 松开镜头亮度增加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBx_Film_Bright_Incress_MouseUp(object sender, MouseEventArgs e)
        {
            pBx_Film_Bright_Incress.Image = Properties.Resources.Film_Bright_Incress_Up;
            isFilmBrightIncrease = false;
        }

        /// <summary>
        /// 按下镜头亮度减少
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBx_Film_Bright_Reduce_MouseDown(object sender, MouseEventArgs e)
        {
            pBx_Film_Bright_Reduce.Image = Properties.Resources.Film_Bright_Reduce_Down;
            isFilmBrightReduce = true;
        }

        /// <summary>
        /// 松开镜头亮度减少
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBx_Film_Bright_Reduce_MouseUp(object sender, MouseEventArgs e)
        {
            pBx_Film_Bright_Reduce.Image = Properties.Resources.Film_Bright_Reduce_Up;
            isFilmBrightReduce = false;
        }

        /// <summary>
        /// 按下灯箱亮度增加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBx_Bright_Incress_MouseDown(object sender, MouseEventArgs e)
        {
            pBx_Bright_Incress.Image = Properties.Resources.Bright_Incress_Down;
            isTableBrightIncrease = true;
        }

        /// <summary>
        /// 松开灯箱亮度增加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBx_Bright_Incress_MouseUp(object sender, MouseEventArgs e)
        {
            pBx_Bright_Incress.Image = Properties.Resources.Bright_Incress_Up;
            isTableBrightIncrease = false;
        }

        /// <summary>
        /// 按下灯箱亮度减少
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBx_Bright_Reduce_MouseDown(object sender, MouseEventArgs e)
        {
            pBx_Bright_Reduce.Image = Properties.Resources.Bright_Reduce_Down;
            isTableBrightReduce = true;
        }

        /// <summary>
        /// 松开灯箱亮度减少
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBx_Bright_Reduce_MouseUp(object sender, MouseEventArgs e)
        {
            pBx_Bright_Reduce.Image = Properties.Resources.Bright_Reduce_Up;
            isTableBrightReduce = false;
        }

        /// <summary>
        /// 按下关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBx_Close_MouseDown(object sender, MouseEventArgs e)
        {
            pBx_Close.Image = Properties.Resources.Close_Down;
        }

        /// <summary>
        /// 松开关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBx_Close_MouseUp(object sender, MouseEventArgs e)
        {
            pBx_Close.Image = Properties.Resources.Close_Up;
            SystemExit();
        }

        /// <summary>
        /// 按下保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBx_Save_MouseDown(object sender, MouseEventArgs e)
        {
            pBx_Save.Image = Properties.Resources.Save_Down;
        }

        /// <summary>
        /// 松开保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBx_Save_MouseUp(object sender, MouseEventArgs e)
        {
            pBx_Save.Image = Properties.Resources.Save_Up;
            SaveFilm();
            JPEGtoDICOM(JPEGFilesPath, DicomFilesPath);
        }

        /// <summary>
        /// 按下下载按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBx_DownLoad_MouseDown(object sender, MouseEventArgs e)
        {
            pBx_DownLoad.Image = Properties.Resources.Download_Down;
        }

        /// <summary>
        /// 松开下载按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBx_DownLoad_MouseUp(object sender, MouseEventArgs e)
        {
            pBx_DownLoad.Image = Properties.Resources.Download_Up;
            DownLoadFilm();
        }

        /// <summary>
        /// 点击锁定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBx_Frozen_Click(object sender, EventArgs e)
        {
            FilmFrozen();
        }

        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            AnimateWindow(this.Handle, 500, AW_BLEND + AW_HIDE);
        }
        #endregion

        #region 功能函数

        #region 初始化 
        /// <summary>
        /// 初始化
        /// </summary>
        private void InitItems()
        {
            try
            {
                Command = new CommandHelper(Application.StartupPath + "\\Config\\CommandConfig.bin", Application.StartupPath + "\\Config\\SerialPortConfig.bin");
                ReadConfig(Application.StartupPath + "\\Config\\UserConfig.bin");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
                System.Environment.Exit(0);
            }
        }
        #endregion

        #region 读取配置文件
        /// <summary>
        /// 读取用户配置
        /// </summary>
        /// <param name="ConfigPath"></param>
        private void ReadConfig(string ConfigPath)
        {
            try
            {
                StreamReader Reader = new StreamReader(ConfigPath);

                JPEGFilesPath = Reader.ReadLine();

                DicomFilesPath = Reader.ReadLine();

                FilmDelayTime = Convert.ToInt32(Reader.ReadLine());

                TableDelayTime = Convert.ToInt32(Reader.ReadLine());

                Reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 启动服务
        /// <summary>
        /// 启动服务
        /// </summary>
        private void StartService()
        {
            Thread TableBrightReduceThread = new Thread(TableBrightReduce);
            TableBrightReduceThread.IsBackground = true;
            TableBrightReduceThread.Start();

            Thread TableBrightIncreaseThread = new Thread(TableBrightIncrease);
            TableBrightIncreaseThread.IsBackground = true;
            TableBrightIncreaseThread.Start();

            Thread FilmBrightIncreaseThread = new Thread(FilmBrightIncrease);
            FilmBrightIncreaseThread.IsBackground = true;
            FilmBrightIncreaseThread.Start();

            Thread FilmBrightReduceThread = new Thread(FilmBrightReduce);
            FilmBrightReduceThread.IsBackground = true;
            FilmBrightReduceThread.Start();
        }
        #endregion

        #region 打开投影机
        /// <summary>
        /// 打开投影机
        /// </summary>
        private void ProjectorPowerOn()
        {
            try
            {
                Command.ProjectorPowerOn();
            }
            catch (Exception)
            {
                MessageBox.Show("请检查串口连接！", "错误");
            }
        }
        #endregion

        #region 关闭投影机
        /// <summary>
        /// 关闭投影机
        /// </summary>
        private void ProjectorPowerOff()
        {
            try
            {
                Command.ProjectorPowerOff();
            }
            catch (Exception)
            {
                MessageBox.Show("请检查串口连接！", "错误");
            }
        }
        #endregion

        #region 打开镜头
        /// <summary>
        /// 打开镜头
        /// </summary>
        private void FilmPowerOn()
        {
            try
            {
                Command.FilmPowerOn();
            }
            catch (Exception)
            {
                MessageBox.Show("请检查串口连接！", "错误");
                System.Environment.Exit(0);
            }
        }
        #endregion

        #region 关闭镜头
        /// <summary>
        /// 关闭镜头
        /// </summary>
        private void FilmPowerOff()
        {
            try
            {
                Command.FilmPowerOff();
            }
            catch (Exception)
            {
                MessageBox.Show("请检查串口连接！", "错误");
            }
        }
        #endregion

        #region 聚焦
        /// <summary>
        /// 聚焦
        /// </summary>
        private void FilmFocus()
        {
            try
            {
                Command.FilmAutoFocus();
            }
            catch (Exception)
            {
                MessageBox.Show("请检查串口连接！", "错误");
            }
        }
        #endregion

        #region 冻结解冻
        /// <summary>
        /// 冻结/解冻
        /// </summary>
        private void FilmFrozen()
        {
            try
            {
                if (isFrozen)
                {
                    Command.FilmUnFroze();
                    isFrozen = false;
                    pBx_Frozen.Image = Properties.Resources.Frozen_Off;
                }
                else
                {
                    Command.FilmFroze();
                    isFrozen = true;
                    pBx_Frozen.Image = Properties.Resources.Frozen_On;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("请检查串口连接！", "错误");
            }
        }
        #endregion

        #region 放大
        /// <summary>
        /// 放大
        /// </summary>
        private void FilmEnlarge()
        {
            try
            {
                Command.FilmEnlarge();
            }
            catch (Exception)
            {
                MessageBox.Show("请检查串口连接！", "错误");
            }
        }
        #endregion

        #region 缩小
        /// <summary>
        /// 缩小
        /// </summary>
        private void FilmMinnor()
        {
            try
            {
                Command.FilmMinnor();
            }
            catch (Exception)
            {
                MessageBox.Show("请检查串口连接！", "错误");
            }
        }
        #endregion

        #region 缩放停止
        /// <summary>
        /// 停止缩放
        /// </summary>
        private void FilmZoomStop()
        {
            try
            {
                Command.FilmZoomStop();
            }
            catch (Exception)
            {
                MessageBox.Show("请检查串口连接！", "错误");
            }
        }
        #endregion

        #region 镜头亮度增加
        /// <summary>
        /// 镜头亮度增加
        /// </summary>
        private void FilmBrightIncrease()
        {
            while (true)
            {
                try
                {
                    while (isFilmBrightIncrease)
                    {
                        Command.FilmBrightIncrease();
                        Thread.Sleep(FilmDelayTime);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("请检查串口连接！", "错误");
                }
            }
        }
        #endregion

        #region 镜头亮度减少
        /// <summary>
        /// 镜头亮度减少
        /// </summary>
        private void FilmBrightReduce()
        {
            while (true)
            {
                try
                {
                    while (isFilmBrightReduce)
                    {
                        Command.FilmBrightReduce();
                        Thread.Sleep(FilmDelayTime);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("请检查串口连接！", "错误");
                }
            }
        }
        #endregion

        #region 调光板亮度增加
        /// <summary>
        /// 调光板亮度增加
        /// </summary>
        private void TableBrightIncrease()
        {
            while (true)
            {
                try
                {
                    while (isTableBrightIncrease)
                    {
                        Command.TableBrightIncrese();
                        Thread.Sleep(TableDelayTime);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("请检查串口连接！", "错误");
                }
            }
        }
        #endregion

        #region 调光板亮度减少
        /// <summary>
        /// 调光板亮度减少
        /// </summary>
        private void TableBrightReduce()
        {
            while (true)
            {
                try
                {
                    while (isTableBrightReduce)
                    {
                        Command.TableBrightReduce();
                        Thread.Sleep(TableDelayTime);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("请检查串口连接！", "错误");
                }
            }
        }
        #endregion

        #region 关闭调光板
        /// <summary>
        /// 关闭调光板
        /// </summary>
        private void TableBrightClose()
        {
            try
            {
                Command.TableBrightClose();
            }
            catch (Exception)
            {
                MessageBox.Show("请检查串口连接！", "错误");
            }
        }
        #endregion

        #region 胶片保存
        /// <summary>
        /// 保存文件
        /// </summary>
        private void SaveFilm()
        {

        }
        #endregion

        #region 胶片转换
        /// <summary>
        /// 胶片格式转换
        /// </summary>
        /// <param name="JPEGPath"></param>
        /// <param name="DICOMPath"></param>
        private void JPEGtoDICOM(string JPEGPath, string DICOMPath)
        {
            if (!Directory.Exists(JPEGPath))
            {
                MessageBox.Show("请指定存放jpeg胶片的路径！", "错误");
                return;
            }

            if (!Directory.Exists(DICOMPath))
            {
                Directory.CreateDirectory(DICOMPath);
            }

            var FileNames = Directory.GetFiles(JPEGPath);

            string DICOMFileName = string.Empty;

            foreach (var JPEGFileName in FileNames)
            {
                DICOMFileName = Path.GetFileNameWithoutExtension(JPEGFileName) + ".DCM";

                if (File.Exists(DICOMFileName))
                {
                    continue;
                }
                else
                {
                    DICOM.ConvertFileToDICOMasGray(JPEGFileName, DICOMFileName);
                }
            }
        }
        #endregion

        #region 胶片下载
        /// <summary>
        /// 胶片下载
        /// </summary>
        private void DownLoadFilm()
        {

        }
        #endregion

        #region 系统退出
        /// <summary>
        /// 系统退出
        /// </summary>
        private void SystemExit()
        {
            if (MessageBox.Show("是否关闭系统？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                //关闭镜头
                FilmPowerOff();

                //关闭调光板
                TableBrightClose();

                //关机
                Process.Start("Shutdown.exe", "-s -t 0");
            }
        }
        #endregion

        #endregion

    }
}
