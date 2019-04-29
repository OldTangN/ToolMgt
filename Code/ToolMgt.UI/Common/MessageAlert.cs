using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
namespace ToolMgt.UI.Common
{
    public static class MessageAlert
    {
        /// <summary>
        /// 弹出框，异步、不阻塞线程
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="title"></param>
        public static void Alert(string msg, string title = "提示")
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MetroWindow win = Application.Current.MainWindow as MetroWindow;
                if (win == null)
                {
                    MessageBox.Show(msg, title);
                    return;
                }
                while (win.OwnedWindows.Count > 0)
                {
                    win = (win.OwnedWindows[0] as MetroWindow) ?? win;
                }
                win.MetroDialogOptions.AffirmativeButtonText = "确定";
                win.MetroDialogOptions.NegativeButtonText = "取消";
                win.ShowMessageAsync(title, msg, MessageDialogStyle.Affirmative, win.MetroDialogOptions);
            });
        }

        /// <summary>
        /// 确认对话框，同步、阻塞线程
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static async Task<bool> Confirm(string msg, string title = "确认？")
        {
            MetroWindow win = Application.Current.MainWindow as MetroWindow;
            if (win == null)
            {
                var dialogrlt = MessageBox.Show(msg, title, MessageBoxButton.OKCancel);
                return dialogrlt == MessageBoxResult.OK;
            }
            while (win.OwnedWindows.Count > 0)
            {
                win = (win.OwnedWindows[0] as MetroWindow) ?? win;
            }
            win.MetroDialogOptions.AffirmativeButtonText = "确定";
            win.MetroDialogOptions.NegativeButtonText = "取消";
            var rlt = await win.ShowMessageAsync(title, msg, MessageDialogStyle.AffirmativeAndNegative, win.MetroDialogOptions);
            return rlt == MessageDialogResult.Affirmative;
        }
    }
}
