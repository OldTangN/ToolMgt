using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using ToolMgt.BLL;
using ToolMgt.Model;
using ToolMgt.UI.Common;

namespace ToolMgt.UI.ViewModel
{
    public class DeptInfoViewModel : ViewModelBase
    {

        public DeptInfoViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.TransforDept, TransforDept);
        }

        private Department _currDept;

        public Department CurrDept { get => _currDept; set => Set(ref _currDept, value); }

        #region CommitCmd
        private RelayCommand _commitCmd;
        public RelayCommand CommitCmd
        {
            get
            {
                if (_commitCmd == null)
                {
                    _commitCmd = new RelayCommand(Commit);
                }
                return _commitCmd;
            }
        }

        private void Commit()
        {
            if (string.IsNullOrEmpty(CurrDept.Code) || string.IsNullOrEmpty(CurrDept.Name))
            {
                MessageAlert.Alert("编码、名称不能为空！");
            }
            DeptDao dao = new DeptDao();
            Result<bool> rlt;
            if (CurrDept.Id == 0)
            {
                rlt = dao.AddDepartment(CurrDept);
            }
            else
            {
                rlt = dao.EditDepartment(CurrDept);
            }
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            MessengerInstance.Send(true, MsgToken.CloseDeptInfo);
        }
        #endregion

        #region CancelCmd
        private RelayCommand _cancelCmd;
        public RelayCommand CancelCmd
        {
            get
            {
                if (_cancelCmd == null)
                {
                    _cancelCmd = new RelayCommand(Cancel);
                }
                return _cancelCmd;
            }
        }

        private void Cancel()
        {
            MessengerInstance.Send(false, MsgToken.CloseDeptInfo);
        }
        #endregion

        private void TransforDept(object dept)
        {
            CurrDept = ObjectCopier.DeepCopyByReflect(dept as Department);
        }

        public override void Cleanup()
        {
            MessengerInstance.Unregister(this);
            base.Cleanup();
        }
    }
}
