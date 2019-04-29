using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Common;
using ToolMgt.Model;

namespace ToolMgt.BLL
{
    public class ToolRepairDao
    {
        private ToolMgtEntities Db;
        public ToolRepairDao()
        {
            Db = ContextFactory.GetContext();
        }

        public Result<List<ToolRepair>> GetToolRepairs()
        {
            Result<List<ToolRepair>> rlt = new Result<List<ToolRepair>>();
            try
            {
                rlt.Entities = Db.ToolRepairs.ToList();
                if (rlt.Entities != null && rlt.Entities.Count > 0)
                {
                    foreach (var toolrep in rlt.Entities)
                    {
                        var tool = Db.Tools.FirstOrDefault(p => p.Id == toolrep.ToolId);
                        var operateuser = Db.Users.FirstOrDefault(p => p.Id == toolrep.OperatorId);
                        toolrep.ToolBarcode = tool?.Barcode;
                        toolrep.ToolName = tool?.Name;
                        toolrep.OperatorName = operateuser?.Name;
                    }
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

        public Result<bool> ApproveToolRepair(ToolRepair tr)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.Entry(tr);
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

        public Result<bool> AddToolRepair(ToolRepair tr)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var tool = Db.Tools.First(p => p.Id == tr.ToolId);
                var toolstate = Db.ToolStates.First(p => p.Code == ToolStateCode.Repair);
                tool.StateId = toolstate.Id;
                Db.ToolRepairs.Add(tr);
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

        public Result<bool> EditToolRepair(ToolRepair tr)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.Entry(tr);
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

        public Result<bool> DeleteToolRepair(int id)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.ToolRepairs.First(p => p.Id == id);
                Db.ToolRepairs.Remove(old);
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
