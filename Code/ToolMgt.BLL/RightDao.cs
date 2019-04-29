using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Common;
using ToolMgt.Model;

namespace ToolMgt.BLL
{
    public class RightDao
    {
        private ToolMgtEntities Db;
        public RightDao()
        {
            Db = ContextFactory.GetContext();
        }

        public Result<List<Right>> GetRights()
        {
            Result<List<Right>> rlt = new Result<List<Right>>();
            try
            {
                rlt.Entities = Db.Rights.ToList();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }

        public Result<List<Right>> GetRightsByRoleId(int roleId)
        {
            Result<List<Right>> rlt = new Result<List<Right>>();
            try
            {
                var data = from rr in Db.RoleRights
                           join r in Db.Rights on rr.RightId equals r.Id
                           where rr.RoleId == roleId
                           select r;
                rlt.Entities = data.OrderBy(p => p.Id).ToList();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }

        public Result<bool> AddRight(Right right)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.Rights.FirstOrDefault(p => p.Code == right.Code);
                if (old != null)
                {
                    rlt.HasError = true;
                    rlt.Msg = "权限编码重复！";
                }
                else
                {
                    Db.Rights.Add(right);
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

        public Result<bool> EditRight(Right right)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.Entry(right);
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

        public Result<bool> DeleteRight(int id)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.Rights.First(p => p.Id == id);
                if (old.RoleRights.Count > 0)
                {
                    rlt.HasError = true;
                    rlt.Msg = "权限信息被使用，无法删除！";
                    return rlt;
                }
                Db.Rights.Remove(old);
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

        public Result<bool> EditRoleRight(int roleId, List<int> rightIds)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {

                Db.RoleRights.RemoveRange(Db.RoleRights.Where(p => p.RoleId == roleId));
                Db.SaveChanges();
                foreach (var rightId in rightIds)
                {
                    Db.RoleRights.Add(new RoleRight() { RoleId = roleId, RightId = rightId });
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

    }
}
