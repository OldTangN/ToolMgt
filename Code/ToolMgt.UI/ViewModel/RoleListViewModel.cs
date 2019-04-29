using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ToolMgt.BLL;
using ToolMgt.Model;
using ToolMgt.UI.Common;

namespace ToolMgt.UI.ViewModel
{
    public class RoleListViewModel : ViewModelBase
    {
        public RoleListViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.RefreshRoleList, RefreshList);
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
            if (!Global.HasRight("0502"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            Role role = new Role();
            MessengerInstance.Send<object>(role, MsgToken.OpenRoleInfo);
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
            if (!Global.HasRight("0502"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectedRole != null)
            {
                MessengerInstance.Send<object>(SelectedRole, MsgToken.OpenRoleInfo);
            }
            else
            {
                MessageAlert.Alert("请选择角色信息！");
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
            if (!Global.HasRight("0503"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectedRole == null)
            {
                MessageAlert.Alert("请选择要删除的记录！");
                return;
            }
            var confirm = await MessageAlert.Confirm("确定删除此记录？");
            if (!confirm)
            {
                return;
            }
            RoleDao dao = new RoleDao();
            var rlt = dao.DeleteRole(SelectedRole.Id);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
            }
            RefreshList(null);
        }
        #endregion

        public Role SelectedRole { get => _selectedRole; set => Set(ref _selectedRole, value); }
        public List<Role> Roles { get => _roles; set => Set(ref _roles, value); }

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

        private void Refresh()
        {
            RefreshList(null);
        }

        #endregion

        private void RefreshList(object obj)
        {
            if (!Global.HasRight("0501"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            RoleDao dao = new RoleDao();
            var rlt = dao.GetRoles();
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            Roles = rlt.Entities;
        }

        private List<Role> _roles;
        private Role _selectedRole;
        private RelayCommand _addCmd;
        private RelayCommand _editCmd;
        private RelayCommand _deleteCmd;
        private RelayCommand _refreshCmd;
    }
}
