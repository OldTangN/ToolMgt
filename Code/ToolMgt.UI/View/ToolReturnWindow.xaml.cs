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
    /// TooReturnWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ToolReturnWindow : MetroWindow,IView
    {
        public ToolReturnWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<bool>(this, MsgToken.CloseToolReturn, CloseToolReturn);
            Messenger.Default.Register<object>(this, MsgToken.OpenSelectUser, OpenSelectUser);
            Messenger.Default.Register<object>(this, MsgToken.OpenSelectTool, OpenSelectTool);
            Messenger.Default.Register<object>(this, MsgToken.TakePic, TakePic);
            Messenger.Default.Register<object>(this, MsgToken.RefreshCamera, RefreshCamera);
            Messenger.Default.Register<string>(this, MsgToken.ShowImage, ShowImage);
            this.Closed += ReturnInfoWindow_Closed;
            CameraHelper.IsDisplay = true;
            CameraHelper.SourcePlayer = player;
            CameraHelper.UpdateCameraDevices();
            if (CameraHelper.CameraDevices.Count > 0)
            {
                CameraHelper.SetCameraDevice(0);
            }
        }
        private void TakePic(object obj)
        {
            try
            {
                string fullPath = CameraHelper.CaptureImage(AppDomain.CurrentDomain.BaseDirectory + @"\Capture");
                Messenger.Default.Send<string>(fullPath, MsgToken.TransforImgPath);
                ShowImage(fullPath);
                imgUser.Visibility = Visibility.Visible;
                videoHost.Visibility = Visibility.Collapsed;
            }
            catch (Exception)
            {
            }
        }

        private void RefreshCamera(object obj)
        {
            try
            {
                videoHost.Visibility = Visibility.Visible;
                imgUser.Visibility = Visibility.Collapsed;

                CameraHelper.IsDisplay = true;
                CameraHelper.SourcePlayer = player;
                CameraHelper.UpdateCameraDevices();
                if (CameraHelper.CameraDevices.Count > 0)
                {
                    CameraHelper.SetCameraDevice(0);
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 显示图片，并不锁定文件
        /// </summary>
        /// <param name="path"></param>
        private void ShowImage(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    var bitmapimg = new BitmapImage();
                    bitmapimg.BeginInit();
                    bitmapimg.CacheOption = BitmapCacheOption.OnLoad;
                    System.IO.MemoryStream stream = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(path));
                    bitmapimg.StreamSource = stream;
                    bitmapimg.EndInit();
                    bitmapimg.Freeze();
                    imgUser.Source = bitmapimg;
                }
                catch (Exception)
                {
                }
            }
        }

        private void ReturnInfoWindow_Closed(object sender, EventArgs e)
        {
            CleanUp();
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

        private void CloseToolReturn(bool rlt)
        {
            if (rlt)
            {
                Messenger.Default.Send<object>(null, MsgToken.RefreshToolRecordList);
            }
            this.DialogResult = rlt;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void CtxMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (ctxMenu.PlacementTarget == null)
            {

            }
        }

        public void CleanUp()
        {
            Messenger.Default.Unregister(this);
            (this.DataContext as GalaSoft.MvvmLight.ViewModelBase)?.Cleanup();
        }
    }
}
