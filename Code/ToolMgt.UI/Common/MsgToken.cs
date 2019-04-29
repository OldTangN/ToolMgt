using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolMgt.UI.Common
{
    public class MsgToken
    {
        /// <summary>
        /// 登录事件
        /// </summary>
        public static string Login = nameof(Login);

        /// <summary>
        /// 登陆后事件
        /// </summary>
        public static string Logined = nameof(Logined);

        #region 传递数据，从List到Info
        /// <summary>
        /// 部门数据传递
        /// </summary>
        public static string TransforDept = nameof(TransforDept);

        /// <summary>
        /// 职务数据传递
        /// </summary>
        public static string TransforDuty = nameof(TransforDuty);

        /// <summary>
        /// 工具状态数据传递
        /// </summary>
        public static string TransforToolState = nameof(TransforToolState);

        /// <summary>
        /// 权限数据传递
        /// </summary>
        public static string TransforRight = nameof(TransforRight);

        /// <summary>
        /// 角色数据传递
        /// </summary>
        public static string TransforRole = nameof(TransforRole);

        /// <summary>
        /// 用户数据传递
        /// </summary>
        public static string TransforUser = nameof(TransforUser);

        public static string TransforTool = nameof(TransforTool);

        public static string TransforToolCategory = nameof(TransforToolCategory);

        public static string TransforToolDamage = nameof(TransforToolDamage);

        public static string TransforToolRepair = nameof(TransforToolRepair);

        public static string TransforImgPath = nameof(TransforImgPath);
        #endregion

        public static string TakePic = nameof(TakePic);
        public static string RefreshCamera = nameof(RefreshCamera);
        public static string ShowImage = nameof(ShowImage);

        #region 打开、关闭窗口
        public static string CloseDeptInfo = nameof(CloseDeptInfo);
        public static string CloseDutyInfo = nameof(CloseDutyInfo);
        public static string CloseToolStateInfo = nameof(CloseToolStateInfo);
        public static string CloseRightInfo = nameof(CloseRightInfo);
        public static string CloseRoleInfo = nameof(CloseRoleInfo);
        public static string CloseUserInfo = nameof(CloseUserInfo);
        public static string CloseUserList = nameof(CloseUserList);
        public static string CloseToolInfo = nameof(CloseToolInfo);
        public static string CloseToolList = nameof(CloseToolList);
        public static string CloseToolCategoryInfo = nameof(CloseToolCategoryInfo);
        public static string CloseToolBorrow = nameof(CloseToolBorrow);
        public static string CloseToolReturn = nameof(CloseToolReturn);
        public static string CloseSelectUser = nameof(CloseSelectUser);
        public static string CloseSelectTool = nameof(CloseSelectTool);
        public static string CloseToolDamageInfo = nameof(CloseToolDamageInfo);
        public static string CloseToolRepairInfo = nameof(CloseToolRepairInfo);
        public static string CloseRightSet = nameof(CloseRightSet);
        public static string CloseLoginStatus = nameof(CloseLoginStatus);
        public static string CloseSystemSet = nameof(CloseSystemSet);

        public static string OpenToolInfo = nameof(OpenToolInfo);
        public static string OpenDeptInfo = nameof(OpenDeptInfo);
        public static string OpenDutyInfo = nameof(OpenDutyInfo);
        public static string OpenToolStateInfo = nameof(OpenToolStateInfo);
        public static string OpenRightInfo = nameof(OpenRightInfo);
        public static string OpenRoleInfo = nameof(OpenRoleInfo);
        public static string OpenUserInfo = nameof(OpenUserInfo);
        public static string OpenToolCategoryInfo = nameof(OpenToolCategoryInfo);
        public static string OpenToolBorrow = nameof(OpenToolBorrow);
        public static string OpenToolReturn = nameof(OpenToolReturn);
        public static string OpenSelectUser = nameof(OpenSelectUser);
        public static string OpenSelectTool = nameof(OpenSelectTool);
        public static string OpenToolDamageInfo = nameof(OpenToolDamageInfo);
        public static string OpenToolRepairInfo = nameof(OpenToolRepairInfo);
        #endregion

        #region 刷新列表
        public static string RefreshDeptList = nameof(RefreshDeptList);
        public static string RefreshDutyList = nameof(RefreshDutyList);
        public static string RefreshToolStateList = nameof(RefreshToolStateList);
        public static string RefreshUserList = nameof(RefreshUserList);
        public static string RefreshRoleList = nameof(RefreshRoleList);
        public static string RefreshRightList = nameof(RefreshRightList);
        public static string RefreshToolList = nameof(RefreshToolList);
        public static string RefreshToolCategoryList = nameof(RefreshToolCategoryList);
        public static string RefreshToolRecordList = nameof(RefreshToolRecordList);
        public static string RefreshToolDamageList = nameof(RefreshToolDamageList);
        public static string RefreshToolRepairList = nameof(RefreshToolRepairList);
        #endregion
    }
}

