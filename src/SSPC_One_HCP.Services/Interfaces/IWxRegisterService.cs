using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SSPC_One_HCP.Services.Interfaces
{
    public interface IWxRegisterService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wxUserModel"></param>
        /// <returns></returns>
        ReturnValueModel AddDocUser(WxUserModel wxUserModel);

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="workUser"></param>
        /// <param name="mobile">手机号</param>
        /// <returns></returns>
        ReturnValueModel SendVerifyCode(WorkUser workUser, string mobile);

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="workUser"></param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        ReturnValueModel VerifyCode(WorkUser workUser, string code);

        /// <summary>
        /// 验证手机号
        /// </summary>
        /// <param name="moblie"></param>
        /// <returns></returns>
        ReturnValueModel CheckMoblie(string moblie);

        ///// <summary>
        ///// 更新微信用户信息
        ///// </summary>
        ///// <param name="wxUserModel"></param>
        ///// <returns></returns>
        //ReturnValueModel UpdateWxUser(WxUserModel wxUserModel);

        /// <summary>
        /// 更新医生医院及科室
        /// </summary>
        /// <param name="workUser"></param>
        /// <param name="wxUserModel">
        /// 微信用户信息：
        /// HospitalName，
        /// DepartmentName
        /// </param>
        /// <param name="workUser"></param>
        /// <param name="isRegistering">是否想注册用户</param>
        /// <returns></returns>
        ReturnValueModel UpdateDocHospitalOrDept(WorkUser workUser, WxUserModel wxUserModel, bool isRegistering);

        /// <summary>
        /// 上传医生个人信息证明图片
        /// </summary>
        /// <param name="workUser">当前操作用户</param>
        /// <param name="httpRequest">请求</param>
        /// <returns></returns>
        ReturnValueModel UploadPicture(WorkUser workUser, HttpRequest httpRequest);

        /// <summary>
        /// 上传签名
        /// </summary>
        /// <param name="workUser">当前操作用户</param>
        /// <param name="httpRequest">请求</param>
        /// <returns></returns>
        ReturnValueModel UploadSign(WorkUser workUser, HttpRequest httpRequest);

        /// <summary>
        /// 缓存微信用户信息
        /// </summary>
        /// <returns></returns>
        void CacheWxUser(WxUserModel wxUser);

        /// <summary>
        /// 获取缓存中的微信用户信息
        /// </summary>
        /// <returns></returns>
        WorkUser GetWorkUser(string unionId);
        /// <summary>
        /// 判断别名是否合格
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        ReturnValueModel IsQualified(string UserName);


        /// <summary>
        /// 缓存销售微信用户信息
        /// </summary>
        /// <returns></returns>
        void CacheWxSaleUser(WxSaleUserModel wxSaleUser);

        /// <summary>
        /// 获取缓存中的销售微信用户信息
        /// </summary>
        /// <returns></returns>
        WorkUser GetWxSaleUser(string id);

        void RemoveWxSaleUser(WxSaleUserModel wxSaleUser);

    }
}
