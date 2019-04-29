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
    public class ToolRepairListViewModel : ViewModelBase
    {

        public ToolRepairListViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.RefreshToolRepairList, RefreshList);
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
            if (!Global.HasRight("1202"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            ToolRepair dmg = new ToolRepair();
            MessengerInstance.Send<object>(dmg, MsgToken.OpenToolRepairInfo);
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
            if (!Global.HasRight("1202"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectedToolRepair != null)
            {
                MessengerInstance.Send<object>(SelectedToolRepair, MsgToken.OpenToolRepairInfo);
            }
            else
            {
                MessageAlert.Alert("请选择工具损坏信息！");
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
            if (!Global.HasRight("1203"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectedToolRepair == null)
            {
                MessageAlert.Alert("请选择要删除的记录！");
                return;
            }
            var confirm = await MessageAlert.Confirm("确定删除此记录？");
            if (!confirm)
            {
                return;
            }
            ToolRepairDao dao = new ToolRepairDao();
            var rlt = dao.DeleteToolRepair(SelectedToolRepair.Id);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
            }
            RefreshList(null);
        }
        #endregion

        public ToolRepair SelectedToolRepair { get => _selectedToolRepair; set => Set(ref _selectedToolRepair, value); }
        public List<ToolRepair> ToolRepairs { get => _ToolRepairs; set => Set(ref _ToolRepairs, value); }

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
            if (!Global.HasRight("1201"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            ToolRepairs = new List<ToolRepair>();
            //return;
            ToolRepairDao dao = new ToolRepairDao();
            var rlt = dao.GetToolRepairs();
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            ToolRepairs = rlt.Entities;
        }

        private List<ToolRepair> _ToolRepairs;
        private ToolRepair _selectedToolRepair;
        private RelayCommand _addCmd;
        private RelayCommand _editCmd;
        private RelayCommand _deleteCmd;
        private RelayCommand _refreshCmd;
    }
}
