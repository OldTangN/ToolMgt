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
    public class UserListViewModel : ViewModelBase
    {
        public UserListViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.RefreshUserList, RefreshList);           
            RefreshList(null);

            DeptDao deptDao = new DeptDao();
            var deptrlt = deptDao.GetDepartments();
            if (deptrlt.HasError)
            {
                MessageAlert.Alert(deptrlt.Msg);
            }
            //RoleDao roleDao = new RoleDao();
            //var rolerlt = roleDao.GetRoles();
            //if (rolerlt.HasError)
            //{
            //    MessageAlert.Alert(rolerlt.Msg);
            //}
            //DutyDao dutyDao = new DutyDao();
            //var dutyrlt = dutyDao.GetDuties();
            //if (dutyrlt.HasError)
            //{
            //    MessageAlert.Alert(dutyrlt.Msg);
            //}

            Department deptAll = new Department()
            {
                Id = 0,
                Name = "全部"
            };
            deptrlt.Entities.Insert(0, deptAll);
            Departments = deptrlt.Entities;

            //Roles = rolerlt.Entities;
            //Duties = dutyrlt.Entities;
            if (App.ICCardReader.IsOpen())
            {
                App.ICCardReader.HandDataBack += ICCardReader_HandDataBack;
            }
        }

        private void ICCardReader_HandDataBack(object sender, Helper.DataEventArgs e)
        {
            SearchCard = e.Data;
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
            if (!Global.HasRight("0802"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            User user = new User();
            App.ICCardReader.HandDataBack -= ICCardReader_HandDataBack;
            MessengerInstance.Send<object>(user, MsgToken.OpenUserInfo);
            App.ICCardReader.HandDataBack += ICCardReader_HandDataBack;
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
            if (!Global.HasRight("0802"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectedUser != null)
            {
                App.ICCardReader.HandDataBack -= ICCardReader_HandDataBack;
                MessengerInstance.Send<object>(SelectedUser, MsgToken.OpenUserInfo);
                App.ICCardReader.HandDataBack += ICCardReader_HandDataBack;
            }
            else
            {
                MessageAlert.Alert("请选择用户信息！");
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
            if (!Global.HasRight("0803"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectedUser == null)
            {
                MessageAlert.Alert("请选择要删除的记录！");
                return;
            }
            var confirm = await MessageAlert.Confirm("确定删除此记录？");
            if (!confirm)
            {
                return;
            }
            UserDao dao = new UserDao();
            var rlt = dao.DeleteUser(SelectedUser.Id);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
            }
            RefreshList(null);
        }
        #endregion

        #region SearchCmd

        #endregion

        public List<Department> Departments { get => _departments; set => Set(ref _departments, value); }
        //public List<Role> Roles { get => _roles; set => Set(ref _roles, value); }
        //public List<Duty> Duties { get => _duties; set => Set(ref _duties, value); }
        public int SelectedDeptId { get; set; }
        //public int SelectedDutyId { get; set; }
        //public int SelectedRoleId { get; set; }
        public string SearchCard { get => _searchCard; set => Set(ref _searchCard, value); }
        public string SearchName { get => _searchName; set => Set(ref _searchName, value); }
        public User SelectedUser { get => _selectedUser; set => Set(ref _selectedUser, value); }
        public List<User> Users { get => _toolStates; set => Set(ref _toolStates, value); }

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
            if (!Global.HasRight("0801"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            Users = new List<User>();
            //return;
            UserDao dao = new UserDao();
            var rlt = dao.GetUsers(SearchName, SearchCard, SelectedDeptId);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            Users = rlt.Entities;
        }

        public override void Cleanup()
        {
            MessengerInstance.Unregister(this);
            App.ICCardReader.HandDataBack -= ICCardReader_HandDataBack;
            base.Cleanup();
        }

        private string _searchName = "";
        private string _searchCard = "";

        private List<User> _toolStates;
        private User _selectedUser;
        private RelayCommand _addCmd;
        private RelayCommand _editCmd;
        private RelayCommand _deleteCmd;
        private RelayCommand _refreshCmd;

        private List<Duty> _duties;
        private List<Department> _departments;
        private List<Role> _roles;
    }
}
