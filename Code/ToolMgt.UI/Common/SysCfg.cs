﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Common;

namespace ToolMgt.UI.Common
{
    public class SysCfg
    {
        /// <summary>
        /// IC读卡器类型 COM、USB，默认USB
        /// </summary>
        public static string ICReaderType => ConfigurationUtil.GetConfiguration(Convert.ToString, () => "USB");

        /// <summary>
        /// IC读卡器类型端口号，默认-1
        /// </summary>
        public static int ICReaderPort => ConfigurationUtil.GetConfiguration(int.Parse, () => -1);

        /// <summary>
        /// IC读卡器波特率，默认-1
        /// </summary>
        public static int ICReaderBaudRate => ConfigurationUtil.GetConfiguration(int.Parse, () => -1);

        /// <summary>
        /// RFIDPort读卡器端口号，默认-1
        /// </summary>
        public static int RFIDPort => ConfigurationUtil.GetConfiguration(int.Parse, () => -1);

        /// <summary>
        /// RFIDBaudRate读卡器波特率，默认-1
        /// </summary>
        public static int RFIDBaudRate => ConfigurationUtil.GetConfiguration(int.Parse, () => -1);

        public static string ImageDir => ConfigurationUtil.GetConfiguration(Convert.ToString, () => "");
    }
}
