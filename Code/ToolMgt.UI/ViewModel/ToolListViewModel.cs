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
    public class ToolListViewModel : ViewModelBase
    {
        public ToolListViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.RefreshToolList, RefreshList);
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

        #region AddCmd
        public RelayCommand<int> AddCmd
        {
            get
            {
                if (_addCmd == null)
                {
                    _addCmd = new RelayCommand<int>(Add);
                }
                return _addCmd;
            }
        }

        private void Add(int para)
        {
            if (!Global.HasRight("0902"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            Tool tool = new Tool()
            {
                BuyDate = DateTime.Now,
                IsPkg = para == 2
            };
            MessengerInstance.Send<object>(tool, MsgToken.OpenToolInfo);
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
            if (!Global.HasRight("0902"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectedTool != null)
            {
                MessengerInstance.Send<object>(SelectedTool, MsgToken.OpenToolInfo);
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
            if (!Global.HasRight("0903"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            if (SelectedTool == null)
            {
                MessageAlert.Alert("请选择要删除的记录！");
                return;
            }
            var confirm = await MessageAlert.Confirm("确定删除此记录？");
            if (!confirm)
            {
                return;
            }
            ToolDao dao = new ToolDao();
            var rlt = dao.DeleteTool(SelectedTool.Id);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
            }
            RefreshList(null);
        }
        #endregion

        public List<ToolState> States { get => _states; set => Set(ref _states, value); }
        public List<ToolCategory> Categorys { get => _categorys; set => Set(ref _categorys, value); }
        public string SearchBarcode { get => _searchBarcode; set => Set(ref _searchBarcode, value); }

        public string SearchName { get => _SearchName; set => Set(ref _SearchName, value); }

        public int SelectedStateId { get => _selectedStateid; set => Set(ref _selectedStateid, value); }
        public int SelectedCategoryId { get => _selectedCategoryId; set => Set(ref _selectedCategoryId, value); }

        public bool SearchPkg { get => _searchPkg; set => Set(ref _searchPkg, value); }

        public Tool SelectedTool { get => _selectedTool; set => Set(ref _selectedTool, value); }

        public List<Tool> Tools { get => _toolStates; set => Set(ref _toolStates, value); }

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
            if (!Global.HasRight("0901"))
            {
                MessageAlert.Alert("权限不足！");
                return;
            }
            Tools = new List<Tool>();
            ToolDao dao = new ToolDao();
            var rlt = dao.GetTools(SearchBarcode,"", SelectedStateId, SelectedCategoryId, SearchPkg);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            Tools = rlt.Entities;
        }

        private string _SearchName = "";
        private bool _searchPkg = false;
        private int _selectedCategoryId = 0;
        private string _searchBarcode = "";
        private int _selectedStateid = 0;
        private List<Tool> _toolStates;
        private Tool _selectedTool;
        private RelayCommand<int> _addCmd;
        private RelayCommand _editCmd;
        private RelayCommand _deleteCmd;
        private RelayCommand _refreshCmd;
        private List<ToolState> _states;
        private List<ToolCategory> _categorys;
    }
}
