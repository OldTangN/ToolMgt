using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolMgt.BLL
{
    /// <summary>
    /// 结果类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T>
    {
        public Result()
        {
           
        }
        public Result(T entities)
        {
            Entities = entities;
        }

        public T Entities { get; set; }
        public string Msg { get; set; } = "";
        public bool HasError { get; set; } = false;
    }
}
