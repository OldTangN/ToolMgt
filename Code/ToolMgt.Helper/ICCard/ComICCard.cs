
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToolMgt.Common;

namespace ToolMgt.Helper.ICCard
{
    public class ComICCard : ICardHelper
    {
        private SerialPort port = new SerialPort();
        public bool IsRead { set; get; } = false;
        private bool _keepreading = false;

        public event EventHandler<DataEventArgs> HandDataBack;

        public ComICCard(string _PortName, int _BaudRate)
        {
            try
            {
                port = new SerialPort();
                port.PortName = _PortName;
                port.BaudRate = _BaudRate;
                port.ReceivedBytesThreshold = 1000;
                port.Open();
                _keepreading = true;
            }
            catch (Exception e)
            {
                LogUtil.WriteLog("读卡器端口" + port.PortName + "打开失败！", e);
            }
        }

        public void Read()
        {
            while (_keepreading)
            {
                try
                {
                    string strData;
                    byte[] r_byte = new byte[20];
                    if (port.IsOpen)
                    {
                        port.Read(r_byte, 0, 8);

                        if (r_byte[0] != 0xBB)
                        {
                            strData = "";
                            for (int i = 2; i < 6; i++)
                            {
                                strData = strData + funBtoHex(r_byte[i]);
                            }
                            HandDataBack?.Invoke(this, new DataEventArgs(strData));
                            IsRead = false;
                        }
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
                catch (Exception ex) { LogUtil.WriteLog(ex); }
            }
        }

        #region 
        private string funBtoHex(byte num)
        {
            string strhex;
            strhex = num.ToString("X");
            if (strhex.Length == 1)
                strhex = " 0" + strhex;
            else
                strhex = " " + strhex;
            return strhex;

        }
        #endregion

        public bool IsOpen()
        {
            return port.IsOpen;
        }

        public void Close()
        {
            try
            {
                _keepreading = false;
                port.Close();
            }
            catch (Exception)
            {
            }
        }
    }
}
