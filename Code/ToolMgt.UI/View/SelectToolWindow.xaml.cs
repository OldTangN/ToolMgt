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
    /// SelectToolWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SelectToolWindow : MetroWindow,IView
    {
        public SelectToolWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<bool>(this, MsgToken.CloseSelectTool, CloseSelectTool);
            this.Closed += SelectToolWindow_Closed;
        }

        private void SelectToolWindow_Closed(object sender, EventArgs e)
        {
            CleanUp();
        }
        void CloseSelectTool(bool rlt)
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
