using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ToolMgt.BLL;
using ToolMgt.Common;
using ToolMgt.Helper;
using ToolMgt.Helper.ICCard;
using ToolMgt.Model;
using ToolMgt.UI.Common;
using ToolMgt.UI.Controls;
using ToolMgt.UI.View;

namespace ToolMgt.UI.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {

        public LoginViewModel()
        {
            MessengerInstance.Register<object>(this, MsgToken.Login, Login);
            CurrLogIn = new LogInModel();
            if (App.ICCardReader.IsOpen())
            {
                App.ICCardReader.HandDataBack += ICCardReader_HandDataBack;
            }
            else
            {
                ErrMsg += "IC读卡器连接失败！ ";
            }
            if (!App.RFIDReader.IsOpen())
            {
                ErrMsg += "RFID读卡器连接失败！";
            }
        }

        private void ICCardReader_HandDataBack(object sender, DataEventArgs e)
        {
            CurrLogIn.CodeOrCard = e.Data;
            CurrLogIn.IsCard = true;
            Login(null);
        }

        public LogInModel CurrLogIn { get; set; }

        private string _ErrMsg = "";
        public string ErrMsg
        {
            get => _ErrMsg;
            set => Set(ref _ErrMsg, value);
        }

        #region 登录按钮
        private ICommand _logInCmd;
        public ICommand LogInCmd => _logInCmd ?? (_logInCmd = new RelayCommand<object>(Login, CanLogIn));

        private void Login(object obj)
        {
            UserDao dao = new UserDao();
            var rlt = dao.LogIn(CurrLogIn);
            if (rlt.HasError)
            {
                MessageAlert.Alert(rlt.Msg);
            }
            else
            {
                Global.CurrUser = rlt.Entities;
                var rightRlt = dao.GetUserRights(Global.CurrUser.Id);
                if (rightRlt.HasError)
                {
                    MessageAlert.Alert(rightRlt.Msg);
                    return;
                }
                Global.CurrRights = rightRlt.Entities;
                if (Global.CurrRights == null && Global.CurrRights.Count == 0)
                {
                    MessageAlert.Alert("用户没有任何权限！");
                    return;
                }
                MessengerInstance.Send(true, MsgToken.Logined);//发送登录成功消息              
            }
        }

        private bool CanLogIn(object obj)
        {
            return !string.IsNullOrEmpty(CurrLogIn.CodeOrCard);
        }
        #endregion

        public override void Cleanup()
        {
            MessengerInstance.Unregister(this);
            App.ICCardReader.HandDataBack -= ICCardReader_HandDataBack;
            base.Cleanup();
        }
    }
}
