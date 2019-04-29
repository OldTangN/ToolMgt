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
    /// SystemSetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SystemSetWindow : MetroWindow, IView
    {
        public SystemSetWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<bool>(this, MsgToken.CloseSystemSet, CloseSystemSet);
            this.Closed += SystemSetWindow_Closed;
        }

        private void SystemSetWindow_Closed(object sender, EventArgs e)
        {
            CleanUp();
        }

        private void CloseSystemSet(bool rlt)
        {
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
