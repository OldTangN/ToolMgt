using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolMgt.Helper.ICCard
{
    public interface ICardHelper
    {
        event EventHandler<DataEventArgs> HandDataBack;

        bool IsOpen();

        void Read();

        void Close();
    }
}
