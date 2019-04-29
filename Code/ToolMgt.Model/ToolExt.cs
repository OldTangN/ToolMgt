using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolMgt.Model
{
    public partial class Tool
    {
        public static readonly string NoBarcode = "无编码";

        private List<ToolPkgItem> _PkgItems = new List<ToolPkgItem>();
        public List<ToolPkgItem> PkgItems { get => _PkgItems; set => Set(ref _PkgItems, value); }
    }
}
