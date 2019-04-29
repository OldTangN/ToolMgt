using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Common;
using ToolMgt.Model;

namespace ToolMgt.BLL
{
    public class DutyDao
    {
        private ToolMgtEntities Db;
        public DutyDao()
        {
            Db = ContextFactory.GetContext();
        }

        public Result<List<Duty>> GetDuties()
        {
            Result<List<Duty>> rlt = new Result<List<Duty>>();
            try
            {
                rlt.Entities = Db.Duties.ToList();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }

        public Result<bool> AddDuty(Duty duty)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.Duties.FirstOrDefault(p => p.Code == duty.Code);
                if (old != null)
                {
                    rlt.HasError = true;
                    rlt.Msg = "职务编码重复！";
                }
                else
                {
                    Db.Duties.Add(duty);
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

        public Result<bool> EditDuty(Duty duty)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.Entry(duty);
                old.State = System.Data.Entity.EntityState.Modified;
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex); rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }

        public Result<bool> DeleteDuty(int id)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.Duties.First(p => p.Id == id);
                //if (old.Users.Count > 0)
                //{
                //    rlt.HasError = true;
                //    rlt.Msg = "职务信息被使用，无法删除！";
                //    return rlt;
                //}
                Db.Duties.Remove(old);
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
