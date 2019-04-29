using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Common;
using ToolMgt.Model;

namespace ToolMgt.BLL
{
    public class ToolDamageDao
    {
        private ToolMgtEntities Db;
        public ToolDamageDao()
        {
            Db = ContextFactory.GetContext();
        }

        public Result<List<ToolDamage>> GetToolDamages()
        {
            Result<List<ToolDamage>> rlt = new Result<List<ToolDamage>>();
            try
            {
                rlt.Entities = Db.ToolDamages.ToList();
                if (rlt.Entities != null && rlt.Entities.Count > 0)
                {
                    foreach (var dmg in rlt.Entities)
                    {
                        var tool = Db.Tools.FirstOrDefault(p => p.Id == dmg.ToolId);
                        var dutyuser = Db.Users.FirstOrDefault(p => p.Id == dmg.DutyUserId);
                        var operateuser = Db.Users.FirstOrDefault(p => p.Id == dmg.OperatorId);
                        var toolstate = Db.ToolStates.FirstOrDefault(p => p.Id == dmg.DamageId);
                        dmg.ToolBarcode = tool?.Barcode;
                        dmg.ToolName = tool?.Name;
                        dmg.DutyUserCardNo = dutyuser?.CardNo;
                        dmg.DutyUserName = dutyuser?.Name;
                        dmg.OperatorName = operateuser?.Name;
                        dmg.StateName = toolstate?.Name;
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

        public Result<bool> AddToolDamage(ToolDamage dmg)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var tool = Db.Tools.First(p => p.Id == dmg.ToolId);
                var toolstate = Db.ToolStates.First(p => p.Code == ToolStateCode.Damage);
                tool.StateId = toolstate.Id;
                Db.ToolDamages.Add(dmg);
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

        public Result<bool> EditToolDamage(ToolDamage dmg)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.Entry(dmg);
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

        public Result<bool> DeleteToolDamage(int id)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.ToolDamages.First(p => p.Id == id);
                Db.ToolDamages.Remove(old);
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
