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
    public class ToolStateInfoViewModel : ViewModelBase
    {

        public ToolStateInfoViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.TransforToolState, TransforToolState);
        }

        private ToolState _currToolState;

        public ToolState CurrToolState { get => _currToolState; set => Set(ref _currToolState, value); }

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
            if (string.IsNullOrEmpty(CurrToolState.Code) || string.IsNullOrEmpty(CurrToolState.Name))
            {
                MessageAlert.Alert("编码、名称不能为空！");
            }
            ToolStateDao dao = new ToolStateDao();
            Result<bool> rlt;
            if (CurrToolState.Id == 0)
            {
                rlt = dao.AddToolState(CurrToolState);
            }
            else
            {
                rlt = dao.EditToolState(CurrToolState);
            }
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            MessengerInstance.Send(true, MsgToken.CloseToolStateInfo);
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
            MessengerInstance.Send(false, MsgToken.CloseToolStateInfo);
        }
        #endregion

        private void TransforToolState(object state)
        {
            CurrToolState = ObjectCopier.DeepCopyByReflect(state as ToolState);           
        }

        public override void Cleanup()
        {
            MessengerInstance.Unregister(this);
            base.Cleanup();
        }
    }
}
