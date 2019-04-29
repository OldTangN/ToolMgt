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
    /// ToolDamageInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ToolDamageInfoWindow : MetroWindow,IView
    {
        public ToolDamageInfoWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<bool>(this, MsgToken.CloseToolDamageInfo, CloseToolDamageInfo);
            Messenger.Default.Register<object>(this, MsgToken.OpenSelectTool, OpenSelectTool);
            Messenger.Default.Register<object>(this, MsgToken.OpenSelectUser, OpenSelectUser);
            this.Closed += ToolDamageInfoWindow_Closed;
        }
                

        private void ToolDamageInfoWindow_Closed(object sender, EventArgs e)
        {
            CleanUp();
        }

        private void CloseToolDamageInfo(bool rlt)
        {
            if (rlt)
            {
                Messenger.Default.Send<object>(null, MsgToken.RefreshToolDamageList);
            }
            this.DialogResult = rlt;
            this.Close();
        }

        void OpenSelectUser(object obj)
        {
            SelectUserWindow win = new SelectUserWindow();
            win.Owner = this;
            win.ShowDialog();
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
