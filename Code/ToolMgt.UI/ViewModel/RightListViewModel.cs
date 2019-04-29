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
    public class RightListViewModel : ViewModelBase
    {

        public RightListViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.RefreshRightList, RefreshList);
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
            if (!Global.HasRight("0602"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            Right right = new Right();
            MessengerInstance.Send<object>(right, MsgToken.OpenRightInfo);
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
            if (!Global.HasRight("0602"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectedRight != null)
            {
                MessengerInstance.Send<object>(SelectedRight, MsgToken.OpenRightInfo);
            }
            else
            {
                MessageAlert.Alert("请选择权限信息！");
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
            if (!Global.HasRight("0603"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectedRight == null)
            {
                MessageAlert.Alert("请选择要删除的记录！");
                return;
            }
            var confirm = await MessageAlert.Confirm("确定删除此记录？");
            if (!confirm)
            {
                return;
            }
            RightDao dao = new RightDao();
            var rlt = dao.DeleteRight(SelectedRight.Id);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
            }
            RefreshList(null);
        }
        #endregion

        public Right SelectedRight { get => _selectedRight; set => Set(ref _selectedRight, value); }
        public List<Right> Rights { get => _rights; set => Set(ref _rights, value); }

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
            if (!Global.HasRight("0601"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            RightDao dao = new RightDao();
            var rlt = dao.GetRights();
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            Rights = rlt.Entities;
        }

        private List<Right> _rights;
        private Right _selectedRight;
        private RelayCommand _addCmd;
        private RelayCommand _editCmd;
        private RelayCommand _deleteCmd;
        private RelayCommand _refreshCmd;
    }
}
