using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolMgt.Model
{
    public partial class ToolPurchaseOrder
    {
        private string _OperatorName;
        public string OperatorName { get => _OperatorName; set => Set(ref _OperatorName, value); }

        private string _ApproveUserName;
        public string ApproveUserName { get => _ApproveUserName; set => Set(ref _ApproveUserName, value); }

        private List<ToolPurchaseOrderDtl> _OrderDtls = new List<ToolPurchaseOrderDtl>();
        public List<ToolPurchaseOrderDtl> OrderDtls { get => _OrderDtls; set => Set(ref _OrderDtls, value); }
    }
}
