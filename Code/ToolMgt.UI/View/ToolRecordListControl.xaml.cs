using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using ToolMgt.UI.Common;

namespace ToolMgt.UI.View
{
    /// <summary>
    /// BorrowListControl.xaml 的交互逻辑
    /// </summary>
    public partial class ToolRecordListControl : UserControl, IView
    {
        public ToolRecordListControl()
        {
            InitializeComponent();
            Messenger.Default.Register<object>(this, MsgToken.OpenToolBorrow, OpenToolBorrow);
                       Messenger.Default.Register<object>(this, MsgToken.OpenToolReturn, OpenToolReturn);
        }

        private void OpenToolBorrow(object obj)
        {
            ToolBorrowWindow win = new ToolBorrowWindow();
            win.Owner = Window.GetWindow(this);
            win.ShowDialog();
        }
        private void OpenToolReturn(object obj)
        {
            ToolReturnWindow win = new ToolReturnWindow();
            win.Owner = Window.GetWindow(this);
            win.ShowDialog();
        }
            
        public void CleanUp()
        {
            Messenger.Default.Unregister(this);
            (this.DataContext as GalaSoft.MvvmLight.ViewModelBase)?.Cleanup();
        }
    }
}
