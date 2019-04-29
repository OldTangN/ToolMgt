using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ToolMgt.BLL;
using ToolMgt.Helper;
using ToolMgt.Model;
using ToolMgt.UI.Common;

namespace ToolMgt.UI.ViewModel
{
    public class UserInfoViewModel : ViewModelBase
    {

        public UserInfoViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.TransforUser, TransforUser);
            MessengerInstance.Register<string>(this, MsgToken.TransforImgPath, TransforImgPath);
            App.ICCardReader.HandDataBack += ICCardReader_HandDataBack;
        }

        private void ICCardReader_HandDataBack(object sender, DataEventArgs e)
        {
            CurrUser.CardNo = e.Data;
        }

        public User CurrUser { get => _currUser; set => Set(ref _currUser, value); }

        #region CommitCmd
        private RelayCommand _commitCmd;
        public RelayCommand CommitCmd
        {
            get
            {
                if (_commitCmd == null)
                {
                    _commitCmd = new RelayCommand(Commit);
                }
                return _commitCmd;
            }
        }

        private void Commit()
        {
            if (string.IsNullOrEmpty(CurrUser.Code) || string.IsNullOrEmpty(CurrUser.Name))
            {
                MessageAlert.Alert("编码、名称不能为空！");
            }
            UserDao dao = new UserDao();
            Result<bool> rlt;
            if (CurrUser.Id == 0)
            {
                rlt = dao.AddUser(CurrUser);
            }
            else
            {
                rlt = dao.EditUser(CurrUser);
            }
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            MessengerInstance.Send(true, MsgToken.CloseUserInfo);
        }
        #endregion

        #region CancelCmd
        private RelayCommand _cancelCmd;
        public RelayCommand CancelCmd
        {
            get
            {
                if (_cancelCmd == null)
                {
                    _cancelCmd = new RelayCommand(Cancel);
                }
                return _cancelCmd;
            }
        }

        private void Cancel()
        {
            MessengerInstance.Send(false, MsgToken.CloseUserInfo);
        }
        #endregion

        #region RefreshCameraCmd
        private RelayCommand _RefreshCameraCmd;
        public RelayCommand RefreshCameraCmd
        {
            get
            {
                if (_RefreshCameraCmd == null)
                {
                    _RefreshCameraCmd = new RelayCommand(RefreshCamera);
                }
                return _RefreshCameraCmd;
            }
        }
        void RefreshCamera()
        {
            MessengerInstance.Send<object>(null, MsgToken.RefreshCamera);
        }
        #endregion

        #region TakePicCmd
        private RelayCommand _TakePicCmd;
        public RelayCommand TakePicCmd
        {
            get
            {
                if (_TakePicCmd == null)
                {
                    _TakePicCmd = new RelayCommand(TakePic);
                }
                return _TakePicCmd;
            }
        }
        void TakePic()
        {
            MessengerInstance.Send<object>(this, MsgToken.TakePic);
        }
        #endregion
        public List<Duty> Duties { get => _duties; set => Set(ref _duties, value); }
        public List<Department> Departments { get => _departments; set => Set(ref _departments, value); }
        public List<Role> Roles { get => _roles; set => Set(ref _roles, value); }

        private void TransforUser(object obj)
        {
            DutyDao dutyDao = new DutyDao();
            var dutyrlt = dutyDao.GetDuties();
            if (dutyrlt.HasError)
            {
                MessageAlert.Alert(dutyrlt.Msg);
            }
            DeptDao deptDao = new DeptDao();
            var deptrlt = deptDao.GetDepartments();
            if (deptrlt.HasError)
            {
                MessageAlert.Alert(deptrlt.Msg);
            }
            RoleDao roleDao = new RoleDao();
            var rolerlt = roleDao.GetRoles();
            if (rolerlt.HasError)
            {
                MessageAlert.Alert(rolerlt.Msg);
            }
            Duties = dutyrlt.Entities;
            Departments = deptrlt.Entities;
            Roles = rolerlt.Entities;
            CurrUser = ObjectCopier.DeepCopyByReflect(obj as User);
            if (CurrUser.Id == 0)
            {
                RefreshCamera();
                CurrUser.JoinDate = DateTime.Now;
            }
            MessengerInstance.Send<string>(CurrUser.ImagePath, MsgToken.ShowImage);
        }

        public string ImgPath { get; set; }

        private void TransforImgPath(string path)
        {
            this.ImgPath = path;
            CurrUser.ImagePath = this.ImgPath;
        }

        public override void Cleanup()
        {
            App.ICCardReader.HandDataBack -= ICCardReader_HandDataBack;
            MessengerInstance.Unregister(this);
            base.Cleanup();
        }
        private User _currUser;
        private List<Duty> _duties;
        private List<Department> _departments;
        private List<Role> _roles;
    }
}
