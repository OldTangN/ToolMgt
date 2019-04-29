using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using ToolMgt.BLL;
using ToolMgt.Common;
using ToolMgt.Model;
using ToolMgt.UI.Common;

namespace ToolMgt.UI.ViewModel
{
    public class ToolCategoryListViewModel : ViewModelBase
    {

        public ToolCategoryListViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.RefreshToolCategoryList, RefreshList);
            RefreshList(null);
        }

        #region AddCmd
        public RelayCommand AddCmd
        {
            get
            {
                if (_addCmd == null)
                {
                    _addCmd = new RelayCommand(Add);
                }
                return _addCmd;
            }
        }

        private void Add()
        {
            if (!Global.HasRight("0102"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            ToolCategory toolCategory = new ToolCategory();
            MessengerInstance.Send<object>(toolCategory, MsgToken.OpenToolCategoryInfo);
        }
        #endregion

        #region EditCmd
        public RelayCommand EditCmd
        {
            get
            {
                if (_editCmd == null)
                {
                    _editCmd = new RelayCommand(Edit);
                }
                return _editCmd;
            }
        }

        private void Edit()
        {
            if (!Global.HasRight("0102"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectedToolCategory != null)
            {
                MessengerInstance.Send<object>(SelectedToolCategory, MsgToken.OpenToolCategoryInfo);
            }
            else
            {
                MessageAlert.Alert("请选择工具状态信息！");
            }
        }
        #endregion

        #region DeleteCmd
        public RelayCommand DeleteCmd
        {
            get
            {
                if (_deleteCmd == null)
                {
                    _deleteCmd = new RelayCommand(Delete);
                }
                return _deleteCmd;
            }
        }

        private async void Delete()
        {
            if (!Global.HasRight("0103"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectedToolCategory == null)
            {
                MessageAlert.Alert("请选择要删除的记录！");
                return;
            }
            var confirm = await MessageAlert.Confirm("确定删除此记录？");
            if (!confirm)
            {
                return;
            }
            ToolCategoryDao dao = new ToolCategoryDao();
            var rlt = dao.DeleteToolCategory(SelectedToolCategory.Id);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
            }
            RefreshList(null);
        }
        #endregion

        public ToolCategory SelectedToolCategory { get => _selectedToolCategory; set => Set(ref _selectedToolCategory, value); }
        public List<ToolCategory> ToolCategorys { get => _toolStates; set => Set(ref _toolStates, value); }

        #region RefreshCmd
        public RelayCommand RefreshCmd
        {
            get
            {
                if (_refreshCmd == null)
                {
                    _refreshCmd = new RelayCommand(Refresh);
                }
                return _refreshCmd;
            }
        }
        void Refresh()
        {
            RefreshList(null);
        }

        #endregion
        private void RefreshList(object obj)
        {
            if (!Global.HasRight("0101"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            ToolCategorys = new List<ToolCategory>();
            //return;
            ToolCategoryDao dao = new ToolCategoryDao();
            var rlt = dao.GetToolCategorys();
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            ToolCategorys = rlt.Entities;
        }

        private List<ToolCategory> _toolStates;
        private ToolCategory _selectedToolCategory;
        private RelayCommand _addCmd;
        private RelayCommand _editCmd;
        private RelayCommand _deleteCmd;
        private RelayCommand _refreshCmd;
    }
}
