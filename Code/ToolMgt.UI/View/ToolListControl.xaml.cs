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
    /// ToolListControl.xaml 的交互逻辑
    /// </summary>
    public partial class ToolListControl : UserControl, IView
    {
        public ToolListControl()
        {
            InitializeComponent();
            Messenger.Default.Register<object>(this, MsgToken.OpenToolInfo, OpenToolInfoWin);
        }

        private void OpenToolInfoWin(object obj)
        {
            ToolInfoWindow win = new ToolInfoWindow();
            win.Owner = Window.GetWindow(this);
            Messenger.Default.Send(obj, MsgToken.TransforTool);
            win.ShowDialog();
        }


        public void CleanUp()
        {
            Messenger.Default.Unregister(this);
            (this.DataContext as GalaSoft.MvvmLight.ViewModelBase)?.Cleanup();
        }
    }
}
