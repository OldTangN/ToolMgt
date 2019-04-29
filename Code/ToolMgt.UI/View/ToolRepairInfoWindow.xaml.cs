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
    /// ToolRepairInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ToolRepairInfoWindow : MetroWindow,IView 
    {
        public ToolRepairInfoWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<bool>(this, MsgToken.CloseToolRepairInfo, CloseToolRepairInfo);
            Messenger.Default.Register<object>(this, MsgToken.OpenSelectTool, OpenSelectTool);
            this.Closed += ToolRepairInfoWindow_Closed;
        }

        private void ToolRepairInfoWindow_Closed(object sender, EventArgs e)
        {
            CleanUp();
        }

        private void CloseToolRepairInfo(bool rlt)
        {
            if (rlt)
            {
                Messenger.Default.Send<object>(null, MsgToken.RefreshToolRepairList);
            }
            this.DialogResult = rlt;
            this.Close();
        }
        void OpenSelectTool(object obj)
        {
            SelectToolWindow win = new SelectToolWindow();
            win.Owner = this;
            win.ShowDialog();
        }

        public void CleanUp()
        {
            Messenger.Default.Unregister(this);
            (this.DataContext as GalaSoft.MvvmLight.ViewModelBase)?.Cleanup();
        }
    }
}
