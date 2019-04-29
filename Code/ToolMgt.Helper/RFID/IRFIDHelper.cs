using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolMgt.Helper.RFID
{
    public interface IRFIDHelper
    {
        event EventHandler<DataEventArgs> HandDataBack;

        bool IsOpen();

        void Read();

        void Close();
    }
}
