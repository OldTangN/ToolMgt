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
    public class ToolCategoryInfoViewModel : ViewModelBase
    {

        public ToolCategoryInfoViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.TransforToolCategory, TransforToolCategory);
        }

        private ToolCategory _currToolCategory;

        public ToolCategory CurrToolCategory { get => _currToolCategory; set => Set(ref _currToolCategory, value); }

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
            if (string.IsNullOrEmpty(CurrToolCategory.Code) || string.IsNullOrEmpty(CurrToolCategory.Name))
            {
                MessageAlert.Alert("编码、名称不能为空！");
            }
            ToolCategoryDao dao = new ToolCategoryDao();
            Result<bool> rlt;
            if (CurrToolCategory.Id == 0)
            {
                rlt = dao.AddToolCategory(CurrToolCategory);
            }
            else
            {
                rlt = dao.EditToolCategory(CurrToolCategory);
            }
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            MessengerInstance.Send(true, MsgToken.CloseToolCategoryInfo);
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
            MessengerInstance.Send(false, MsgToken.CloseToolCategoryInfo);
        }
        #endregion

        private void TransforToolCategory(object obj)
        {
            CurrToolCategory = ObjectCopier.DeepCopyByReflect(obj as ToolCategory);           
        }

        public override void Cleanup()
        {
            MessengerInstance.Unregister(this);
            base.Cleanup();
        }
    }
}
