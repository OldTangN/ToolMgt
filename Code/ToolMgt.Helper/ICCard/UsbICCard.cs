using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using ToolMgt.Common;

namespace ToolMgt.Helper.ICCard
{
    public class UsbICCard : ICardHelper
    {
        #region
        [DllImport("dcrf32.dll")]
        public static extern int dc_init(Int16 port, Int32 baud);  //初试化
        [DllImport("dcrf32.dll")]
        public static extern short dc_exit(int icdev);
        [DllImport("dcrf32.dll")]
        public static extern short dc_beep(int icdev, uint _Msec);
        [DllImport("dcrf32.dll")]
        public static extern short dc_card_double_hex(int icdev, char _Mode, [MarshalAs(UnmanagedType.LPStr)] StringBuilder Snr);  //从卡中读数据(转换为16进制)

        [DllImport("dcrf32.dll")]
        public static extern short dc_read(int icdev, int adr, [Out] byte[] sdata);  //从卡中读数据
        [DllImport("dcrf32.dll")]
        public static extern short dc_read(int icdev, int adr, [MarshalAs(UnmanagedType.LPStr)] StringBuilder sdata);  //从卡中读数据

        [DllImport("dcrf32.dll")]
        public static extern int dc_reset(int icdev, uint sec);
        [DllImport("dcrf32.dll")]
        public static extern short dc_config_card(int icdev, byte cardtype);

        [DllImport("dcrf32.dll")]
        public static extern short dc_card(int icdev, char _Mode, ref ulong Snr);

        [DllImport("dcrf32.dll")]
        public static extern short dc_read_hex(int icdev, int adr, ref byte sdata);  //从卡中读数据(转换为16进制)
        [DllImport("dcrf32.dll")]
        public static extern short dc_read_hex(int icdev, int adr, [MarshalAs(UnmanagedType.LPStr)] StringBuilder sdata);  //从卡中读数

        [DllImport("dcrf32.dll")]
        public static extern short dc_write(int icdev, int adr, [In] string sdata);  //向卡中写入数据
        [DllImport("dcrf32.dll")]
        public static extern short dc_write_hex(int icdev, int adr, [In] string sdata);  //向卡中写入数据(转换为16进制)
        #endregion

        public int Icdev { set; get; } = -1;

        public int Port { set; get; }
        public bool IsRead { set; get; } = false;
        public int Baudrate { set; get; } = 115200;

        private bool _keepreading = false;

        public event EventHandler<DataEventArgs> HandDataBack;

        public UsbICCard(int _port, int _Baudrate)
        {
            try
            {
                Port = _port;
                Baudrate = _Baudrate;
                Icdev = dc_init(Convert.ToInt16(_port), _Baudrate);
                if (Icdev > 0)
                {
                    _keepreading = true;
                }
                else
                {
                    _keepreading = false;
                    LogUtil.WriteLog("读卡器连接失败！");
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("读卡器端口" + _port + "打开失败！", ex);
            }
        }

        public void Read()
        {
            while (_keepreading)
            {
                try
                {
                    ulong icCardNo = 0;
                    int st = dc_reset(Icdev, 0);
                    dc_config_card(Icdev, 65);
                    dc_card(Icdev, '0', ref icCardNo);
                    if (icCardNo != 0)
                    {
                        dc_beep(Icdev, 10);
                        HandDataBack?.Invoke(this, new DataEventArgs(icCardNo.ToString()));
                        IsRead = false;
                    }
                    TimeSpan waitTime = new TimeSpan(0, 0, 0, 2, 50);
                    Thread.Sleep(waitTime);
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                }
            }
        }

        public bool IsOpen()
        {
            return Icdev > 0;
        }

        public void Close()
        {
            try
            {
                _keepreading = false;
                if (Icdev > 0)
                {
                    dc_exit(Icdev);
                    Icdev = -1;
                }
            }
            catch (Exception)
            {
            }
        }

        ~UsbICCard()
        {
            Close();
        }
    }
}
