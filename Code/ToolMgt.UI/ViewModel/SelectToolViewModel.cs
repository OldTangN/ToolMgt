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
    public class SelectToolViewModel : ViewModelBase
    {
        public SelectToolViewModel()
        {
            ToolCategoryDao deptDao = new ToolCategoryDao();
            var deptrlt = deptDao.GetToolCategorys();
            if (deptrlt.HasError)
            {
                MessageAlert.Alert(deptrlt.Msg);
                return;
            }
            ToolCategory ctgAll = new ToolCategory()
            {
                Id = 0,
                Name = "全部"
            };
            deptrlt.Entities.Insert(0, ctgAll);
            Categorys = deptrlt.Entities;
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
            Tools = new List<Tool>();
            ToolDao dao = new ToolDao();
            var rlt = dao.GetTools(SearchName, SearchBarcode, SelectedCtgId);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            Tools = rlt.Entities;
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
            if (SelectedTool == null)
            {
                MessageAlert.Alert("请选择工具！");
                return;
            }
            MessengerInstance.Send<Tool>(SelectedTool, MsgToken.TransforTool);
            MessengerInstance.Send<bool>(true, MsgToken.CloseSelectTool);
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
            MessengerInstance.Send<bool>(false, MsgToken.CloseSelectTool);
        }
        #endregion

        private List<Tool> _Tools;
        public List<Tool> Tools { get => _Tools; set => Set(ref _Tools, value); }

        private Tool _SelectedTool;
        public Tool SelectedTool { get => _SelectedTool; set => Set(ref _SelectedTool, value); }

        public List<ToolCategory> Categorys { get => _Categorys; set => Set(ref _Categorys, value); }

        public int SelectedCtgId { get => _SelectedCtgId; set => Set(ref _SelectedCtgId, value); }

        public string SearchBarcode { get => _SearchBarcode; set => Set(ref _SearchBarcode, value); }
        public string SearchName { get => _SearchName; set => Set(ref _SearchName, value); }

        private string _SearchName = "";
        private string _SearchBarcode = "";
        private int _SelectedCtgId = 0;
        private List<ToolCategory> _Categorys;
    }
}
