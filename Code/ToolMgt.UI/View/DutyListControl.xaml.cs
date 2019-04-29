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
    /// DutyListControl.xaml 的交互逻辑
    /// </summary>
    public partial class DutyListControl : UserControl, IView
    {
        public DutyListControl()
        {
            InitializeComponent();
            Messenger.Default.Register<object>(this, MsgToken.OpenDutyInfo, OpenDutyInfoWin);
        }

        private void OpenDutyInfoWin(object obj)
        {
            DutyInfoWindow win = new DutyInfoWindow();
            win.Owner = Window.GetWindow(this);
            Messenger.Default.Send(obj, MsgToken.TransforDuty);
            win.ShowDialog();
        }

        public void CleanUp()
        {
            Messenger.Default.Unregister(this);
            (this.DataContext as GalaSoft.MvvmLight.ViewModelBase)?.Cleanup();
        }
    }
}
