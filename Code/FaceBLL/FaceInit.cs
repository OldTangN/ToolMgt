using System;
using System.Runtime.InteropServices;
using ToolMgt.Common;

namespace FaceBLL
{
    public class FaceInit
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FaceCallback(IntPtr bytes, int size, String res);

        /// <summary>
        /// sdk初始化
        /// </summary>
        /// <param name="id_card">传入true为使用证件照模型，传入fasle为普通生活照模型</param>
        /// <returns></returns>
        [DllImport("BaiduFaceApi.dll", EntryPoint = "sdk_init", CharSet = CharSet.Ansi
             , CallingConvention = CallingConvention.Cdecl)]
        public static extern int sdk_init(bool id_card);
        // 是否授权
        [DllImport("BaiduFaceApi.dll", EntryPoint = "is_auth", CharSet = CharSet.Ansi
                , CallingConvention = CallingConvention.Cdecl)]
        public static extern bool is_auth();

        // 获取设备指纹
        [DllImport("BaiduFaceApi.dll", EntryPoint = "get_device_id", CharSet = CharSet.Ansi
                 , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr get_device_id();
        // sdk销毁
        [DllImport("BaiduFaceApi.dll", EntryPoint = "sdk_destroy", CharSet = CharSet.Ansi
             , CallingConvention = CallingConvention.Cdecl)]
        public static extern void sdk_destroy();

        /// <summary>
        /// 人脸注册(传入图片文件路径)
        /// </summary>
        /// <param name="user_id">用户id，字母、数字、下划线组成，最多128个字符</param>
        /// <param name="group_id">用户组id，标识一组用户（由数字、字母、下划线组成），长度限制128B。用户组和user_id之间，仅为映射关系。如传入的groupid并未事先创建完毕，则注册用户的同时，直接完成group的创建</param>
        /// <param name="file_name">图片信息，须小于10M，传入图片的本地文件地址</param>
        /// <param name="user_info">用户资料，256个字符以内</param>
        /// <returns></returns>
        [DllImport("BaiduFaceApi.dll", EntryPoint = "user_add", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr user_add(string user_id, string group_id, string file_name,
            string user_info = "");

        /// <summary>
        /// 人脸注册(传入图片二进制buffer)
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="group_id"></param>
        /// <param name="image">图片字节数组</param>
        /// <param name="size">数组长度</param>
        /// <param name="user_info"></param>
        /// <returns></returns>
        [DllImport("BaiduFaceApi.dll", EntryPoint = "user_add_by_buf", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr user_add_by_buf(string user_id, string group_id, byte[] image,
           int size, string user_info = "");

        /// <summary>
        ///  人脸更新(传入图片文件路径)
        /// </summary>
        /// <param name="user_id">用户id（由数字、字母、下划线组成），长度限制128B</param>
        /// <param name="group_id">用户组id，标识一组用户（由数字、字母、下划线组成），长度限制128B</param>
        /// <param name="file_name">图片信息，数据大小应小于10M，传入本地图片文件地址，每次仅支持单张图片</param>
        /// <param name="user_info"></param>
        /// <returns></returns>
        [DllImport("BaiduFaceApi.dll", EntryPoint = "user_update", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr user_update(string user_id, string group_id, string file_name,
            string user_info = "");

        /// <summary>
        /// 人脸更新(传入图片二进制buffer)
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="group_id"></param>
        /// <param name="image">图片字节数组</param>
        /// <param name="size">数组长度</param>
        /// <param name="user_info"></param>
        /// <returns></returns>
        [DllImport("BaiduFaceApi.dll", EntryPoint = "user_update_by_buf", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr user_update_by_buf(string user_id, string group_id, byte[] image,
           int size, string user_info = "");

        /// <summary>
        /// 人脸删除
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="group_id"></param>
        /// <param name="face_token"></param>
        /// <returns></returns>
        [DllImport("BaiduFaceApi.dll", EntryPoint = "user_face_delete", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr user_face_delete(string user_id, string group_id, string face_token);
        /// <summary>
        /// 用户删除
        /// </summary>
        /// <param name="user_id">用户id（由数字、字母、下划线组成），长度限制128B</param>
        /// <param name="group_id">用户组id，标识一组用户（由数字、字母、下划线组成），长度限制128B</param>
        /// <returns></returns>
        [DllImport("BaiduFaceApi.dll", EntryPoint = "user_delete", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr user_delete(string user_id, string group_id);

        // 单目RGB静默活体检测（传入图片文件路径)
        [DllImport("BaiduFaceApi.dll", EntryPoint = "rgb_liveness_check", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr rgb_liveness_check(string file_name);

        /// <summary>
        /// 1:N人脸识别（传图片文件路径和库里的比对）
        /// </summary>
        /// <param name="image">图片信息，数据大小小于10M，传入图片文件路径</param>
        /// <param name="group_id_list">组id列表。默认至少填写一个group_id，从指定的group中进行查找。需要同时查询多个group，用逗号分隔，上限10个</param>
        /// <param name="user_id">用户id，若指定了某个user，则只会与指定group下的这个user进行对比；若user_id传空字符串” ”，则会与此group下的所有user进行1：N识别</param>
        /// <param name="user_top_num">识别后返回的用户top数，默认为1，最多返回50个结果</param>
        /// <returns></returns>
        [DllImport("BaiduFaceApi.dll", EntryPoint = "identify", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr identify(string image, string group_id_list, string user_id, int user_top_num = 1);

        /// <summary>
        /// 1:N人脸识别（传图片二进制文件buffer和库里的比对）
        /// </summary>
        /// <param name="buf">图片数组</param>
        /// <param name="size">数组长度</param>
        /// <param name="group_id_list"></param>
        /// <param name="user_id"></param>
        /// <param name="user_top_num"></param>
        /// <returns></returns>
        [DllImport("BaiduFaceApi.dll", EntryPoint = "identify_by_buf", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr identify_by_buf(byte[] buf, int size, string group_id_list,
            string user_id, int user_top_num = 1);
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="group_id"></param>
        /// <returns></returns>
        [DllImport("BaiduFaceApi.dll", EntryPoint = "get_user_info", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr get_user_info(string user_id, string group_id);
        // 测试获取设备指纹device_id
        public static void test_get_device_id()
        {
            IntPtr ptr = get_device_id();
            string buf = Marshal.PtrToStringAnsi(ptr);
            LogUtil.WriteLog("device id is:" + buf);
        }
        /// <summary>
        /// 人脸注册
        /// </summary>
        /// <param name="user_id">用户id，字母、数字、下划线组成，最多128个字符</param>
        /// <param name="file_name">图片信息，须小于10M，传入图片的本地文件地址</param>
        /// <param name="group_id">用户组id，标识一组用户（由数字、字母、下划线组成），长度限制128B。用户组和user_id之间，仅为映射关系。如传入的groupid并未事先创建完毕，则注册用户的同时，直接完成group的创建</param>
        public static string test_user_add(string user_id, string file_name, string group_id = "qddeer")
        {
            IntPtr ptr = user_add(user_id, group_id, file_name);
            string buf = Marshal.PtrToStringAnsi(ptr);
            LogUtil.WriteLog("user_add res is:" + buf);

            return buf;
            //return JsonConvert.DeserializeObject<FaceResut>(buf);
        }

        /// <summary>
        /// 人脸更新
        /// </summary>
        /// <param name="user_id">用户id，字母、数字、下划线组成，最多128个字符</param>
        /// <param name="file_name">图片信息，须小于10M，传入图片的本地文件地址</param>
        /// <param name="group_id">用户组id，标识一组用户（由数字、字母、下划线组成），长度限制128B</param>
        public static string test_user_update(string user_id, string file_name, string group_id = "qddeer")
        {
            IntPtr ptr = user_update(user_id, group_id, file_name);
            string buf = Marshal.PtrToStringAnsi(ptr);
            LogUtil.WriteLog("user_update res is:" + buf);
            return buf;
            //return JsonConvert.DeserializeObject<FaceResut>(buf);
        }

        /// <summary>
        /// 用户删除
        /// </summary>
        /// <param name="user_id">用户id，字母、数字、下划线组成，最多128个字符</param>
        /// <param name="group_id">用户组id，标识一组用户（由数字、字母、下划线组成），长度限制128B<</param>
        public static string test_user_delete(string user_id, string group_id = "qddeer")
        {
            IntPtr ptr = user_delete(user_id, group_id);
            string buf = Marshal.PtrToStringAnsi(ptr);
            LogUtil.WriteLog("user_delete res is:" + buf);

            return buf;
            // return JsonConvert.DeserializeObject<FaceResut>(buf);
        }

        /// <summary>
        /// 人脸删除
        /// </summary>
        /// <param name="user_id">用户id，字母、数字、下划线组成，最多128个字符</param>
        /// <param name="group_id">用户组id，标识一组用户（由数字、字母、下划线组成），长度限制128B<</param>
        public static string test_user_face_delete(string user_id, string face_token, string group_id = "qddeer")
        {
            IntPtr ptr = user_face_delete(user_id, group_id, face_token);
            string buf = Marshal.PtrToStringAnsi(ptr);
            LogUtil.WriteLog("user_face_delete res is:" + buf);

            return buf;
            // return JsonConvert.DeserializeObject<FaceResut>(buf);
        }

        /// <summary>
        /// 测试单目RGB静默活体检测（传入图片文件路径)
        /// </summary>
        /// <param name="file_name">传入图片文件路径</param>
        public static string test_rgb_liveness_check(string file_name)
        {
            // 传入图片文件绝对路径
            IntPtr ptr = rgb_liveness_check(file_name);
            string buf = Marshal.PtrToStringAnsi(ptr);
            LogUtil.WriteLog("rgb_liveness_check res is:" + buf);

            return buf;
            //return JsonConvert.DeserializeObject<FaceResut>(buf);
        }
        // 测试1:N比较，传入图片文件路径
        public static string test_identify(string file_name, string group_id = "qddeer")
        {
            string user_id = "";
            IntPtr ptr = identify(file_name, group_id, user_id);
            string buf = Marshal.PtrToStringAnsi(ptr);
            LogUtil.WriteLog("identify res is:" + buf);

            return buf;
            //return JsonConvert.DeserializeObject<FaceResut>(buf);
        }

        public static string test_get_user_info(int user_id, string group_id = "qddeer")
        {
            IntPtr ptr = get_user_info(user_id.ToString(), group_id);
            string buf = Marshal.PtrToStringAnsi(ptr);
            LogUtil.WriteLog("get_user_info res is:" + buf);
            return buf;
        }
    }
}
