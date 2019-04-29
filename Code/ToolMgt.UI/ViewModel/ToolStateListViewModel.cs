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
    public class ToolStateListViewModel : ViewModelBase
    {

        public ToolStateListViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.RefreshToolStateList, RefreshList);
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
            if (!Global.HasRight("0202"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            ToolState state = new ToolState();
            MessengerInstance.Send<object>(state, MsgToken.OpenToolStateInfo);
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
            if (!Global.HasRight("0202"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectedToolState != null)
            {
                MessengerInstance.Send<object>(SelectedToolState, MsgToken.OpenToolStateInfo);
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
            if (!Global.HasRight("0203"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectedToolState == null)
            {
                MessageAlert.Alert("请选择要删除的记录！");
                return;
            }
            var confirm = await MessageAlert.Confirm("确定删除此记录？");
            if (!confirm)
            {
                return;
            }
            ToolStateDao dao = new ToolStateDao();
            var rlt = dao.DeleteToolState(SelectedToolState.Id);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
            }
            RefreshList(null);
        }
        #endregion

        public ToolState SelectedToolState { get => _selectedToolState; set => Set(ref _selectedToolState, value); }
        public List<ToolState> ToolStates { get => _toolStates; set => Set(ref _toolStates, value); }

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
            if (!Global.HasRight("0201"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            ToolStates = new List<ToolState>();
            //return;
            ToolStateDao dao = new ToolStateDao();
            var rlt = dao.GetToolStates();
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            ToolStates = rlt.Entities;

        }

        private List<ToolState> _toolStates;
        private ToolState _selectedToolState;
        private RelayCommand _addCmd;
        private RelayCommand _editCmd;
        private RelayCommand _deleteCmd;
        private RelayCommand _refreshCmd;
    }
}
