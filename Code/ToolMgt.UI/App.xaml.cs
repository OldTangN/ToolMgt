using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ToolMgt.Common;
using ToolMgt.Helper.ICCard;
using ToolMgt.Helper.RFID;
using ToolMgt.UI.Common;

namespace ToolMgt.UI
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private Thread ThreadICReader;
        private Thread ThreadRFIDReader;
        public App()
        {
            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            string readerType = SysCfg.ICReaderType;
            if (string.IsNullOrEmpty(readerType))
            {
                return;
            }
            int readerPort = SysCfg.ICReaderPort;
            int readerBaud = SysCfg.ICReaderBaudRate;
            if (readerBaud == -1 || readerPort == -1)
            {
                //MessageAlert.Alert("IC读卡器配置错误！");
                return;
            }
            if (App.ICCardReader == null)
            {
                if (SysCfg.ICReaderType == "USB")
                {
                    App.ICCardReader = new UsbICCard(SysCfg.ICReaderPort, SysCfg.ICReaderBaudRate);
                }
                else
                {
                    App.ICCardReader = new ComICCard("COM" + SysCfg.ICReaderPort, SysCfg.ICReaderBaudRate);
                }
                if (App.ICCardReader.IsOpen())
                {
                    ThreadICReader = new Thread(new ParameterizedThreadStart(ICReadThread));
                    ThreadICReader.IsBackground = true;
                    ThreadICReader.Start(App.ICCardReader);
                }

                int rfidPort = SysCfg.RFIDPort;
                int rfidBaudRate = SysCfg.RFIDBaudRate;
                if (rfidPort == -1 || rfidBaudRate == -1)
                {
                    //MessageAlert.Alert("RFID读卡器配置错误！");
                    return;
                }
                //App.RFIDReader = new RfidRead(rfidPort);
                App.RFIDReader = new RFIDSimulation(rfidPort, rfidBaudRate);
                if (App.RFIDReader.IsOpen())
                {
                    ThreadRFIDReader = new Thread(new ParameterizedThreadStart(RFIDReadThread));
                    ThreadRFIDReader.IsBackground = true;
                    ThreadRFIDReader.Start(RFIDReader);
                }
            }
        }

        private void ICReadThread(object obj)
        {
            ICardHelper card = obj as ICardHelper;
            card?.Read();
        }
        private  void RFIDReadThread(object obj)
        {
            IRFIDHelper card = obj as IRFIDHelper;
            card?.Read();
        }
        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            LogUtil.WriteLog(e.Exception);
            MessageBox.Show(e.Exception.Message);
        }

        public static ICardHelper ICCardReader;
        public static IRFIDHelper RFIDReader;
    }
}
