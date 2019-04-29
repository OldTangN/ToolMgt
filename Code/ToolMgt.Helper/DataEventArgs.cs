using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolMgt.Helper
{
    public class DataEventArgs : EventArgs
    {
        public string Data { set; get; }

        public DataEventArgs(string _data) : base()
        {
            this.Data = _data;
        }
    }
}
