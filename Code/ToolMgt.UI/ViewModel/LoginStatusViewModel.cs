using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ToolMgt.BLL;
using ToolMgt.Helper;
using ToolMgt.Model;
using ToolMgt.UI.Common;

namespace ToolMgt.UI.ViewModel
{
    public class LoginStatusViewModel : ViewModelBase
    {
        public LoginStatusViewModel()
        {
            CurrUser = Global.CurrUser;
        }

        public User CurrUser { get => _currUser; set => Set(ref _currUser, value); }

        #region CommitCmd
        private RelayCommand _commitCmd;
        public RelayCommand CommitCmd
        {
            get
            {
                if (_commitCmd == null)
                {
                    _commitCmd = new RelayCommand(Commit);
                }
                return _commitCmd;
            }
        }

        private void Commit()
        {
            MessengerInstance.Send(true, MsgToken.CloseLoginStatus);
        }
        #endregion
       
        public override void Cleanup()
        {
            MessengerInstance.Unregister(this);
            base.Cleanup();
        }
        private User _currUser;
    }
}
