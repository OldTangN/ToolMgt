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
using CommonServiceLocator;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using MahApps.Metro.Controls;
using ToolMgt.Common;
using ToolMgt.UI.Common;

namespace ToolMgt.UI.View
{
    /// <summary>
    /// LogInWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : MetroWindow, IView
    {
        public LoginWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<bool>(this, MsgToken.Logined, Logined);
            this.Loaded += MetroWindow_Loaded;
            this.Closed += LoginWindow_Closed; ;
        }

        private void LoginWindow_Closed(object sender, EventArgs e)
        {
            CleanUp();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                int n = FaceBLL.FaceInit.sdk_init(false);
                // 测试是否授权
                bool authed = FaceBLL.FaceInit.is_auth();
                if (!authed)
                {
                    MessageAlert.Alert("人脸识别模块授权过期 或 未授权！");
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                MessageAlert.Alert("人脸识别模块初始化失败！" + ex.Message);
            }
        }

        private void Logined(bool rlt)
        {
            if (rlt)
            {
                this.Dispatcher.Invoke(() =>
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    Application.Current.MainWindow = mainWindow;
                    this.Close();
                });
            }
        }

        private void txtName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtName.Text = "";
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void pbPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Messenger.Default.Send<object>(null, MsgToken.Login);
            }
        }

        private void pbPwd_GotFocus(object sender, RoutedEventArgs e)
        {
            pbPwd.Password = "";
        }

        private void pbPwd_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtPwd.Text = pbPwd.Password;
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            txtPwd.Focus();
            txtPwd.SelectAll();
        }

        public void CleanUp()
        {
            Messenger.Default.Unregister(this);
            (this.DataContext as GalaSoft.MvvmLight.ViewModelBase)?.Cleanup();
        }
    }
}
