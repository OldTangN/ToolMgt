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
    public class ToolDamageInfoViewModel : ViewModelBase
    {

        public ToolDamageInfoViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.TransforToolDamage, TransforToolDamage);
            MessengerInstance.Register<User>(this, MsgToken.TransforUser, TransforUser);
            MessengerInstance.Register<Tool>(this, MsgToken.TransforTool, TransforTool);
            App.RFIDReader.HandDataBack += RFIDReader_HandDataBack;
        }

        private void RFIDReader_HandDataBack(object sender, Helper.DataEventArgs e)
        {
            string[] rfids = e.Data?.Split(';');
            if (rfids != null && rfids.Length > 0)
            {
                ToolDao toolDao = new ToolDao();
                var rlt = toolDao.GetToolByRFID(rfids[0]);
                if (rlt.HasError)
                {
                    MessageAlert.Alert(rlt.Msg);
                    return;
                }
                if (rlt.Entities != null)
                {
                    TransforTool(rlt.Entities);
                }
            }
        }

        private void TransforUser(User user)
        {
            CurrToolDamage.DutyUserId = user.Id;
            CurrToolDamage.DutyUserCardNo = user.CardNo;
            CurrToolDamage.DutyUserName = user.Name;
        }
        private void TransforTool(Tool tool)
        {
            CurrToolDamage.ToolId = tool.Id;
            CurrToolDamage.ToolBarcode = tool.Barcode;
            CurrToolDamage.ToolName = tool.Name;
        }

        private ToolDamage _currToolDamage;

        public ToolDamage CurrToolDamage { get => _currToolDamage; set => Set(ref _currToolDamage, value); }

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
            ToolDamageDao dao = new ToolDamageDao();
            Result<bool> rlt;
            if (CurrToolDamage.Id == 0)
            {
                rlt = dao.AddToolDamage(CurrToolDamage);
            }
            else
            {
                rlt = dao.EditToolDamage(CurrToolDamage);
            }
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            MessengerInstance.Send(true, MsgToken.CloseToolDamageInfo);
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
            MessengerInstance.Send(false, MsgToken.CloseToolDamageInfo);
        }
        #endregion

        #region SelectUserCmd
        private RelayCommand _SelectUserCmd;
        public RelayCommand SelectUserCmd
        {
            get
            {
                if (_SelectUserCmd == null)
                {
                    _SelectUserCmd = new RelayCommand(SelectUser);
                }
                return _SelectUserCmd;
            }
        }
        private void SelectUser()
        {
            //打开用户列表选择
            MessengerInstance.Send<object>(null, MsgToken.OpenSelectUser);
        }
        #endregion

        #region SelectToolCmd
        private RelayCommand _SelectToolCmd;
        public RelayCommand SelectToolCmd
        {
            get
            {
                if (_SelectToolCmd == null)
                {
                    _SelectToolCmd = new RelayCommand(SelectTool);
                }
                return _SelectToolCmd;
            }
        }
        private void SelectTool()
        {
            //打开用户列表选择
            MessengerInstance.Send<object>(null, MsgToken.OpenSelectTool);
        }
        #endregion

        private void TransforToolDamage(object dmg)
        {
            ToolStateDao stateDao = new ToolStateDao();
            var staterlt = stateDao.GetToolStates();
            if (staterlt.HasError)
            {
                MessageAlert.Alert(staterlt.Msg);
            }
            States = staterlt.Entities;

            CurrToolDamage = ObjectCopier.DeepCopyByReflect(dmg as ToolDamage);
            CurrToolDamage.OperatorId = Global.CurrUser.Id;
            CurrToolDamage.OperatorName = Global.CurrUser.Name;
            if (CurrToolDamage.Id == 0)
            {
                CurrToolDamage.DamageDate = DateTime.Now;
            }
        }

        public override void Cleanup()
        {
            MessengerInstance.Unregister(this);
            App.RFIDReader.HandDataBack += RFIDReader_HandDataBack;
            base.Cleanup();
        }

        private List<ToolState> _states;
        public List<ToolState> States { get => _states; set => Set(ref _states, value); }
    }
}
