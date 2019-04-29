using System;

namespace FaceBLL
{
    public enum FaceError
    {
        成功 = 0,
        系统错误 = 1,
        未知错误 = 2,
        授权校验失败 = 1001,
        请求参数错误 = 1002,
        数据库操作失败 = 1003,
        没有数据 = 1004,
        记录不存在 = 1005,
        记录已经存在 = 1006,
        文件不存在 = 1007,
        提取特征值失败 = 1008,
        文件太大 = 1009,
        人脸资源文件不存在 = 1010,
        特征值长度错误 = 1011,
        未检测到人脸 = 1012,
        摄像头错误或不存在 = 1013
    }
    public class FaceErrors
    {
        public static FaceError GetFaceError(int errorno)
        {
            FaceError error = FaceError.未知错误;
            Enum.TryParse<FaceError>(errorno.ToString(), out error);
            return error;
        }
    }
}
