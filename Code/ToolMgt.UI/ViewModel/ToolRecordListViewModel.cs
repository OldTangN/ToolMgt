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
    public class ToolRecordListViewModel : ViewModelBase
    {

        public ToolRecordListViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.RefreshToolRecordList, RefreshList);
            RefreshList(null);

            ToolStateDao stateDao = new ToolStateDao();
            var deptrlt = stateDao.GetToolStates();
            if (deptrlt.HasError)
            {
                MessageAlert.Alert(deptrlt.Msg);
            }
            ToolCategoryDao toolCategoryDao = new ToolCategoryDao();
            var categoryrlt = toolCategoryDao.GetToolCategorys();
            if (categoryrlt.HasError)
            {
                MessageAlert.Alert(categoryrlt.Msg);
            }
            ToolCategory categoryAll = new ToolCategory()
            {
                Id = 0,
                Name = "全部"
            };
            categoryrlt.Entities.Insert(0, categoryAll);
            Categorys = categoryrlt.Entities;

            ToolState deptAll = new ToolState()
            {
                Id = 0,
                Name = "全部"
            };
            deptrlt.Entities.Insert(0, deptAll);
            States = deptrlt.Entities;
        }

        #region BorrowCmd
        public RelayCommand BorrowCmd
        {
            get
            {
                if (_borrowCmd == null)
                {
                    _borrowCmd = new RelayCommand(Borrow);
                }
                return _borrowCmd;
            }
        }

        private void Borrow()
        {
            if (!Global.HasRight("1002"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            ToolRecord state = new ToolRecord();
            MessengerInstance.Send<object>(null, MsgToken.OpenToolBorrow);
        }
        #endregion

        #region ReturnCmd
        public RelayCommand ReturnCmd
        {
            get
            {
                if (_returnCmd == null)
                {
                    _returnCmd = new RelayCommand(Return);
                }
                return _returnCmd;
            }
        }

        private void Return()
        {
            if (!Global.HasRight("1002"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            ToolRecord state = new ToolRecord();
            MessengerInstance.Send<object>(null, MsgToken.OpenToolReturn);
        }
        #endregion

        #region SearchCmd
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
            if (!Global.HasRight("1001"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            ToolRecords = new List<ToolRecord>();
            ToolRecordDao dao = new ToolRecordDao();
            var rlt = dao.GetToolRecords(SearchCard, SearchBarcode, SelectedCategoryId, SelectedStateId);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            ToolRecords = rlt.Entities;
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
            if (!Global.HasRight("1003"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectRecord == null)
            {
                MessageAlert.Alert("请选择要删除的记录！");
                return;
            }
            var confirm = await MessageAlert.Confirm("确定删除此记录？");
            if (!confirm)
            {
                return;
            }
            ToolRecordDao dao = new ToolRecordDao();
            var rlt = dao.DeleteToolRecord(SelectRecord.Id);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
            }
            RefreshList(null);
        }
        #endregion

        public List<ToolRecord> ToolRecords { get => _toolRecords; set => Set(ref _toolRecords, value); }

        private void RefreshList(object obj)
        {
            Search();
        }

        public ToolRecord SelectRecord { get => _SelectRecord; set => Set(ref _SelectRecord, value); }
        public string SearchCard { get => _SearchCard; set => Set(ref _SearchCard, value); }
        public string SearchBarcode { get => _SearchBarcode; set => Set(ref _SearchBarcode, value); }
        public int SelectedCategoryId { get => _SelectedCategoryId; set => Set(ref _SelectedCategoryId, value); }
        public int SelectedStateId { get => _SelectedStateId; set => Set(ref _SelectedStateId, value); }

        public List<ToolState> States { get => _states; set => Set(ref _states, value); }
        public List<ToolCategory> Categorys { get => _categorys; set => Set(ref _categorys, value); }

        private List<ToolState> _states;
        private List<ToolCategory> _categorys;
        private int _SelectedStateId = 0;
        private int _SelectedCategoryId = 0;
        private string _SearchBarcode = "";
        private string _SearchCard = "";
        private ToolRecord _SelectRecord;
        private List<ToolRecord> _toolRecords;
        private RelayCommand _borrowCmd;
        private RelayCommand _returnCmd;
        private RelayCommand _deleteCmd;
        private RelayCommand _SearchCmd;
    }
}
