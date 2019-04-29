using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ToolMgt.BLL;
using ToolMgt.Model;
using ToolMgt.UI.Common;

namespace ToolMgt.UI.ViewModel
{
    public class RoleInfoViewModel : ViewModelBase
    {

        public RoleInfoViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.TransforRole, TransforRole);
        }

        private Role _currRole;

        public Role CurrRole { get => _currRole; set => Set(ref _currRole, value); }

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
            if (string.IsNullOrEmpty(CurrRole.Code) || string.IsNullOrEmpty(CurrRole.Name))
            {
                MessageAlert.Alert("编码、名称不能为空！");
            }
            RoleDao dao = new RoleDao();
            Result<bool> rlt;
            if (CurrRole.Id == 0)
            {
                rlt = dao.AddRole(CurrRole);
            }
            else
            {
                rlt = dao.EditRole(CurrRole);
            }
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            MessengerInstance.Send(true, MsgToken.CloseRoleInfo);
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
            MessengerInstance.Send(false, MsgToken.CloseRoleInfo);
        }
        #endregion

        private void TransforRole(object role)
        {
            CurrRole = ObjectCopier.DeepCopyByReflect(role as Role);
        }

        public override void Cleanup()
        {
            MessengerInstance.Unregister(this);
            base.Cleanup();
        }
    }
}
