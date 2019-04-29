using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Common;
using ToolMgt.Model;

namespace ToolMgt.BLL
{
    public class ToolPkgItemDao
    {
        private ToolMgtEntities Db;
        public ToolPkgItemDao()
        {
            Db = ContextFactory.GetContext();
        }

        public Result<List<ToolPkgItem>> GetToolPkgItems(int parentId)
        {
            Result<List<ToolPkgItem>> rlt = new Result<List<ToolPkgItem>>();
            try
            {
                var tmp = Db.ToolPkgItems.Where(p => p.ParentId == parentId).ToList();
                if (tmp != null && tmp.Count > 0)
                {
                    foreach (var item in tmp)
                    {
                        var child = Db.Tools.FirstOrDefault(p => p.Id == item.ChildrenId);
                        item.ChildBarcode = child?.Barcode;
                        item.ChildSpec = child?.Spec;
                        item.ChildName = child?.Name;
                    }
                }
                rlt.Entities = tmp;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }
    }
}
