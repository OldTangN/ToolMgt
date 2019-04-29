using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolMgt.Model
{
    public partial class User
    {
        private string _DutyName;
        public string DutyName { get => _DutyName; set => Set(ref _DutyName, value); }

        private string _DeptName;
        public string DeptName { get => _DeptName; set => Set(ref _DeptName, value); }

        private string _RoleName;
        public string RoleName { get => _RoleName; set => Set(ref _RoleName, value); }
    }
}
