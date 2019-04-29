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
    public class DutyInfoViewModel : ViewModelBase
    {

        public DutyInfoViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.TransforDuty, TransforDuty);
        }

        private Duty _currDuty;

        public Duty CurrDuty { get => _currDuty; set => Set(ref _currDuty, value); }

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
            if (string.IsNullOrEmpty(CurrDuty.Code) || string.IsNullOrEmpty(CurrDuty.Name))
            {
                MessageAlert.Alert("编码、名称不能为空！");
            }
            DutyDao dao = new DutyDao();
            Result<bool> rlt;
            if (CurrDuty.Id == 0)
            {
                rlt = dao.AddDuty(CurrDuty);
            }
            else
            {
                rlt = dao.EditDuty(CurrDuty);
            }
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            MessengerInstance.Send(true, MsgToken.CloseDutyInfo);
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
            MessengerInstance.Send(false, MsgToken.CloseDutyInfo);
        }
        #endregion

        private void TransforDuty(object duty)
        {
            CurrDuty = ObjectCopier.DeepCopyByReflect(duty as Duty);           
        }

        public override void Cleanup()
        {
            MessengerInstance.Unregister(this);
            base.Cleanup();
        }
    }
}
