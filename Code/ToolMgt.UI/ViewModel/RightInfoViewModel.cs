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
    public class RightInfoViewModel : ViewModelBase
    {

        public RightInfoViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.TransforRight, TransforRight);
        }

        private Right _currRight;

        public Right CurrRight { get => _currRight; set => Set(ref _currRight, value); }

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
            if (string.IsNullOrEmpty(CurrRight.Code) || string.IsNullOrEmpty(CurrRight.Name))
            {
                MessageAlert.Alert("编码、名称不能为空！");
            }
            RightDao dao = new RightDao();
            Result<bool> rlt;
            if (CurrRight.Id == 0)
            {
                rlt = dao.AddRight(CurrRight);
            }
            else
            {
                rlt = dao.EditRight(CurrRight);
            }
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            MessengerInstance.Send(true, MsgToken.CloseRightInfo);
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
            MessengerInstance.Send(false, MsgToken.CloseRightInfo);
        }
        #endregion

        private void TransforRight(object right)
        {
            CurrRight = ObjectCopier.DeepCopyByReflect(right as Right);
        }

        public override void Cleanup()
        {
            MessengerInstance.Unregister(this);
            base.Cleanup();
        }
    }
}
