using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Common;
using ToolMgt.Model;

namespace ToolMgt.BLL
{
    public class ToolDao
    {
        public ToolDao()
        {
        }

        /// <summary>
        /// 获取未被其他工具包占用的工具
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public Result<Tool> GetToolIsNotChild(string barcode)
        {
            Result<Tool> rlt = new Result<Tool>();
            using (ToolMgtEntities Db = ContextFactory.GetContext())
            {
                try
                {
                    var tool = Db.Tools.FirstOrDefault(p => p.Barcode == barcode);
                    var pkgItem = Db.ToolPkgItems.FirstOrDefault(p => p.ChildrenId == tool.Id);
                    if (pkgItem == null)
                    {
                        rlt.Entities = tool;
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    rlt.HasError = true;
                    rlt.Msg = ex.Message;
                }
            }

            return rlt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="barCode">条码</param>
        /// <param name="stateId">状态id</param>
        /// <param name="categroyId">类别id</param>
        /// <returns></returns>
        public Result<List<Tool>> GetTools(string barCode, string name = "", int stateId = 0, int categroyId = 0, bool SearchPkg = false)
        {
            Result<List<Tool>> rlt = new Result<List<Tool>>();
            using (ToolMgtEntities Db = ContextFactory.GetContext())
            {
                try
                {
                    rlt.Entities = Db.Tools.Include("ToolCategory").Include("ToolState").Where(p => (p.StateId == stateId || stateId == 0)
                                                && (p.CategoryId == categroyId || categroyId == 0)
                                                && p.Barcode.Contains(barCode)
                                                && p.Name.Contains(name)
                                                && (p.IsPkg == SearchPkg || !SearchPkg)).ToList();
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    rlt.HasError = true;
                    rlt.Msg = ex.Message;
                }
            }
            return rlt;
        }

        public Result<Tool> GetToolByBarcode(string barcode)
        {
            Result<Tool> rlt = new Result<Tool>();
            using (ToolMgtEntities Db = ContextFactory.GetContext())
            {
                try
                {
                    rlt.Entities = Db.Tools.Include("ToolCategory").Include("ToolState").FirstOrDefault(p => p.Barcode == barcode);
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    rlt.HasError = true;
                    rlt.Msg = ex.Message;
                }
            }
            return rlt;
        }

        public Result<Tool> GetToolByRFID(string rfid)
        {
            Result<Tool> rlt = new Result<Tool>();
            using (ToolMgtEntities Db = ContextFactory.GetContext())
            {
                try
                {
                    rlt.Entities = Db.Tools.Include("ToolCategory").Include("ToolState").FirstOrDefault(p => p.RFID == rfid);
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    rlt.HasError = true;
                    rlt.Msg = ex.Message;
                }
            }
            return rlt;
        }

        public Result<bool> AddTool(Tool tool)
        {
            Result<bool> rlt = new Result<bool>();
            using (ToolMgtEntities Db = ContextFactory.GetContext())
            {
                try
                {
                    var old = Db.Tools.FirstOrDefault(p => p.Barcode == tool.Barcode);
                    if (old != null)
                    {
                        rlt.HasError = true;
                        rlt.Msg = "工具条码重复！";
                        return rlt;
                    }
                    if (!string.IsNullOrEmpty(tool.RFID))
                    {
                        old = Db.Tools.FirstOrDefault(p => p.RFID == tool.RFID);
                        if (old != null)
                        {
                            rlt.HasError = true;
                            rlt.Msg = "工具RFID重复！";
                            return rlt;
                        }
                    }
                    if (tool.IsPkg)
                    {
                        //判断重复使用的子工具
                        if (tool.PkgItems != null && tool.PkgItems.Count > 0)
                        {
                            foreach (var chld in tool.PkgItems)
                            {
                                var exist = Db.ToolPkgItems.FirstOrDefault(p => p.ChildrenId == chld.ChildrenId);
                                if (exist != null)
                                {
                                    rlt.HasError = true;
                                    rlt.Msg = $"Id为【{ chld.ChildrenId }】的工具，已被其他工具包使用！";
                                    return rlt;
                                }
                            }
                        }
                    }

                    Db.Tools.Add(tool);
                    Db.SaveChanges();//保存后才会得到工具包Id

                    if (tool.PkgItems != null && tool.PkgItems.Count > 0)
                    {
                        tool.PkgItems.ForEach(p => p.ParentId = tool.Id);//设置工具包Id
                        Db.ToolPkgItems.AddRange(tool.PkgItems);
                        Db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    rlt.HasError = true;
                    rlt.Msg = ex.Message;
                }
            }
            return rlt;
        }

        public Result<bool> EditTool(Tool tool)
        {
            Result<bool> rlt = new Result<bool>();
            using (ToolMgtEntities Db = ContextFactory.GetContext())
            {
                try
                {
                    var old = Db.Tools.FirstOrDefault(p => p.Barcode == tool.Barcode && p.Id != tool.Id);
                    if (old != null)
                    {
                        rlt.HasError = true;
                        rlt.Msg = "工具条码重复！";
                        return rlt;
                    }
                    if (!string.IsNullOrEmpty(tool.RFID))
                    {
                        old = Db.Tools.FirstOrDefault(p => p.RFID == tool.RFID && p.Id != tool.Id);
                        if (old != null)
                        {
                            rlt.HasError = true;
                            rlt.Msg = "工具RFID重复！";
                            return rlt;
                        }
                    }

                    #region 处理工具包
                    if (tool.IsPkg)
                    {
                        //判断重复使用的子工具
                        if (tool.PkgItems != null && tool.PkgItems.Count > 0)
                        {
                            foreach (var chld in tool.PkgItems)
                            {
                                var exist = Db.ToolPkgItems.FirstOrDefault(p => p.ChildrenId == chld.ChildrenId);
                                if (exist != null && exist.ParentId != tool.Id)
                                {
                                    rlt.HasError = true;
                                    rlt.Msg = $"Id为【{ chld.ChildrenId }】的工具，已被其他工具包使用！";
                                    return rlt;
                                }
                            }
                        }
                    }
                    Db.ToolPkgItems.RemoveRange(Db.ToolPkgItems.Where(p => p.ParentId == tool.Id));//删除旧的子工具
                    if (tool.PkgItems != null && tool.PkgItems.Count > 0)//重新加入子工具
                    {
                        Db.ToolPkgItems.AddRange(tool.PkgItems);
                    }
                    #endregion

                    tool.ToolState = Db.ToolStates.FirstOrDefault(p => p.Id == tool.StateId);
                    tool.ToolCategory = Db.ToolCategories.FirstOrDefault(p => p.Id == tool.CategoryId);
                    var oldEntity = Db.Entry(tool);
                    oldEntity.State = System.Data.Entity.EntityState.Modified;
                    Db.SaveChanges();
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    rlt.HasError = true;
                    rlt.Msg = ex.Message;
                }
            }
            return rlt;
        }

        public Result<bool> DeleteTool(int id)
        {
            Result<bool> rlt = new Result<bool>();
            using (ToolMgtEntities Db = ContextFactory.GetContext())
            {
                try
                {
                    var old = Db.Tools.First(p => p.Id == id);
                    //if (old.Tools.Count > 0)
                    //{
                    //    rlt.HasError = true;
                    //    rlt.Msg = "工具信息被使用，无法删除！";
                    //    return rlt;
                    //}
                    var olditems = Db.ToolPkgItems.Where(p => p.ParentId == id);
                    if (olditems != null && olditems.Count() > 0)
                    {
                        Db.ToolPkgItems.RemoveRange(olditems);//删除工具包信息
                    }
                    Db.Tools.Remove(old);
                    Db.SaveChanges();
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    rlt.HasError = true;
                    rlt.Msg = ex.Message;
                }
            }
            return rlt;
        }
    }
}
