using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaceBLL;
using Newtonsoft.Json;
using ToolMgt.Common;
using ToolMgt.Model;

namespace ToolMgt.BLL
{
    public class UserDao
    {
        private ToolMgtEntities Db;

        public UserDao()
        {
            Db = ContextFactory.GetContext();
        }

        public Result<User> LogIn(LogInModel model)
        {
            Result<User> rlt = new Result<User>();
            try
            {
                User u = Db.Users.FirstOrDefault(p => p.Code == model.CodeOrCard || p.CardNo == model.CodeOrCard);
                if (u == null)
                {
                    rlt.HasError = true;
                    rlt.Msg = "用户名或卡号不存在！";
                }
                else
                {
                    if (!model.IsCard && !string.Equals(model.Pwd, u.Pwd))//非卡号登录时验证密码
                    {
                        rlt.HasError = true;
                        rlt.Msg = "用户密码错误！";
                    }
                    else
                    {
                        //查询duty department
                        GetExtPropertyValue(u);
                        rlt.Entities = u;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }

        private void GetExtPropertyValue(User u)
        {
            u.DeptName = Db.Departments.FirstOrDefault(p => p.Id == u.DeptId)?.Name;
            u.DutyName = Db.Duties.FirstOrDefault(p => p.Id == u.DutyId)?.Name;
            u.RoleName = Db.Roles.FirstOrDefault(p => p.Id == u.RoleId)?.Name;
        }

        public Result<List<Right>> GetUserRights(int uid)
        {
            Result<List<Right>> rlt = new Result<List<Right>>();
            try
            {
                var data = from u in Db.Users
                           join ro in Db.Roles on u.RoleId equals ro.Id
                           join rr in Db.RoleRights on ro.Id equals rr.RoleId
                           join r in Db.Rights on rr.RightId equals r.Id
                           where u.Id == uid
                           select r;
                rlt.Entities = data.ToList();//.Distinct(new FastPropertyComparer<Right>("Id"))去重复
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex); rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }

        public Result<User> GetUser(int id)
        {
            Result<User> rlt = new Result<User>();
            try
            {
                User u = Db.Users.First(p => p.Id == id);
                GetExtPropertyValue(u);
                rlt.Entities = u;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }

        public Result<User> GetUser(string cardNo)
        {
            Result<User> rlt = new Result<User>();
            try
            {
                User u = Db.Users.First(p => p.CardNo == cardNo);
                GetExtPropertyValue(u);
                rlt.Entities = u;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }

        /// <summary>
        /// 判断用户信息重复
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="code"></param>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        private Result<bool> IsRepeat(User u)
        {
            Result<bool> rlt = new Result<bool>();
            rlt.Msg = "";
            rlt.Entities = false;
            try
            {
                //无AsNoTracking() 在Entry时，会报实体重复
                var users = Db.Users.AsNoTracking().Where(p => p.Code == u.Code);
                if (users.Count() > 0)
                {
                    foreach (var item in users)
                    {
                        if (item.Id != u.Id)
                        {
                            rlt.Msg += "用户账号重复！\r\n";
                            rlt.Entities = true;
                            break;
                        }
                    }
                }
                //无AsNoTracking() 在Entry时，会报实体重复
                users = Db.Users.AsNoTracking().Where(p => p.CardNo == u.CardNo);
                if (users.Count() > 0)
                {
                    foreach (var item in users)
                    {
                        if (item.Id != u.Id)
                        {
                            rlt.Entities = true;
                            rlt.Msg += "用户卡号重复！\r\n";
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }

        public Result<List<User>> GetUsers(string name, string cardno, int deptid)
        {
            Result<List<User>> rlt = new Result<List<User>>();
            try
            {
                rlt.Entities = Db.Users
                    .Where(p => p.Name.Contains(name) && p.CardNo.Contains(cardno) && (p.DeptId == deptid || deptid == 0))
                    .ToList();
                if (rlt.Entities != null && rlt.Entities.Count > 0)
                {
                    foreach (var u in rlt.Entities)
                    {
                        GetExtPropertyValue(u);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }

        public Result<bool> AddUser(User user)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var rptrlt = IsRepeat(user);
                if (rptrlt.Entities)
                {
                    rlt.Entities = false;
                    rlt.Msg = rptrlt.Msg;
                }
                else
                {
                    Db.Users.Add(user);
                    Db.SaveChanges();
                    if (!string.IsNullOrEmpty(user.ImagePath))
                    {
                        try
                        {
                            if (System.IO.File.Exists(user.ImagePath))
                            {
                                string imgPath = System.IO.Path.Combine(ConfigurationUtil.GetConfiguration("ImageDir"), Guid.NewGuid() + ".jpg");
                                System.IO.File.Copy(user.ImagePath, imgPath);
                                string resut = FaceInit.test_user_add(user.Id.ToString(), imgPath);
                                var faceRlt = JsonConvert.DeserializeObject<FaceResut>(resut);
                                if (faceRlt.errno == 0)//成功
                                {
                                    user.ImagePath = imgPath;
                                    user.FaceToken = faceRlt.data.face_token;
                                }
                                else//失败
                                {
                                    user.ImagePath = "";
                                    rlt.HasError = true;
                                    rlt.Msg = "人脸识别信息注册失败，请使用【用户编辑】功能重新录入人脸信息！\r\n" + FaceErrors.GetFaceError(faceRlt.errno);
                                }
                                Db.SaveChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogUtil.WriteLog(ex);
                            rlt.HasError = true;
                            rlt.Msg = "人脸识别信息注册失败，请使用【用户编辑】功能重新录入人脸信息！\r\n" + ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }
        public Result<bool> EditUser(User u)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var rptrlt = IsRepeat(u);
                if (rptrlt.Entities)
                {
                    rlt.Entities = false;
                    rlt.Msg = rptrlt.Msg;
                }
                else
                { //判断图片发生变化
                    bool imgChange = false;
                    var old = Db.Users.AsNoTracking().First(p => p.Id == u.Id);
                    if (old.ImagePath != u.ImagePath)
                    {
                        imgChange = true;
                    }
                    var entity = Db.Entry(u);

                    entity.State = System.Data.Entity.EntityState.Modified;
                    Db.SaveChanges();
                    if (!string.IsNullOrEmpty(u.ImagePath) && imgChange)
                    {
                        try
                        {
                            if (System.IO.File.Exists(u.ImagePath))
                            {
                                string imgPath = System.IO.Path.Combine(ConfigurationUtil.GetConfiguration("ImageDir"), Guid.NewGuid() + ".jpg");
                                System.IO.File.Copy(u.ImagePath, imgPath);

                                //查询用户是否存在
                                string result = FaceInit.test_get_user_info(u.Id);
                                var faceRlt = JsonConvert.DeserializeObject<FaceResut>(result);
                                if (faceRlt.errno == 0)
                                {
                                    if (faceRlt.data.result == null || faceRlt.data.result.Count == 0)
                                    {
                                        result = FaceInit.test_user_add(u.Id.ToString(), imgPath);
                                    }
                                    else
                                    {
                                        result = FaceInit.test_user_update(u.Id.ToString(), imgPath);
                                    }
                                    faceRlt = JsonConvert.DeserializeObject<FaceResut>(result);
                                    if (faceRlt.errno == 0)//成功
                                    {
                                        try
                                        {
                                            System.IO.File.Delete(old.ImagePath);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                        u.ImagePath = imgPath;
                                        u.FaceToken = faceRlt.data.face_token;
                                    }
                                    else//失败
                                    {
                                        u.ImagePath = "";
                                        rlt.HasError = true;
                                        rlt.Msg = "人脸识别信息更新失败，请使用【用户编辑】功能重新录入人脸信息！\r\n" + FaceErrors.GetFaceError(faceRlt.errno);
                                    }
                                }
                                else
                                {
                                    u.ImagePath = "";
                                    rlt.HasError = true;
                                    rlt.Msg = "人脸识别信息更新失败，请使用【用户编辑】功能重新录入人脸信息！\r\n" + FaceErrors.GetFaceError(faceRlt.errno);
                                }

                                Db.SaveChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogUtil.WriteLog(ex);
                            rlt.HasError = true;
                            rlt.Msg = "人脸识别信息更新失败，请使用【用户编辑】功能重新录入人脸信息！\r\n" + ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }

        public Result<bool> ChangePwd(int uid, string oldpwd, string newpwd)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var user = Db.Users.FirstOrDefault(p => p.Id == uid && p.Pwd == oldpwd);
                if (user == null)
                {
                    rlt.HasError = true;
                    rlt.Msg = "原密码错误！";
                }
                else
                {
                    user.Pwd = newpwd;
                    Db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }

        public Result<bool> DeleteUser(int id)
        {
            Result<bool> rlt = new Result<bool>();
            try
            {
                var old = Db.Users.First(p => p.Id == id);
                Db.Users.Remove(old);
                Db.SaveChanges();
                try
                {
                    System.IO.File.Delete(old.ImagePath);
                }
                catch (Exception ex)
                {
                }
                try
                {
                    FaceInit.test_user_face_delete(old.Id.ToString(), old.FaceToken);
                }
                catch (Exception ex)
                {
                }
                try
                {
                    FaceInit.test_user_delete(old.Id.ToString());
                }
                catch (Exception ex)
                {
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }

        public Result<User> LoginByFace(string path)
        {
            Result<User> rlt = new Result<User>();
            try
            {
                string result = FaceInit.test_identify(path);
                FaceResut faceResut = JsonConvert.DeserializeObject<FaceResut>(result);
                if (faceResut.errno == 0)
                {
                    if (faceResut.data.result_num > 0)
                    {
                        int user_id = Convert.ToInt32(faceResut.data.result[0].user_id);
                        decimal score = Convert.ToDecimal(faceResut.data.result[0].score);
                        if (score > 70)
                        {
                            var user = Db.Users.FirstOrDefault(p => p.Id == user_id);
                            if (user == null)
                            {
                                rlt.HasError = true;
                                rlt.Msg = "用户不存在！";
                            }
                            else
                            {
                                rlt.Entities = user;
                            }
                        }
                    }
                    else
                    {
                        rlt.HasError = true;
                        rlt.Msg = "用户不存在！";
                    }
                }
                else
                {
                    rlt.HasError = true;
                    rlt.Msg = "识别失败！" + FaceErrors.GetFaceError(faceResut.errno);
                }
            }
            catch (Exception ex)
            {
                rlt.HasError = true;
                rlt.Msg = ex.Message;
            }
            return rlt;
        }
    }
}
