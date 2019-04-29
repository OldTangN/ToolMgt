using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolMgt.Model
{
    public partial class ToolRepair
    {
        private string _ToolBarcode;
        public string ToolBarcode { get => _ToolBarcode; set => Set(ref _ToolBarcode, value); }

        private string _ToolName;
        public string ToolName { get => _ToolName; set => Set(ref _ToolName, value); }

        private string _OperatorName;
        public string OperatorName { get => _OperatorName; set => Set(ref _OperatorName, value); }

        private string _ApproveUserName;
        public string ApproveUserName { get => _ApproveUserName; set => Set(ref _ApproveUserName, value); }

    }
}
