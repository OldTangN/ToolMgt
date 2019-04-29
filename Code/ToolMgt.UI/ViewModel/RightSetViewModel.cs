using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ToolMgt.BLL;
using ToolMgt.Common;
using ToolMgt.Helper;
using ToolMgt.Model;
using ToolMgt.UI.Common;

namespace ToolMgt.UI.ViewModel
{
    public class RightSetViewModel : ViewModelBase
    {
        public RightSetViewModel()
        {            
            //查询角色
            RoleDao roleDao = new RoleDao();
            var roleRlt = roleDao.GetRoles();
            if (roleRlt.HasError)
            {
                MessageAlert.Alert(roleRlt.Msg);
            }
            else
            {
                Roles = roleRlt.Entities;
            }
            //查询所有权限
            RightDao rightDao = new RightDao();
            var rightRlt = rightDao.GetRights();
            if (rightRlt.HasError)
            {
                MessageAlert.Alert(rightRlt.Msg);
            }
            else
            {
                Rights = rightRlt.Entities;
            }
        }

        public List<Right> Rights { get; set; }
        public ObservableCollection<Right> RoleRights { get; set; } = new ObservableCollection<Right>();
        public List<Role> Roles { get; set; }

        private Role _SelectedRole;
        public Role SelectedRole
        {
            get => _SelectedRole; set
            {
                if (_SelectedRole != value)
                {
                    _SelectedRole = value;
                    if (_SelectedRole != null)
                    {
                        RightDao dao = new RightDao();
                        var rlt = dao.GetRightsByRoleId(value.Id);//查询角色权限
                        if (rlt.HasError)
                        {
                            MessageAlert.Alert(rlt.Msg);
                            return;
                        }
                        RoleRights.Clear();
                        if (rlt.Entities != null && rlt.Entities.Count > 0)
                        {
                            foreach (var item in rlt.Entities)
                            {
                                RoleRights.Add(item);
                            }
                        }
                        
                    }
                }
            }
        }
        public Right SelectedRight { get; set; }
        public Right SelectedRoleRight { get; set; }

        #region AddCmd
        private RelayCommand _AddCmd;
        public RelayCommand AddCmd
        {
            get
            {
                if (_AddCmd == null)
                {
                    _AddCmd = new RelayCommand(Add);
                }
                return _AddCmd;
            }
        }

        private void Add()
        {
            if (SelectedRole == null)
            {
                MessageAlert.Alert("请选择角色");
                return;
            }
            if (SelectedRight == null)
            {
                MessageAlert.Alert("请选择权限");
                return;
            }
            var exists = RoleRights.FirstOrDefault(p => p.Id == SelectedRight.Id);
            if (exists == null)
            {
                RoleRights.Add(SelectedRight);
            }
        }
        #endregion

        #region AddAllCmd
        private RelayCommand _AddAllCmd;
        public RelayCommand AddAllCmd
        {
            get
            {
                if (_AddAllCmd == null)
                {
                    _AddAllCmd = new RelayCommand(AddAll);
                }
                return _AddAllCmd;
            }
        }

        private void AddAll()
        {
            if (SelectedRole == null)
            {
                MessageAlert.Alert("请选择角色");
                return;
            }
            foreach (var right in Rights)
            {
                var exists = RoleRights.FirstOrDefault(p => p.Id == right.Id);
                if (exists == null)
                {
                    RoleRights.Add(right);
                }
            }
        }

        #endregion

        #region DeleteCmd
        private RelayCommand _DeleteCmd;
        public RelayCommand DeleteCmd
        {
            get
            {
                if (_DeleteCmd == null)
                {
                    _DeleteCmd = new RelayCommand(Delete);
                }
                return _DeleteCmd;
            }
        }

        private void Delete()
        {
            if (SelectedRole == null)
            {
                MessageAlert.Alert("请选择角色");
                return;
            }
            if (SelectedRoleRight == null)
            {
                MessageAlert.Alert("请选择角色权限");
                return;
            }
            var exists = RoleRights.FirstOrDefault(p => p.Id == SelectedRoleRight.Id);
            if (exists != null)
            {
                RoleRights.Remove(exists);
            }
        }
        #endregion

        #region CommitCmd
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
            if (!Global.HasRight("0701"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectedRole == null)
            {
                MessageAlert.Alert("请选择角色");
                return;
            }
            RightDao rightDao = new RightDao();
            List<int> rightIds = new List<int>();
            RoleRights.ToList().ForEach(p => rightIds.Add(p.Id));
            var rlt = rightDao.EditRoleRight(SelectedRole.Id, rightIds);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
            }
            MessengerInstance.Send<bool>(true, MsgToken.CloseRightSet);
        }
        #endregion

        #region CancelCmd
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

        private void Cancel()
        {
            MessengerInstance.Send<bool>(true, MsgToken.CloseRightSet);
        }
        #endregion

        public override void Cleanup()
        {
            base.Cleanup();
        }
    }
}
