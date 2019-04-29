using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolMgt.Model
{
    public partial class ToolPkgItem
    {
        private string _ChildBarcode;
        public string ChildBarcode { get => _ChildBarcode; set => Set(ref _ChildBarcode, value); }

        private string _ChildName;

        public string ChildName { get => _ChildName; set => Set(ref _ChildName, value); }

        private string _ChildSpec;
        public string ChildSpec { get => _ChildSpec; set => Set(ref _ChildSpec, value); }
    }
}
