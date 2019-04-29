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
using ToolMgt.Model;
using ToolMgt.UI.Common;

namespace ToolMgt.UI.ViewModel
{
    public class ToolBorrowViewModel : ViewModelBase
    {
        public ToolBorrowViewModel()
        {
            Borrower = new User()
            {
                Id = 0,
                Name = "",
                CardNo = ""
            };
            BorrowOperator = Global.CurrUser;
            ExceptReturnTime = DateTime.Now;
            MessengerInstance.Register<User>(this, MsgToken.TransforUser, TransforUser);
            MessengerInstance.Register<Tool>(this, MsgToken.TransforTool, TransforTool);
            MessengerInstance.Register<string>(this, MsgToken.TransforImgPath, TransforImgPath);
            App.ICCardReader.HandDataBack += ICCardReader_HandDataBack;
            App.RFIDReader.HandDataBack += RFIDReader_HandDataBack;
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

        private void ICCardReader_HandDataBack(object sender, Helper.DataEventArgs e)
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

        private void RFIDReader_HandDataBack(object sender, Helper.DataEventArgs e)
        {
            string[] rfids = e.Data?.Split(';');
            if (rfids != null && rfids.Length > 0)
            {
                ToolDao toolDao = new ToolDao();
                foreach (var rfid in rfids)
                {
                    //判断是否已经存在于领用列表
                    if (ExistRFID(rfid))
                    {
                        continue;
                    }
                    var rlt = toolDao.GetToolByRFID(rfid);
                    if (!rlt.HasError && rlt.Entities != null)
                    {
                        TransforTool(rlt.Entities);
                    }
                }
            }
        }

        private void TransforUser(User user)
        {
            Borrower = ObjectCopier.DeepCopyByReflect(user);
            MessengerInstance.Send<string>(Borrower.ImagePath, MsgToken.ShowImage);
        }
        private void TransforTool(Tool tool)
        {
            var t = ObjectCopier.DeepCopyByReflect(tool);
            if (t.ToolState.Name != "正常")
            {
                MessageAlert.Alert("当前工具状态不可借出！");
                return;
            }
            //判断重复
            if (ExistBarcode(tool.Barcode))
            {
                return;
            }
            ToolRecord rcd = new ToolRecord()
            {
                ToolId = t.Id,
                BorrowerId = Borrower.Id,
                BorrowOperatorId = BorrowOperator.Id,
                BorrowTime = DateTime.Now,
                ExceptReturnTime = this.ExceptReturnTime,
                Tool = tool
            };
            ToolRecords.Add(rcd);
        }

        private bool ExistBarcode(string barcode)
        {
            return ToolRecords.FirstOrDefault(p => p.Tool.Barcode == barcode) != null;
        }
        private bool ExistRFID(string rfid)
        {
            return ToolRecords.FirstOrDefault(p => p.Tool.RFID == rfid) != null;
        }

        private string _SearchBarcode;

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

        #region SelectToolCmd
        private RelayCommand _SelectToolCmd;
        public RelayCommand SelectToolCmd
        {
            get
            {
                if (_SelectToolCmd == null)
                {
                    _SelectToolCmd = new RelayCommand(SelectTool);
                }
                return _SelectToolCmd;
            }
        }
        private void SelectTool()
        {
            //打开用户列表选择
            MessengerInstance.Send<object>(null, MsgToken.OpenSelectTool);
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

        private User _BorrowOperator;
        public User BorrowOperator { get => _BorrowOperator; set => Set(ref _BorrowOperator, value); }

        private DateTime _ExceptReturnTime;
        public DateTime ExceptReturnTime { get => _ExceptReturnTime; set => Set(ref _ExceptReturnTime, value); }
        public string SearchBarcode { get => _SearchBarcode; set => Set(ref _SearchBarcode, value); }

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
            ToolDao toolDao = new ToolDao();
            var rlt = toolDao.GetToolByBarcode(SearchBarcode);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            if (rlt.Entities == null)
            {
                MessageAlert.Alert("工具条码不存在！");
                return;
            }
            if (rlt.Entities.ToolState.Name != "正常")
            {
                MessageAlert.Alert("当前工具状态不可借出！");
                return;
            }
            //判断重复
            if (ExistBarcode(rlt.Entities.Barcode))
            {
                return;
            }

            ToolRecord rcd = new ToolRecord()
            {
                ToolId = rlt.Entities.Id,
                BorrowerId = Borrower.Id,
                BorrowOperatorId = BorrowOperator.Id,
                BorrowTime = DateTime.Now,
                ExceptReturnTime = this.ExceptReturnTime,
                Tool = ObjectCopier.DeepCopyByReflect(rlt.Entities)
            };
            ToolRecords.Add(rcd);
        }
        #endregion

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

        private void _ToolRecords_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
        }

        #region DeleteCmd
        private RelayCommand<int> _DeleteCmd;
        public RelayCommand<int> DeleteCmd
        {
            get
            {
                if (_DeleteCmd == null)
                {
                    _DeleteCmd = new RelayCommand<int>(Delete);
                }
                return _DeleteCmd;
            }
        }

        private void Delete(int toolid)
        {
            var rcd = ToolRecords.FirstOrDefault(p => p.Id == toolid);
            if (rcd != null)
            {
                ToolRecords.Remove(rcd);
            }
        }
        #endregion

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
                MessageAlert.Alert("请录入要借用的工具！");
                return;
            }
            foreach (var r in ToolRecords)
            {
                r.BorrowerId = Borrower.Id;
                r.BorrowOperatorId = BorrowOperator.Id;
                r.BorrowTime = DateTime.Now;
                r.ExceptReturnTime = Convert.ToDateTime(string.Format("{0:yyyy-MM-dd 23:59:59}", this.ExceptReturnTime));
            };
            ToolRecordDao toolRecordDao = new ToolRecordDao();
            var rlt = toolRecordDao.AddToolRecords(ToolRecords);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            MessengerInstance.Send(true, MsgToken.CloseToolBorrow);
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
            MessengerInstance.Send(false, MsgToken.CloseToolBorrow);
            ToolRecords.Clear();
        }
        #endregion

        public override void Cleanup()
        {
            MessengerInstance.Unregister(this);
            App.RFIDReader.HandDataBack -= RFIDReader_HandDataBack;
            App.ICCardReader.HandDataBack -= ICCardReader_HandDataBack;
            base.Cleanup();
        }
    }
}
