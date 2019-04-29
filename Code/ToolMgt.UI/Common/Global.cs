using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolMgt.Model;

namespace ToolMgt.UI.Common
{
    public class Global
    {
        public static User CurrUser;
        public static List<Right> CurrRights;
        public static bool HasRight(string rightCode)
        {
            var right = CurrRights.FirstOrDefault(p => p.Code == rightCode);
            if (right != null)
            {
                return true;
            }
            return false;
        }
    }
}
