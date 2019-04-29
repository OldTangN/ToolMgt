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
    /// ChangePwdWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChangePwdWindow : MetroWindow,IView
    {
        public ChangePwdWindow()
        {
            InitializeComponent();
            this.Closed += ChangePwdWindow_Closed;
        }

        private void ChangePwdWindow_Closed(object sender, EventArgs e)
        {
            CleanUp();
        }

        private void CloseChangePwd(bool rlt)
        {
            this.DialogResult = rlt;
            this.Close();
        }

        public void CleanUp()
        {
        }

        private void BtnCommit_Click(object sender, RoutedEventArgs e)
        {
            if (pwdNew.Password != pwdRepeat.Password)
            {
                MessageAlert.Alert("新密码不一致！");
                return;
            }
            ToolMgt.BLL.UserDao userDao = new BLL.UserDao();
            var rlt= userDao.ChangePwd(Global.CurrUser.Id, pwdOld.Password, pwdNew.Password);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
                return;
            }
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
