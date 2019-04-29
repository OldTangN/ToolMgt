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
    public class DeptListViewModel : ViewModelBase
    {
        public DeptListViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.RefreshDeptList, RefreshList);
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
            if (!Global.HasRight("0302"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            Department dept = new Department();
            MessengerInstance.Send<object>(dept, MsgToken.OpenDeptInfo);
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
            if (!Global.HasRight("0302"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectedDept != null)
            {
                MessengerInstance.Send<object>(SelectedDept, MsgToken.OpenDeptInfo);
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
            if (!Global.HasRight("0303"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectedDept == null)
            {
                MessageAlert.Alert("请选择要删除的记录！");
                return;
            }
            var confirm = await MessageAlert.Confirm("确定删除此记录？");
            if (!confirm)
            {
                return;
            }
            DeptDao dao = new DeptDao();
            var rlt = dao.DeleteDepartment(SelectedDept.Id);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
            }
            RefreshList(null);
        }
        #endregion

        public Department SelectedDept { get => _selectedDept; set => Set(ref _selectedDept, value); }
        public List<Department> Depts { get => _depts; set => Set(ref _depts, value); }

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
            if (!Global.HasRight("0301"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            DeptDao dao = new DeptDao();
            var rlt = dao.GetDepartments();
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            Depts = rlt.Entities;

        }

        private List<Department> _depts;
        private Department _selectedDept;
        private RelayCommand _addCmd;
        private RelayCommand _editCmd;
        private RelayCommand _deleteCmd;
        private RelayCommand _refreshCmd;
    }
}
