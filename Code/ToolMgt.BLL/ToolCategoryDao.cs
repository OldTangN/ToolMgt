using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Common;
using ToolMgt.Model;

namespace ToolMgt.BLL
{
    public class ToolCategoryDao
    {
        private ToolMgtEntities Db;
        public ToolCategoryDao()
        {
            Db = ContextFactory.GetContext();
        }

        public Result<List<ToolCategory>> GetToolCategorys()
        {
            Result<List<ToolCategory>> rlt = new Result<List<ToolCategory>>();
            try
            {
                rlt.Entities = Db.ToolCategories.ToList();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }

        public Result<bool> AddToolCategory(ToolCategory ctg)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.ToolCategories.FirstOrDefault(p => p.Code == ctg.Code);
                if (old != null)
                {
                    rlt.HasError = true;
                    rlt.Msg = "工具状态编码重复！";
                }
                else
                {
                    Db.ToolCategories.Add(ctg);
                    Db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }

        public Result<bool> EditToolCategory(ToolCategory ctg)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.Entry(ctg);
                old.State = System.Data.Entity.EntityState.Modified;
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }

        public Result<bool> DeleteToolCategory(int id)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.ToolCategories.First(p => p.Id == id);
                if (old.Tools.Count > 0)
                {
                    rlt.HasError = true;
                    rlt.Msg = "工具状态信息被使用，无法删除！";
                    return rlt;
                }
                Db.ToolCategories.Remove(old);
                Db.SaveChanges();
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
