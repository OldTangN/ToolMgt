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
    public class ToolDamageListViewModel : ViewModelBase
    {
        public ToolDamageListViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.RefreshToolDamageList, RefreshList);
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
            if (!Global.HasRight("1102"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            ToolDamage dmg = new ToolDamage();
            MessengerInstance.Send<object>(dmg, MsgToken.OpenToolDamageInfo);
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
            if (!Global.HasRight("1102"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectedToolDamage != null)
            {
                MessengerInstance.Send<object>(SelectedToolDamage, MsgToken.OpenToolDamageInfo);
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
            if (!Global.HasRight("1103"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectedToolDamage == null)
            {
                MessageAlert.Alert("请选择要删除的记录！");
                return;
            }
            var confirm = await MessageAlert.Confirm("确定删除此记录？");
            if (!confirm)
            {
                return;
            }
            ToolDamageDao dao = new ToolDamageDao();
            var rlt = dao.DeleteToolDamage(SelectedToolDamage.Id);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
            }
            RefreshList(null);
        }
        #endregion

        public ToolDamage SelectedToolDamage { get => _selectedToolDamage; set => Set(ref _selectedToolDamage, value); }
        public List<ToolDamage> ToolDamages { get => _ToolDamages; set => Set(ref _ToolDamages, value); }

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
            if (!Global.HasRight("1101"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            ToolDamages = new List<ToolDamage>();
            //return;
            ToolDamageDao dao = new ToolDamageDao();
            var rlt = dao.GetToolDamages();
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            ToolDamages = rlt.Entities;
        }

        private List<ToolDamage> _ToolDamages;
        private ToolDamage _selectedToolDamage;
        private RelayCommand _addCmd;
        private RelayCommand _editCmd;
        private RelayCommand _deleteCmd;
        private RelayCommand _refreshCmd;
    }
}
