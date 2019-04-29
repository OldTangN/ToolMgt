using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolMgt.Model
{
    /// <summary>
    /// 系统工具状态编码
    /// </summary>
    public class ToolStateCode
    {
        /// <summary>
        /// 正常 001
        /// </summary>
        public static readonly string Normal = "001";
        /// <summary>
        /// 使用中 002
        /// </summary>
        public static readonly string Using = "002";
        /// <summary>
        /// 损坏 003
        /// </summary>
        public static readonly string Damage = "003";
        /// <summary>
        /// 维修中 004
        /// </summary>
        public static readonly string Repair = "004";
    }
}
