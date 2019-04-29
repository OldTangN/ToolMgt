using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using ToolMgt.BLL;
using ToolMgt.Helper;
using ToolMgt.Model;
using ToolMgt.UI.Common;

namespace ToolMgt.UI.ViewModel
{
    public class ToolRepairInfoViewModel : ViewModelBase
    {

        public ToolRepairInfoViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.TransforToolRepair, TransforToolRepair);
            MessengerInstance.Register<Tool>(this, MsgToken.TransforTool, TransforTool);
            App.RFIDReader.HandDataBack += RFIDReader_HandDataBack;
        }

        private void RFIDReader_HandDataBack(object sender, DataEventArgs e)
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

        private void TransforTool(Tool tool)
        {
            CurrToolRepair.ToolId = tool.Id;
            CurrToolRepair.ToolBarcode = tool.Barcode;
            CurrToolRepair.ToolName = tool.Name;
        }

        private ToolRepair _currToolRepair;

        public ToolRepair CurrToolRepair { get => _currToolRepair; set => Set(ref _currToolRepair, value); }

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
            ToolRepairDao dao = new ToolRepairDao();
            Result<bool> rlt;
            if (CurrToolRepair.Id == 0)
            {
                CurrToolRepair.CreateTime = DateTime.Now;
                rlt = dao.AddToolRepair(CurrToolRepair);
            }
            else
            {
                rlt = dao.EditToolRepair(CurrToolRepair);
            }
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            MessengerInstance.Send(true, MsgToken.CloseToolRepairInfo);
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
            MessengerInstance.Send(false, MsgToken.CloseToolRepairInfo);
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

        #region ApproveCmd
        private RelayCommand _ApproveCmd;
        public RelayCommand ApproveCmd
        {
            get
            {
                if (_ApproveCmd == null)
                {
                    _ApproveCmd = new RelayCommand(Approve);
                }
                return _ApproveCmd;
            }
        }

        private void Approve()
        {
            ToolRepairDao dao = new ToolRepairDao();
            CurrToolRepair.ApproveDate = DateTime.Now;
            CurrToolRepair.ApproveUserId = Global.CurrUser.Id;
            CurrToolRepair.ApproveUserName = Global.CurrUser.Name;
            var rlt = dao.ApproveToolRepair(CurrToolRepair);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            MessengerInstance.Send(true, MsgToken.CloseToolRepairInfo);
        }

        #endregion

        private void TransforToolRepair(object obj)
        {
            CurrToolRepair = ObjectCopier.DeepCopyByReflect(obj as ToolRepair);
            CurrToolRepair.OperatorId = Global.CurrUser.Id;
            CurrToolRepair.OperatorName = Global.CurrUser.Name;
        }

        public override void Cleanup()
        {
            MessengerInstance.Unregister(this);
            App.RFIDReader.HandDataBack -= RFIDReader_HandDataBack;
            base.Cleanup();
        }
    }
}
