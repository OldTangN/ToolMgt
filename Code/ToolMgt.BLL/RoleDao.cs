using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Common;
using ToolMgt.Model;

namespace ToolMgt.BLL
{
    public class RoleDao
    {
        private ToolMgtEntities Db;
        public RoleDao()
        {
            Db = ContextFactory.GetContext();
        }

        public Result<List<Role>> GetRoles()
        {
            Result<List<Role>> rlt = new Result<List<Role>>();
            try
            {
                rlt.Entities = Db.Roles.ToList();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }

        public Result<bool> AddRole(Role role)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.Roles.FirstOrDefault(p => p.Code == role.Code);
                if (old != null)
                {
                    rlt.HasError = true;
                    rlt.Msg = "角色编码重复！";
                }
                else
                {
                    Db.Roles.Add(role);
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

        public Result<bool> EditRole(Role role)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.Entry(role);
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

        public Result<bool> DeleteRole(int id)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.Roles.First(p => p.Id == id);
                //if (old.Users.Count > 0 || old.RoleRights.Count > 0)
                //{
                //    rlt.HasError = true;
                //    rlt.Msg = "角色信息被使用，无法删除！";
                //    return rlt;
                //}
                Db.Roles.Remove(old);
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
