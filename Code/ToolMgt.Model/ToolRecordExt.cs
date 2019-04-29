using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolMgt.Model
{
    public partial class ToolRecord
    {
        private string _BorrowOperatorName;
        private string _ReturnOperatorName;
        private string _BorrowerName;
        public string BorrowOperatorName { get => _BorrowOperatorName; set => Set(ref _BorrowOperatorName, value); }
        public string ReturnOperatorName { get => _ReturnOperatorName; set => Set(ref _ReturnOperatorName, value); }
        public string BorrowerName { get => _BorrowerName; set => Set(ref _BorrowerName, value); }

        private string _ReturnStateCode;
        public string ReturnStateCode { get => _ReturnStateCode; set => Set(ref _ReturnStateCode, value); }

        private string _ReturnStateName;
        public string ReturnStateName { get => _ReturnStateName; set => Set(ref _ReturnStateName, value); }
    }
}
