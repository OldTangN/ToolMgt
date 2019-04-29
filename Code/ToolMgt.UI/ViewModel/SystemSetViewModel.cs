using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ToolMgt.Common;
using ToolMgt.UI.Common;

namespace ToolMgt.UI.ViewModel
{
    public class SystemSetViewModel : ViewModelBase
    {
        public SystemSetViewModel()
        {
            this.ICReaderType = SysCfg.ICReaderType;
            this.ICReaderBaudRate = SysCfg.ICReaderBaudRate;
            this.ICReaderPort = SysCfg.ICReaderPort;
            this.RFIDBaudRate = SysCfg.RFIDBaudRate;
            this.RFIDPort = SysCfg.RFIDPort;
            this.ImageDir = SysCfg.ImageDir;
        }
        public string ImageDir { get; set; }
        /// <summary>
        /// IC读卡器类型 COM、USB，默认USB
        /// </summary>
        public string ICReaderType { get; set; }
        /// <summary>
        /// IC读卡器类型端口号，默认-1
        /// </summary>
        public int ICReaderPort { get; set; }

        /// <summary>
        /// IC读卡器波特率，默认-1
        /// </summary>
        public int ICReaderBaudRate { get; set; }

        /// <summary>
        /// RFIDPort读卡器端口号，默认-1
        /// </summary>
        public int RFIDPort { get; set; }

        /// <summary>
        /// RFIDBaudRate读卡器波特率，默认-1
        /// </summary>
        public int RFIDBaudRate { get; set; }

        private RelayCommand _CommitCmd;
        public RelayCommand CommitCmd
        {
            get
            {
                if (_CommitCmd == null)
                {
                    _CommitCmd = new RelayCommand(Commit);
                }
                return _CommitCmd;
            }
        }
        void Commit()
        {
            if (!Global.HasRight("0702"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            ConfigurationUtil.SetConfiguration(nameof(this.ICReaderType), this.ICReaderType);
            ConfigurationUtil.SetConfiguration(nameof(this.ICReaderPort), this.ICReaderPort.ToString());
            ConfigurationUtil.SetConfiguration(nameof(this.ICReaderBaudRate), this.ICReaderBaudRate.ToString());
            ConfigurationUtil.SetConfiguration(nameof(this.RFIDPort), this.RFIDPort.ToString());
            ConfigurationUtil.SetConfiguration(nameof(this.RFIDBaudRate), this.RFIDBaudRate.ToString());
            ConfigurationUtil.SetConfiguration(nameof(this.ImageDir), this.ImageDir); 
            MessengerInstance.Send<bool>(false, MsgToken.CloseSystemSet);
        }

        private RelayCommand _CancelCmd;
        public RelayCommand CancelCmd
        {
            get
            {
                if (_CancelCmd == null)
                {
                    _CancelCmd = new RelayCommand(Cancel);
                }
                return _CancelCmd;
            }
        }
        void Cancel()
        {
            MessengerInstance.Send<bool>(false, MsgToken.CloseSystemSet);
        }
    }
}
