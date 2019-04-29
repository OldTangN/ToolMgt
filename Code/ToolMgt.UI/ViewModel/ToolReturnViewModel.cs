using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ToolMgt.BLL;
using ToolMgt.Helper;
using ToolMgt.Model;
using ToolMgt.UI.Common;

namespace ToolMgt.UI.ViewModel
{
    public class ToolReturnViewModel : ViewModelBase
    {
        public ToolReturnViewModel()
        {
            Borrower = new User()
            {
                Id = 0,
                Name = "",
                CardNo = ""
            };
            ReturnOperator = Global.CurrUser;
            RealReturnTime = DateTime.Now;
            MessengerInstance.Register<User>(this, MsgToken.TransforUser, TransforUser);
            MessengerInstance.Register<string>(this, MsgToken.TransforImgPath, TransforImgPath);

            ToolStateDao dao = new ToolStateDao();
            var rlt = dao.GetToolStates();
            this.ToolStates = rlt.Entities;
            App.RFIDReader.HandDataBack += RFIDReader_HandDataBack; ;
            App.ICCardReader.HandDataBack += ICCardReader_HandDataBack;
        }

        public string ImgPath { get; set; }
        private void TransforImgPath(string path)
        {
            this.ImgPath = path;
            MessengerInstance.Send<string>(this.ImgPath, MsgToken.ShowImage);
            //TODO:人脸识别
            UserDao userDao = new UserDao();
            var rlt = userDao.LoginByFace(this.ImgPath);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            if (rlt.Entities != null)
            {
                TransforUser(rlt.Entities);
            }
        }
        private void ICCardReader_HandDataBack(object sender, DataEventArgs e)
        {
            UserDao userDao = new UserDao();
            var rlt = userDao.GetUser(e.Data);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            if (rlt.Entities != null)
            {
                TransforUser(rlt.Entities);
            }
        }

        private void RFIDReader_HandDataBack(object sender, DataEventArgs e)
        {
            //查询是否当前用户领走的工具
            string[] rfids = e.Data?.Split(';');
            if (rfids == null || rfids.Length == 0)
            {
                return;
            }
            var state = ToolStates.FirstOrDefault(p => p.Code == ToolStateCode.Normal);
            if (state == null)
            {
                return;
            }
            foreach (var rfid in rfids)
            {
                //判断是否存在于归还列表
                if (!ExistRFID(rfid))
                {
                    continue;
                }
                ToolRecord rcd = ToolRecords.First(p => p.Tool.RFID == rfid);
                //设置正常状态
                rcd.IsReturn = true;
                rcd.ReturnStateId = state.Id;
                rcd.ReturnStateCode = state.Code;
                rcd.ReturnStateName = state.Name;
            }
        }
        private bool ExistRFID(string rfid)
        {
            return ToolRecords.FirstOrDefault(p => p.Tool.RFID == rfid) != null;
        }

        private void TransforUser(User user)
        {
            Borrower = ObjectCopier.DeepCopyByReflect(user);
            MessengerInstance.Send<string>(Borrower.ImagePath, MsgToken.ShowImage);

            ToolRecordDao dao = new ToolRecordDao();
            var rlt = dao.GetNotReturnToolRecords(Borrower.Id);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            ToolRecords.Clear();
            if (rlt.Entities != null && rlt.Entities.Count > 0)
            {
                foreach (var r in rlt.Entities)
                {
                    ToolRecords.Add(r);
                }
            }
        }

        #region SelectUserCmd
        private RelayCommand _SelectUserCmd;
        public RelayCommand SelectUserCmd
        {
            get
            {
                if (_SelectUserCmd == null)
                {
                    _SelectUserCmd = new RelayCommand(SelectUser);
                }
                return _SelectUserCmd;
            }
        }
        private void SelectUser()
        {
            //打开用户列表选择
            MessengerInstance.Send<object>(null, MsgToken.OpenSelectUser);
        }
        #endregion

        #region SearchUserCmd
        private RelayCommand _SearchUserCmd;
        public RelayCommand SearchUserCmd
        {
            get
            {
                if (_SearchUserCmd == null)
                {
                    _SearchUserCmd = new RelayCommand(SearchUser);
                }
                return _SearchUserCmd;
            }
        }

        private void SearchUser()
        {
            //查询用户
            UserDao userDao = new UserDao();
            var rlt = userDao.GetUser(Borrower.CardNo);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            if (rlt.Entities == null || rlt.Entities.Id == 0)
            {
                MessageAlert.Alert("卡号不存在！");
                return;
            }
            Borrower = ObjectCopier.DeepCopyByReflect(rlt.Entities);
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

        private User _Borrower;
        public User Borrower { get => _Borrower; set => Set(ref _Borrower, value); }

        private User _ReturnOperator;
        public User ReturnOperator { get => _ReturnOperator; set => Set(ref _ReturnOperator, value); }

        private DateTime _RealReturnTime = DateTime.Now;
        public DateTime RealReturnTime { get => _RealReturnTime; set => Set(ref _RealReturnTime, value); }

        private ObservableCollection<ToolRecord> _ToolRecords;
        public ObservableCollection<ToolRecord> ToolRecords
        {
            get
            {
                if (_ToolRecords == null)
                {
                    _ToolRecords = new ObservableCollection<ToolRecord>();
                    _ToolRecords.CollectionChanged += _ToolRecords_CollectionChanged;
                }
                return _ToolRecords;
            }
        }

        private ToolRecord _SelectedRecord;
        public ToolRecord SelectedRecord { get => _SelectedRecord; set => Set(ref _SelectedRecord, value); }

        private void _ToolRecords_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
        }

        private List<ToolState> _ToolStates;
        public List<ToolState> ToolStates { get => _ToolStates; set => Set(ref _ToolStates, value); }

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
            if (Borrower == null || Borrower.Id == 0)
            {
                MessageAlert.Alert("未登录领用人！");
                return;
            }
            if (ToolRecords.Count == 0)
            {
                MessageAlert.Alert("请录入要归还的工具！");
                return;
            }
            List<ToolRecord> returnRecords = new List<ToolRecord>();
            foreach (var r in ToolRecords)
            {
                if (r.IsReturn)
                {
                    r.ReturnOperatorId = ReturnOperator.Id;
                    r.RealReturnTime = DateTime.Now;
                    returnRecords.Add(r);
                }
            };
            if (returnRecords.Count > 0)
            {
                ToolRecordDao toolRecordDao = new ToolRecordDao();
                var rlt = toolRecordDao.ToolReturn(returnRecords);
                if (rlt.HasError)
                {
                    MessageAlert.Alert(rlt.Msg);
                    return;
                }
            }
            MessengerInstance.Send(true, MsgToken.CloseToolReturn);
            ToolRecords.Clear();
            MessengerInstance.Send<object>(null, MsgToken.RefreshToolRecordList);
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
            MessengerInstance.Send(false, MsgToken.CloseToolReturn);
            ToolRecords.Clear();
            Borrower = null;
        }
        #endregion

        #region SetStateCmd
        private RelayCommand<ToolState> _SetStateCmd;
        public RelayCommand<ToolState> SetStateCmd
        {
            get
            {
                if (_SetStateCmd == null)
                {
                    _SetStateCmd = new RelayCommand<ToolState>(SetState);
                }
                return _SetStateCmd;
            }
        }

        private void SetState(ToolState state)
        {
            if (SelectedRecord == null || state == null)
            {
                return;
            }
            SelectedRecord.IsReturn = true;
            SelectedRecord.ReturnStateId = state.Id;
            SelectedRecord.ReturnStateCode = state.Code;
            SelectedRecord.ReturnStateName = state.Name;
        }
        #endregion

        public override void Cleanup()
        {
            App.RFIDReader.HandDataBack -= RFIDReader_HandDataBack;
            App.ICCardReader.HandDataBack -= ICCardReader_HandDataBack;
            MessengerInstance.Unregister(this);
            base.Cleanup();
        }
    }
}
