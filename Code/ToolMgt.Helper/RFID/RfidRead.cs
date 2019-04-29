using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using ReaderB;
using ToolMgt.Common;

namespace ToolMgt.Helper.RFID
{
    public class RfidRead : IRFIDHelper
    {
        private bool _keepreading;
        private int fCmdRet = 30; //所有执行指令的返回值
        private int rfidcomindex = 0;
        private bool IsConnected = false;
        public event EventHandler<DataEventArgs> HandDataBack;
        public RfidRead(int _rfidcomindex)
        {
            try
            {
                rfidcomindex = _rfidcomindex;
                Openport();
            }
            catch
            {
                LogUtil.WriteLog("校验仪端口" + rfidcomindex + "打开失败！");
            }
        }

        private bool Openport()
        {
            try
            {
                byte fComAdr = Convert.ToByte("FF", 16); // $FF;
                int port = 0, i;
                for (i = 6; i >= 0; i--)
                {
                    byte fBaud = Convert.ToByte(i);
                    int openresult = StaticClassReaderB.OpenComPort(rfidcomindex, ref fComAdr, fBaud, ref port);
                    if (openresult == 0)
                    {
                        _keepreading = true;
                        IsConnected = true;
                        return true;
                    }
                }
                return false;
            }
            catch { return false; }
        }

        private void ClosePort()
        {
            _keepreading = false;
            StaticClassReaderB.CloseSpecComPort(rfidcomindex);
        }

        private StringBuilder sb = new StringBuilder();
        public void Read()
        {
            byte[] ReceiveBytes = new byte[4096];
            while (true)
            {
                try
                {
                    if (_keepreading)
                    {
                        byte fComAdr = 0;
                        byte AdrTID = 0;
                        byte LenTID = 0;
                        byte TIDFlag = 0;
                        int Totallen = 0;
                        int CardNum = 0;
                        int EPClen = 0;
                        int m = 0;
                        string temps, sEPC;
                        byte[] EPC = new byte[5000];
                        fCmdRet = StaticClassReaderB.Inventory_G2(ref fComAdr, AdrTID, LenTID, TIDFlag, EPC, ref Totallen, ref CardNum, rfidcomindex);
                        if ((fCmdRet == 1) | (fCmdRet == 2) | (fCmdRet == 3) | (fCmdRet == 4) | (fCmdRet == 0xFB))//代表已查找结束，
                        {
                            if (CardNum != 0)
                            {
                                byte[] daw = new byte[Totallen];
                                Array.Copy(EPC, daw, Totallen);
                                temps = ByteArrayToHexString(daw);
                                EPClen = daw[m];
                                sEPC = temps.Substring(m * 2 + 2, EPClen * 2);
                                HandDataBack?.Invoke(this, new DataEventArgs(sEPC));
                            }
                        }
                    }
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                }
            }
        }

        private string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            return sb.ToString().ToUpper();
        }

        public bool IsOpen()
        {
            return IsConnected;
        }

        public void Close()
        {
            ClosePort();
        }
    }
}
