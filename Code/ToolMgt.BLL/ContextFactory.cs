using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Model;

namespace ToolMgt.BLL
{
    internal static class ContextFactory
    {
        public static ToolMgtEntities GetContext()
        {
            return new ToolMgtEntities();
            if (context == null)
            {
                lock (lockobj)
                {
                    if (context == null)
                    {
                        context = new ToolMgtEntities();
                        context.Database.Connection.ConnectionString = "data source=localhost;initial catalog=ToolMgt;user id=sa;password=123;MultipleActiveResultSets=True;App=EntityFramework";
                    }
                }
            }
            return context;
        }

        private static ToolMgtEntities context;
        private static readonly object lockobj = new object();
    }
}
