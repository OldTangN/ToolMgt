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
    /// DeptListControl.xaml 的交互逻辑
    /// </summary>
    public partial class DeptListControl : UserControl, IView
    {
        public DeptListControl()
        {
            InitializeComponent();
            Messenger.Default.Register<object>(this, MsgToken.OpenDeptInfo, OpenDeptInfoWin);
        }

        private void OpenDeptInfoWin(object obj)
        {
            DeptInfoWindow win = new DeptInfoWindow();
            win.Owner = Window.GetWindow(this);
            Messenger.Default.Send(obj, MsgToken.TransforDept);
            win.ShowDialog();
        }
        public void CleanUp()
        {
            Messenger.Default.Unregister(this);
            (this.DataContext as GalaSoft.MvvmLight.ViewModelBase)?.Cleanup();
        }
    }
}
