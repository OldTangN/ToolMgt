/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:ToolMgt.UI"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace ToolMgt.UI.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();

            SimpleIoc.Default.Register<DeptListViewModel>();
            SimpleIoc.Default.Register<DeptInfoViewModel>();

            SimpleIoc.Default.Register<DutyListViewModel>();
            SimpleIoc.Default.Register<DutyInfoViewModel>();

            SimpleIoc.Default.Register<ToolStateListViewModel>();
            SimpleIoc.Default.Register<ToolStateInfoViewModel>();

            SimpleIoc.Default.Register<RoleListViewModel>();
            SimpleIoc.Default.Register<RoleInfoViewModel>();

            SimpleIoc.Default.Register<RightListViewModel>();
            SimpleIoc.Default.Register<RightInfoViewModel>();

            SimpleIoc.Default.Register<UserListViewModel>();
            SimpleIoc.Default.Register<UserInfoViewModel>();

            SimpleIoc.Default.Register<ToolListViewModel>();
            SimpleIoc.Default.Register<ToolInfoViewModel>();

            SimpleIoc.Default.Register<ToolCategoryListViewModel>();
            SimpleIoc.Default.Register<ToolCategoryInfoViewModel>();

            SimpleIoc.Default.Register<ToolBorrowViewModel>();
            SimpleIoc.Default.Register<ToolRecordListViewModel>();

            SimpleIoc.Default.Register<SelectUserViewModel>();
            SimpleIoc.Default.Register<SelectToolViewModel>();

            SimpleIoc.Default.Register<ToolReturnViewModel>();

            SimpleIoc.Default.Register<ToolDamageListViewModel>();
            SimpleIoc.Default.Register<ToolDamageInfoViewModel>();

            SimpleIoc.Default.Register<ToolRepairListViewModel>();
            SimpleIoc.Default.Register<ToolRepairInfoViewModel>();
            SimpleIoc.Default.Register<LoginStatusViewModel>();
        }

        private LoginViewModel _Login;
        public LoginViewModel Login
        {
            get
            {
                _Login?.Cleanup();
                _Login = new LoginViewModel();
                return _Login;
            }
        }

        private LoginStatusViewModel _LoginStatus;
        public LoginStatusViewModel LoginStatus
        {
            get
            {
                _LoginStatus?.Cleanup();
                _LoginStatus = new LoginStatusViewModel();
                return _LoginStatus;
            }
        }

        private MainViewModel _Main;
        public MainViewModel Main
        {
            get
            {
                _Main?.Cleanup();
                _Main = new MainViewModel();
                return _Main;// ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        private DeptListViewModel _DeptList;
        public DeptListViewModel DeptList
        {
            get
            {
                _DeptList?.Cleanup();
                _DeptList = new DeptListViewModel();
                return _DeptList;// ServiceLocator.Current.GetInstance<DeptListViewModel>();
            }
        }
        DeptInfoViewModel _DeptInfo;
        public DeptInfoViewModel DeptInfo
        {
            get
            {
                _DeptInfo?.Cleanup();
                _DeptInfo = new DeptInfoViewModel();
                return _DeptInfo;// ServiceLocator.Current.GetInstance<DeptInfoViewModel>();
            }
        }
        DutyListViewModel _DutyList;
        public DutyListViewModel DutyList
        {
            get
            {
                _DutyList?.Cleanup();
                _DutyList = new DutyListViewModel();
                return _DutyList;// ServiceLocator.Current.GetInstance<DutyListViewModel>();
            }
        }
        DutyInfoViewModel _DutyInfo;
        public DutyInfoViewModel DutyInfo
        {
            get
            {
                _DutyInfo?.Cleanup();
                _DutyInfo = new DutyInfoViewModel();
                return _DutyInfo;// ServiceLocator.Current.GetInstance<DutyInfoViewModel>();
            }
        }

        ToolStateListViewModel _ToolStateList;
        public ToolStateListViewModel ToolStateList
        {
            get
            {
                _ToolStateList?.Cleanup();
                _ToolStateList = new ToolStateListViewModel();
                return _ToolStateList;// ServiceLocator.Current.GetInstance<ToolStateListViewModel>();
            }
        }

        ToolStateInfoViewModel _ToolStateInfo;
        public ToolStateInfoViewModel ToolStateInfo
        {
            get
            {
                _ToolStateInfo?.Cleanup();
                _ToolStateInfo = new ToolStateInfoViewModel();
                return _ToolStateInfo;// ServiceLocator.Current.GetInstance<ToolStateInfoViewModel>();
            }
        }
        RoleListViewModel _RoleList;
        public RoleListViewModel RoleList
        {
            get
            {
                _RoleList?.Cleanup();
                _RoleList = new RoleListViewModel();
                return _RoleList;// ServiceLocator.Current.GetInstance<RoleListViewModel>();
            }
        }
        RoleInfoViewModel _RoleInfo;
        public RoleInfoViewModel RoleInfo
        {
            get
            {
                _RoleInfo?.Cleanup();
                _RoleInfo = new RoleInfoViewModel();
                return _RoleInfo;// ServiceLocator.Current.GetInstance<RoleInfoViewModel>();
            }
        }
        RightListViewModel _RightList;
        public RightListViewModel RightList
        {
            get
            {
                _RightList?.Cleanup();
                _RightList = new RightListViewModel();
                return _RightList;// ServiceLocator.Current.GetInstance<RightListViewModel>();
            }
        }
        RightInfoViewModel _RightInfo;
        public RightInfoViewModel RightInfo
        {
            get
            {
                _RightInfo?.Cleanup();
                _RightInfo = new RightInfoViewModel();
                return _RightInfo;// ServiceLocator.Current.GetInstance<RightInfoViewModel>();
            }
        }
        UserListViewModel _UserList;
        public UserListViewModel UserList
        {
            get
            {
                _UserList?.Cleanup();
                _UserList = new UserListViewModel();
                return _UserList;// ServiceLocator.Current.GetInstance<UserListViewModel>();
            }
        }
        private UserInfoViewModel _UserInfo;
        public UserInfoViewModel UserInfo
        {
            get
            {
                _UserInfo?.Cleanup();
                _UserInfo = new UserInfoViewModel();
                return _UserInfo;// ServiceLocator.Current.GetInstance<UserInfoViewModel>();
            }
        }
        ToolListViewModel _ToolList;
        public ToolListViewModel ToolList
        {
            get
            {
                _ToolList?.Cleanup();
                _ToolList = new ToolListViewModel();
                return _ToolList;// ServiceLocator.Current.GetInstance<ToolListViewModel>();
            }
        }
        private ToolInfoViewModel _ToolInfo;
        public ToolInfoViewModel ToolInfo
        {
            get
            {
                _ToolInfo?.Cleanup();
                _ToolInfo = new ToolInfoViewModel();//ServiceLocator.Current.GetInstance<ToolInfoViewModel>();
                return _ToolInfo;
            }
        }
        ToolCategoryListViewModel _ToolCategoryList;
        public ToolCategoryListViewModel ToolCategoryList
        {
            get
            {
                _ToolCategoryList?.Cleanup();
                _ToolCategoryList = new ToolCategoryListViewModel();//ServiceLocator.Current.GetInstance<ToolCategoryListViewModel>();
                return _ToolCategoryList;
            }
        }
        ToolCategoryInfoViewModel _ToolCategoryInfo;
        public ToolCategoryInfoViewModel ToolCategoryInfo
        {
            get
            {
                _ToolCategoryInfo?.Cleanup();
                _ToolCategoryInfo = new ToolCategoryInfoViewModel();//ServiceLocator.Current.GetInstance<ToolCategoryInfoViewModel>();
                return _ToolCategoryInfo;
            }
        }

        private ToolBorrowViewModel _ToolBorrow;
        public ToolBorrowViewModel ToolBorrow
        {
            get
            {
                _ToolBorrow?.Cleanup();
                _ToolBorrow = new ToolBorrowViewModel();//ServiceLocator.Current.GetInstance<ToolBorrowViewModel>();
                return _ToolBorrow;
            }
        }
        ToolRecordListViewModel _ToolRecordList;
        public ToolRecordListViewModel ToolRecordList
        {
            get
            {
                _ToolRecordList?.Cleanup();
                _ToolRecordList = new ToolRecordListViewModel();//ServiceLocator.Current.GetInstance<ToolRecordListViewModel>();
                return _ToolRecordList;
            }
        }
        SelectUserViewModel _SelectUser;
        public SelectUserViewModel SelectUser
        {
            get
            {
                _SelectUser?.Cleanup();
                _SelectUser = new SelectUserViewModel();//ServiceLocator.Current.GetInstance<SelectUserViewModel>();
                return _SelectUser;
            }
        }
        SelectToolViewModel _SelectTool;
        public SelectToolViewModel SelectTool
        {
            get
            {
                _SelectTool?.Cleanup();
                _SelectTool = new SelectToolViewModel();//ServiceLocator.Current.GetInstance<SelectToolViewModel>();
                return _SelectTool;
            }
        }
        private ToolReturnViewModel _ToolReturn;
        public ToolReturnViewModel ToolReturn
        {
            get
            {
                _ToolReturn?.Cleanup();
                _ToolReturn = new ToolReturnViewModel();//ServiceLocator.Current.GetInstance<ToolReturnViewModel>();
                return _ToolReturn;
            }
        }
        ToolDamageListViewModel _ToolDamageList;
        public ToolDamageListViewModel ToolDamageList
        {
            get
            {
                _ToolDamageList?.Cleanup();
                _ToolDamageList = new ToolDamageListViewModel();//ServiceLocator.Current.GetInstance<ToolDamageListViewModel>();
                return _ToolDamageList;
            }
        }
        ToolDamageInfoViewModel _ToolDamageInfo;
        public ToolDamageInfoViewModel ToolDamageInfo
        {
            get
            {
                _ToolDamageInfo?.Cleanup();
                _ToolDamageInfo = new ToolDamageInfoViewModel();//ServiceLocator.Current.GetInstance<ToolDamageInfoViewModel>();
                return _ToolDamageInfo;
            }
        }
        ToolRepairListViewModel _ToolRepairList;
        public ToolRepairListViewModel ToolRepairList
        {
            get
            {
                _ToolRepairList?.Cleanup();
                _ToolRepairList = new ToolRepairListViewModel();//ServiceLocator.Current.GetInstance<ToolRepairListViewModel>();
                return _ToolRepairList;
            }
        }
        ToolRepairInfoViewModel _ToolRepairInfo;
        public ToolRepairInfoViewModel ToolRepairInfo
        {
            get
            {
                _ToolRepairInfo?.Cleanup();
                _ToolRepairInfo = new ToolRepairInfoViewModel();//ServiceLocator.Current.GetInstance<ToolRepairInfoViewModel>();
                return _ToolRepairInfo;
            }
        }

        RightSetViewModel _RightSet;
        public RightSetViewModel RightSet
        {
            get
            {
                _RightSet?.Cleanup();
                _RightSet = new RightSetViewModel();
                return _RightSet;
            }
        }

        SystemSetViewModel _SystemSet;
        public SystemSetViewModel SystemSet
        {
            get
            {
                _SystemSet?.Cleanup();
                _SystemSet = new SystemSetViewModel();
                return _SystemSet;
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}