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
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using ToolMgt.UI.Common;

namespace ToolMgt.UI.View
{
    /// <summary>
    /// ToolStateInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ToolStateInfoWindow : MetroWindow,IView
    {
        public ToolStateInfoWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<bool>(this, MsgToken.CloseToolStateInfo, CloseToolStateInfo);
            this.Closed += ToolStateInfoWindow_Closed;
        }

        private void ToolStateInfoWindow_Closed(object sender, EventArgs e)
        {
            CleanUp();
        }

        private void CloseToolStateInfo(bool rlt)
        {
            if (rlt)
            {
                Messenger.Default.Send<object>(null, MsgToken.RefreshToolStateList);
            }
            this.DialogResult = rlt;
            this.Close();
        }

        public void CleanUp()
        {
            Messenger.Default.Unregister(this);
            (this.DataContext as GalaSoft.MvvmLight.ViewModelBase)?.Cleanup();
        }
    }
}
