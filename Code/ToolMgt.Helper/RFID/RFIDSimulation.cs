using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Common;

namespace ToolMgt.Helper.RFID
{
    public class RFIDSimulation : IRFIDHelper
    {
        public event EventHandler<DataEventArgs> HandDataBack;

        private bool _KeepRead;

        private SerialPort _SerialPort;
        private bool IsConnected = false;
        public RFIDSimulation(int comNum, int baudRate)
        {
            try
            {
                _SerialPort = new SerialPort("COM" + comNum, baudRate);
                _SerialPort.Parity = Parity.None;
                _SerialPort.DataBits = 8;
                _SerialPort.DataReceived += _SerialPort_DataReceived;
                _SerialPort.Open();
                IsConnected = true;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                IsConnected = false;
            }
        }

        private void _SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int dataLength = _SerialPort.BytesToRead;
            byte[] buffer = new byte[dataLength];
            _SerialPort.Read(buffer, 0, dataLength);
            //LogUtil.WriteLog(FrameHelper.bytetostr(buffer));
            string strData = Encoding.ASCII.GetString(buffer);
            HandDataBack?.Invoke(this, new DataEventArgs(strData));
        }

        public void Close()
        {
            try
            {
                _SerialPort?.Close();
            }
            catch (Exception)
            {
            }
        }

        public bool IsOpen()
        {
            return IsConnected;
        }

        public void Read()
        {
            while (_KeepRead)
            {
                if (!IsConnected)
                {
                    break;
                }
                try
                {

                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                }
            }
        }
    }
}
