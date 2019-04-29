using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class ToolInfoViewModel : ViewModelBase
    {

        public ToolInfoViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.TransforTool, TransforTool);
            App.RFIDReader.HandDataBack += RFIDReader_HandDataBack;
        }

        private void RFIDReader_HandDataBack(object sender, DataEventArgs e)
        {
            string[] rfids = e.Data?.Split(';');
            if (rfids != null && rfids.Length > 0)
            {
                CurrTool.RFID = rfids[0];
            }
        }

        public string SearchBarcode { get => _searchBarcode; set => Set(ref _searchBarcode, value); }
        public Tool CurrTool { get => _currTool; set => Set(ref _currTool, value); }

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
            if (string.IsNullOrEmpty(CurrTool.Barcode))
            {
                MessageAlert.Alert("条码不能为空！");
                return;
            }
            ToolDao dao = new ToolDao();
            Result<bool> rlt;
            if (!CurrTool.IsMeasureTool)
            {
                CurrTool.CheckNextDate = null;
            }
            else
            {
                if (CurrTool.CheckNextDate == null)
                {
                    MessageAlert.Alert("请填写到校日期！");
                    return;
                }
            }
            CurrTool.PkgItems.Clear();
            if (CurrTool.IsPkg && ToolPkgItems.Count > 0)
            {
                foreach (var item in ToolPkgItems)
                {
                    ToolPkgItem pkgitem = new ToolPkgItem()
                    {
                        ParentId = CurrTool.Id,
                        ChildrenId = item.ChildrenId,
                        CreateDate = item.CreateDate
                    };
                    CurrTool.PkgItems.Add(pkgitem);
                }
            }
            if (CurrTool.Id == 0)
            {
                rlt = dao.AddTool(CurrTool);
            }
            else
            {
                rlt = dao.EditTool(CurrTool);
            }
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            MessengerInstance.Send(true, MsgToken.CloseToolInfo);
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
            MessengerInstance.Send(false, MsgToken.CloseToolInfo);
        }
        #endregion

        #region AddCmd
        private RelayCommand _addCmd;
        public RelayCommand AddCmd
        {
            get
            {
                if (_addCmd == null)
                {
                    _addCmd = new RelayCommand(AddItem);
                }
                return _addCmd;
            }
        }

        private void AddItem()
        {
            if (string.IsNullOrEmpty(SearchBarcode))
            {
                MessageAlert.Alert("请输入完整的工具条码！");
                return;
            }
            ToolDao toolDao = new ToolDao();
            var rlt = toolDao.GetToolIsNotChild(SearchBarcode);
            if (rlt.Entities == null)
            {
                MessageAlert.Alert("此条码工具不存在 或 已被其他工具包使用！");
                return;
            }

            if (ToolPkgItems.FirstOrDefault(p => p.ChildrenId == rlt.Entities.Id) != null)
            {
                return;
            }
            ToolPkgItem item = new ToolPkgItem()
            {
                ParentId = CurrTool.Id,
                ChildrenId = rlt.Entities.Id,
                ChildBarcode = rlt.Entities.Barcode,
                ChildSpec = rlt.Entities.Spec,
                ChildName = rlt.Entities.Name,
                CreateDate = DateTime.Now
            };
            ToolPkgItems.Add(item);
        }
        #endregion

        #region RemoveCmd
        private RelayCommand<int> _RemoveCmd;
        public RelayCommand<int> RemoveCmd
        {
            get
            {
                if (_RemoveCmd == null)
                {
                    _RemoveCmd = new RelayCommand<int>(Remove);
                }
                return _RemoveCmd;
            }
        }

        private void Remove(int chldId)
        {
            if (chldId == 0)
            {
                return;
            }
            var old = ToolPkgItems.First(p => p.ChildrenId == chldId);
            ToolPkgItems.Remove(old);
        }
        #endregion
        public List<ToolState> States { get => _states; set => Set(ref _states, value); }
        public List<ToolCategory> Categorys { get => _categorys; set => Set(ref _categorys, value); }
        public ObservableCollection<ToolPkgItem> ToolPkgItems
        {
            get
            {
                if (_ToolPkgItems == null)
                {
                    _ToolPkgItems = new ObservableCollection<ToolPkgItem>();
                }
                return _ToolPkgItems;
            }
        }
        private void TransforTool(object obj)
        {
            ToolStateDao stateDao = new ToolStateDao();
            var staterlt = stateDao.GetToolStates();
            if (staterlt.HasError)
            {
                MessageAlert.Alert(staterlt.Msg);
            }
            ToolCategoryDao cagDao = new ToolCategoryDao();
            var cagrlt = cagDao.GetToolCategorys();
            if (cagrlt.HasError)
            {
                MessageAlert.Alert(cagrlt.Msg);
            }

            States = staterlt.Entities;
            Categorys = cagrlt.Entities;

            ToolPkgItems.Clear();//未真正释放viewmodel，清下缓存
            CurrTool = ObjectCopier.DeepCopyByReflect(obj as Tool);
            if (CurrTool.IsPkg && CurrTool.Id != 0)
            {
                ToolPkgItemDao toolPkgItemDao = new ToolPkgItemDao();
                var rlt = toolPkgItemDao.GetToolPkgItems(CurrTool.Id);
                if (rlt.Entities != null && rlt.Entities.Count > 0)
                {
                    rlt.Entities.ForEach(p => ToolPkgItems.Add(p));
                }
            }
        }

        public override void Cleanup()
        {
            App.RFIDReader.HandDataBack -= RFIDReader_HandDataBack; ;
            MessengerInstance.Unregister(this);
            base.Cleanup();
        }


        private ObservableCollection<ToolPkgItem> _ToolPkgItems;
        private string _searchBarcode;
        private Tool _currTool;
        private List<ToolState> _states;
        private List<ToolCategory> _categorys;
    }
}
