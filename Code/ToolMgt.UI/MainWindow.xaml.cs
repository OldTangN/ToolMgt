using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ToolMgt.UI.Common;
using ToolMgt.UI.Controls;
using ToolMgt.UI.View;

namespace ToolMgt.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow,IView
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.Closed += MainWindow_Closed; ;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            CleanUp();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //TODO:已测试 重复注册会多次运行
            //Messenger.Default.Register<object>(this, "test", test);
            //Messenger.Default.Register<object>(this, "test", test);
            //Messenger.Default.Register<object>(this, "test", test);
            //Messenger.Default.Register<object>(this, "test", test);
        }

        //int i = 0;
        //private void test(object obj)
        //{
        //    i++;
        //    MessageBox.Show(i.ToString());
        //}

        private void MiLogStatus_Click(object sender, RoutedEventArgs e)
        {
            LoginStatusWindow win = new LoginStatusWindow();
            win.Owner = this;
            win.ShowDialog();
        }

        private void MiChangePwd_Click(object sender, RoutedEventArgs e)
        {
            ChangePwdWindow win = new ChangePwdWindow();
            win.Owner = this;
            win.ShowDialog();
        }
        private async void MiExit_Click(object sender, RoutedEventArgs e)
        {
            var rlt = await MessageAlert.Confirm("确定退出系统？");
            if (rlt)
            {
                this.Close();
                Environment.Exit(0);
            }
            //Messenger.Default.Send<object>(null, "test");
            //ToolMgt.BLL.UserDao dao = new BLL.UserDao();
            //Model.User u = dao.GetUser(1).Entities;
            //Model.User u2 = new Model.User()
            //{
            //    Id = u.Id,
            //    Code = u.Code,
            //    Name = u.Name,
            //    Note = u.Note,
            //    CardNo = u.CardNo,
            //    DeptId = u.DeptId,
            //    DutyId = u.DutyId,
            //    IsDel = u.IsDel,
            //    Phone = u.Phone,
            //    Pwd = u.Pwd
            //};
            //u2.Note = u2.Note + "&";
            //BLL.UserDao dao2 = new BLL.UserDao();
            //dao2.EditUser(u);
        }

        private void MiUserMgt_Click(object sender, RoutedEventArgs e)
        {
            ShowView(new UserListControl());
        }

        private void MiDeptMgt_Click(object sender, RoutedEventArgs e)
        {
            ShowView(new DeptListControl());
        }

        private void MiDutyMgt_Click(object sender, RoutedEventArgs e)
        {
            ShowView(new DutyListControl());
        }

        private void MiToolState_Click(object sender, RoutedEventArgs e)
        {
            ShowView(new ToolStateListControl());
        }

        private void ShowView(UIElement ui)
        {
            (gridCenter.Child as IView)?.CleanUp();
            gridCenter.Child = ui;
        }

        private void MiRoleMgt_Click(object sender, RoutedEventArgs e)
        {
            ShowView(new RoleListControl());
        }

        private void MiRightMgt_Click(object sender, RoutedEventArgs e)
        {
            ShowView(new RightListControl());
        }

        private void MiToolCategory_Click(object sender, RoutedEventArgs e)
        {
            ShowView(new ToolCategoryListControl());
        }

        private void MiToolMgt_Click(object sender, RoutedEventArgs e)
        {
            ShowView(new ToolListControl());
        }

        private void MiToolBorrow_Click(object sender, RoutedEventArgs e)
        {
            ShowView(new ToolRecordListControl());
        }


        private void MiToolDamage_Click(object sender, RoutedEventArgs e)
        {
            ShowView(new ToolDamageListControl());
        }

        private void MiToolRepair_Click(object sender, RoutedEventArgs e)
        {
            ShowView(new ToolRepairListControl());
        }

        public void CleanUp()
        {
            Messenger.Default.Unregister(this);
            (this.DataContext as GalaSoft.MvvmLight.ViewModelBase)?.Cleanup();
        }

        private void MiRightSet_Click(object sender, RoutedEventArgs e)
        {
            RightSetWindow win = new RightSetWindow();
            win.Owner = this;
            win.ShowDialog();
        }

        private void MiSystemSet_Click(object sender, RoutedEventArgs e)
        {
            SystemSetWindow win = new SystemSetWindow();
            win.Owner = this;
            win.ShowDialog();
        }
    }
}
