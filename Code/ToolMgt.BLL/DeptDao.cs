using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Common;
using ToolMgt.Model;

namespace ToolMgt.BLL
{
    public class DeptDao
    {
        private ToolMgtEntities Db;
        public DeptDao()
        {
            Db = ContextFactory.GetContext();
        }

        public Result<List<Department>> GetDepartments()
        {
            Result<List<Department>> rlt = new Result<List<Department>>();
            try
            {
                rlt.Entities = Db.Departments.ToList();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }

        public Result<bool> AddDepartment(Department dept)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.Departments.FirstOrDefault(p => p.Code == dept.Code);
                if (old != null)
                {
                    rlt.HasError = true;
                    rlt.Msg = "部门编码重复！";
                }
                else
                {
                    Db.Departments.Add(dept);
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

        public Result<bool> EditDepartment(Department dept)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.Entry(dept);
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
        public Result<bool> DeleteDepartment(int id)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.Departments.First(p => p.Id == id);
                //if (old.Users.Count > 0)
                //{
                //    rlt.HasError = true;
                //    rlt.Msg = "部门信息被使用，无法删除！";
                //    return rlt;
                //}
                Db.Departments.Remove(old);
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
