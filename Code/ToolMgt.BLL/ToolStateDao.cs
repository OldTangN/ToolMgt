using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Common;
using ToolMgt.Model;

namespace ToolMgt.BLL
{
    public class ToolStateDao
    {
        private ToolMgtEntities Db;
        public ToolStateDao()
        {
            Db = ContextFactory.GetContext();
        }

        public Result<List<ToolState>> GetToolStates()
        {
            Result<List<ToolState>> rlt = new Result<List<ToolState>>();
            try
            {
                rlt.Entities = Db.ToolStates.ToList();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }

        public Result<bool> AddToolState(ToolState state)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.ToolStates.FirstOrDefault(p => p.Code == state.Code);
                if (old != null)
                {
                    rlt.HasError = true;
                    rlt.Msg = "工具状态编码重复！";
                }
                else
                {
                    Db.ToolStates.Add(state);
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

        public Result<bool> EditToolState(ToolState state)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.Entry(state);
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

        public Result<bool> DeleteToolState(int id)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.ToolStates.First(p => p.Id == id);
                if (old.Tools.Count > 0)
                {
                    rlt.HasError = true;
                    rlt.Msg = "工具状态信息被使用，无法删除！";
                    return rlt;
                }
                Db.ToolStates.Remove(old);
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

        public Result<ToolState> GetToolStateByCode(string stateCode)
        {
            Result<ToolState> rlt = new Result<ToolState>();
            try
            {
                rlt.Entities = Db.ToolStates.FirstOrDefault(p => p.Code == stateCode);
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
