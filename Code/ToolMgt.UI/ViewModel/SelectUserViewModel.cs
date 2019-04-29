using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ToolMgt.BLL;
using ToolMgt.Common;
using ToolMgt.Model;
using ToolMgt.UI.Common;

namespace ToolMgt.UI.ViewModel
{
    public class SelectUserViewModel : ViewModelBase
    {
        public SelectUserViewModel()
        {
            DeptDao deptDao = new DeptDao();
            var deptrlt = deptDao.GetDepartments();
            if (deptrlt.HasError)
            {
                MessageAlert.Alert(deptrlt.Msg);
                return;
            }
            Department deptAll = new Department()
            {
                Id = 0,
                Name = "全部"
            };
            deptrlt.Entities.Insert(0, deptAll);
            Departments = deptrlt.Entities;
            Search();
        }

        #region SearchCmd
        private RelayCommand _SearchCmd;
        public RelayCommand SearchCmd
        {
            get
            {
                if (_SearchCmd == null)
                {
                    _SearchCmd = new RelayCommand(Search);
                }
                return _SearchCmd;
            }
        }

        private void Search()
        {
            Users = new List<User>();
            UserDao dao = new UserDao();
            var rlt = dao.GetUsers(SearchName, SearchCard, SelectedDeptId);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            Users = rlt.Entities;
        }
        #endregion

        #region Commit
        private RelayCommand _CommitCmd;
        public RelayCommand CommitCmd
        {
            get
            {
                if (_CommitCmd == null)
                {
                    _CommitCmd = new RelayCommand(Commit);
                }
                return _CommitCmd;
            }
        }

        private void Commit()
        {
            if (SelectedUser == null)
            {
                MessageAlert.Alert("请选择用户！");
                return;
            }
            MessengerInstance.Send<User>(SelectedUser, MsgToken.TransforUser);
            MessengerInstance.Send<bool>(true, MsgToken.CloseSelectUser);
        }
        #endregion

        #region Cancel
        private RelayCommand _CancelCmd;
        public RelayCommand CancelCmd
        {
            get
            {
                if (_CancelCmd == null)
                {
                    _CancelCmd = new RelayCommand(Cancel);
                }
                return _CancelCmd;
            }
        }
        void Cancel()
        {
            MessengerInstance.Send<bool>(false, MsgToken.CloseSelectUser);
        }
        #endregion

        private List<User> _Users;
        public List<User> Users { get => _Users; set => Set(ref _Users, value); }

        private User _SelectedUser;
        public User SelectedUser { get => _SelectedUser; set => Set(ref _SelectedUser, value); }

        public List<Department> Departments { get => _departments; set => Set(ref _departments, value); }

        public int SelectedDeptId { get => _SelectedDeptId; set => Set(ref _SelectedDeptId, value); }

        public string SearchCard { get => _searchCard; set => Set(ref _searchCard, value); }
        public string SearchName { get => _searchName; set => Set(ref _searchName, value); }

        private string _searchName = "";
        private string _searchCard = "";
        private int _SelectedDeptId = 0;
        private List<Department> _departments;
    }
}
