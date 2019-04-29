using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Common;
using ToolMgt.Model;

namespace ToolMgt.BLL
{
    public class ToolPurchaseDao
    {
        private ToolMgtEntities Db;
        public ToolPurchaseDao()
        {
            Db = ContextFactory.GetContext();
        }

        private void SetExtProperty(List<ToolPurchaseOrder> rlt)
        {
            if (rlt != null && rlt.Count > 0)
            {
                foreach (var r in rlt)
                {
                    r.ApproveUserName = Db.Users.FirstOrDefault(p => p.Id == r.ApproveUserId)?.Name;
                    r.OperatorName = Db.Users.FirstOrDefault(p => p.Id == r.OperatorId)?.Name;
                }
            }
        }

        public Result<List<ToolPurchaseOrder>> GetToolPurchaseOrders()
        {
            Result<List<ToolPurchaseOrder>> rlt = new Result<List<ToolPurchaseOrder>>();
            try
            {
                var tmp = Db.ToolPurchaseOrders.ToList();
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

        public Result<bool> AddToolPurchaseOrder(ToolPurchaseOrder order)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                Db.ToolPurchaseOrders.Add(order);//TODO:测试明细会不会同步保存
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

        public Result<bool> EditToolPurchaseOrder(ToolPurchaseOrder ord)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.Entry(ord);
                old.State = System.Data.Entity.EntityState.Modified;

                Db.ToolPurchaseOrderDtls.RemoveRange(Db.ToolPurchaseOrderDtls.Where(p => p.OrderId == ord.Id));
                Db.ToolPurchaseOrderDtls.AddRange(ord.OrderDtls);
                
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

        public Result<bool> DeleteToolPurchaseOrder(int ordId)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.ToolPurchaseOrders.First(p => p.Id == ordId);
                Db.ToolPurchaseOrders.Remove(old);
                Db.ToolPurchaseOrderDtls.RemoveRange(Db.ToolPurchaseOrderDtls.Where(p => p.OrderId == ordId));
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
