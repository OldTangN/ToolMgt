using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolMgt.Model
{
    public partial class ToolDamage
    {
        private string _ToolBarcode;
        public string ToolBarcode { get => _ToolBarcode; set => Set(ref _ToolBarcode, value); }

        private string _ToolName;
        public string ToolName { get => _ToolName; set => Set(ref _ToolName, value); }

        private string _StateName;
        public string StateName { get => _StateName; set => Set(ref _StateName, value); }

        private string _DutyUserCardNo;
        public string DutyUserCardNo { get => _DutyUserCardNo; set => Set(ref _DutyUserCardNo, value); }

        private string _DutyUserName;
        public string DutyUserName { get => _DutyUserName; set => Set(ref _DutyUserName, value); }

        private string _OperatorName;
        public string OperatorName { get => _OperatorName; set => Set(ref _OperatorName, value); }
    }
}
