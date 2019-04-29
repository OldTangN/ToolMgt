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
    /// UserShow.xaml 的交互逻辑
    /// </summary>
    public partial class UserInfoWindow : MetroWindow, IView
    {
        public UserInfoWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<bool>(this, MsgToken.CloseUserInfo, CloseUserInfo);
            Messenger.Default.Register<object>(this, MsgToken.TakePic, TakePic);
            Messenger.Default.Register<object>(this, MsgToken.RefreshCamera, RefreshCamera);
            Messenger.Default.Register<string>(this, MsgToken.ShowImage, ShowImage);
            this.Closed += UserInfoWindow_Closed;
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
            catch (Exception ex)
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
                catch (Exception ex)
                {
                }
            }
        }

        private void UserInfoWindow_Closed(object sender, EventArgs e)
        {
            CleanUp();
        }

        private void CloseUserInfo(bool rlt)
        {
            if (rlt)
            {
                Messenger.Default.Send<object>(null, MsgToken.RefreshUserList);
            }
            this.DialogResult = rlt;
            this.Close();
        }

        public void CleanUp()
        {
            CameraHelper.CloseDevice();
            Messenger.Default.Unregister(this);
            (this.DataContext as GalaSoft.MvvmLight.ViewModelBase)?.Cleanup();
        }
    }
}
