using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.FkLibSyncModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Interfaces
{
    /// <summary>
    /// 费卡文库对接服务
    /// </summary>
    public interface IFKLibService
    {
        /// <summary>
        /// 签到接口
        /// </summary>
        /// <param name="wxUser">医生信息</param>
        /// <param name="fkMeetingId">费卡文库的会议Id</param>
        /// <returns></returns>
        ReturnValueModel SyncCheckIn(WxUserModel wxUser, string fkMeetingId);

        /// <summary>
        /// 签到接口
        /// 2、多福医生
        /// 由费卡文库科室会进入小程序后，判定医生是否注册
        /// （1）已注册，调用费卡文库签到接口，进行状态变更，传递参数：活动ID，医生昵称，医生姓名，小程序UnionID，OneHCP医生唯一ID，OneHCP验证结果，云势ID
        /// （2）未注册，医生进行注册，注册成功后，调用费卡文库签到接口，进行状态变更，传递参数：活动ID，医生昵称，医生姓名，小程序UnionID，OneHCP医生唯一ID，OneHCP验证结果，云势ID      
        /// </summary>
        ReturnValueModel SyncCheckIn(CheckInSyncModel checkInSyncModel);

        /// <summary>
        ///3、科室会同步
        ///      两种方式：
        ///     （1）费卡文库创建科室会，同步在小程序生成；缺点：一旦系统维护，不能及时同步
        ///     （2）定期同步；缺点：非实时；每天凌晨科室会同步
        ///
        ///      注：将采用定期同步：每天凌晨同步已完成的科室会到OneHCP数据库
        ///      同步内容：
        ///      科室会编号、科室会标题，活动状态，创建时间，召开时间，医院，科室，内容，参与人数
        /// </summary>
        ReturnValueModel SyncMeetingInfo();

        /// <summary>
        /// 4、人员信息同步
        ///    将采用定期同步：每月凌晨同步更新，根据OneHCP唯一ID更新签到人员认证信息
        ///    同步字段：OneHCP医生验证状态、理由、云势ID
        /// </summary>
        ReturnValueModel SyncPersonInfo();
    }
}
