using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Common;
using ToolMgt.Model;

namespace ToolMgt.BLL
{
    public class ToolRecordDao
    {
        private ToolMgtEntities Db;
        public ToolRecordDao()
        {
            Db = ContextFactory.GetContext();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public Result<List<ToolRecord>> GetNotReturnToolRecords(int uid)
        {
            Result<List<ToolRecord>> rlt = new Result<List<ToolRecord>>();
            try
            {
                rlt.Entities = Db.ToolRecords.Include("Tool").Where(p => p.BorrowerId == uid && p.IsReturn == false).ToList();
                SetExtProperty(rlt.Entities);
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }

        private void SetExtProperty(List<ToolRecord> rlt)
        {
            if (rlt != null && rlt.Count > 0)
            {
                foreach (var r in rlt)
                {
                    r.BorrowerName = Db.Users.FirstOrDefault(p => p.Id == r.BorrowerId)?.Name;
                    r.BorrowOperatorName = Db.Users.FirstOrDefault(p => p.Id == r.BorrowOperatorId)?.Name;
                    r.ReturnOperatorName = Db.Users.FirstOrDefault(p => p.Id == r.ReturnOperatorId)?.Name;
                    var state = Db.ToolStates.FirstOrDefault(p => p.Id == r.ReturnStateId);
                    r.ReturnStateCode = state?.Code;
                    r.ReturnStateName = state?.Name;
                }
            }
        }

        public Result<List<ToolRecord>> GetToolRecords(string cardNo = "", string barcode = "", int categoryid = 0, int returnstateid = 0)
        {
            Result<List<ToolRecord>> rlt = new Result<List<ToolRecord>>();
            try
            {
                var toolIds = Db.Tools.Where(t => t.Barcode.Contains(barcode)
                                   && (t.CategoryId == categoryid || categoryid == 0)).Select(c => c.Id).ToList();
                int borrowerid = Db.Users.Where(p => p.CardNo == cardNo).Select(c => c.Id).FirstOrDefault();
                var tmp = Db.ToolRecords.Include("Tool").Where(p => (p.BorrowerId == borrowerid || borrowerid == 0)
                                && toolIds.Contains(p.ToolId)
                                && (p.ReturnStateId == returnstateid || returnstateid == 0))
                                    .ToList();
                SetExtProperty(tmp);
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

        public Result<bool> AddToolRecords(IEnumerable<ToolRecord> records)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var state = Db.ToolStates.FirstOrDefault(p => p.Code == ToolStateCode.Using);
                //Db.ToolRecords.AddRange(records);
                foreach (var r in records)
                {
                    r.Tool = null;//清除关联，否则添加实体时会添加Tool
                    r.ReturnStateId = state.Id;
                    Db.ToolRecords.Add(r);
                    //修改工具状态为已领用
                    var tool = Db.Tools.First(p => p.Id == r.ToolId);
                    tool.StateId = state.Id;
                }
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

        public Result<bool> ToolReturn(List<ToolRecord> records)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                foreach (var rcd in records)
                {
                    var old = Db.ToolRecords.First(p => p.Id == rcd.Id);
                    old.ReturnStateId = rcd.ReturnStateId;
                    old.RealReturnTime = rcd.RealReturnTime;
                    old.ReturnOperatorId = rcd.ReturnOperatorId;
                    old.Note = rcd.Note;
                    old.IsReturn = true;
                    var tool = Db.Tools.First(p => p.Id == rcd.ToolId);
                    tool.StateId = rcd.ReturnStateId;
                }
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

        public Result<bool> DeleteToolRecord(int id)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.ToolRecords.First(p => p.Id == id);
                Db.ToolRecords.Remove(old);
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

        public Result<bool> VertifyCanBorrow(int toolId)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.ToolRecords.FirstOrDefault(p => p.ToolId == toolId && p.IsReturn == false);
                if (old == null)
                {
                    rlt.Entities = true;
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
    }
}
