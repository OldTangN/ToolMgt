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
    public class DutyListViewModel : ViewModelBase
    {

        public DutyListViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.RefreshDutyList, RefreshList);
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
            if (!Global.HasRight("0402"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            Duty duty = new Duty();
            MessengerInstance.Send<object>(duty, MsgToken.OpenDutyInfo);
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
            if (!Global.HasRight("0402"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectedDuty != null)
            {
                MessengerInstance.Send<object>(SelectedDuty, MsgToken.OpenDutyInfo);
            }
            else
            {
                MessageAlert.Alert("请选择部门信息！");
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
            if (!Global.HasRight("0403"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectedDuty == null)
            {
                MessageAlert.Alert("请选择要删除的记录！");
                return;
            }
            var confirm = await MessageAlert.Confirm("确定删除此记录？");
            if (!confirm)
            {
                return;
            }
            DutyDao dao = new DutyDao();
            var rlt = dao.DeleteDuty(SelectedDuty.Id);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
            }
            RefreshList(null);
        }
        #endregion

        public Duty SelectedDuty { get => _selectedDuty; set => Set(ref _selectedDuty, value); }
        public List<Duty> Duties { get => _duties; set => Set(ref _duties, value); }

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
            if (!Global.HasRight("0401"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            Duties = new List<Duty>();
            //return;
            DutyDao dao = new DutyDao();
            var rlt = dao.GetDuties();
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            Duties = rlt.Entities;

        }

        private List<Duty> _duties;
        private Duty _selectedDuty;
        private RelayCommand _addCmd;
        private RelayCommand _editCmd;
        private RelayCommand _deleteCmd;
        private RelayCommand _refreshCmd;
    }
}
