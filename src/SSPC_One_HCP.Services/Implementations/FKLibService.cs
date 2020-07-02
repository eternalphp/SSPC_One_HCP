using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.FkLibSyncModels;
using System.Configuration;
using SSPC_One_HCP.Core.Utils;
using Newtonsoft.Json;

namespace SSPC_One_HCP.Services.Implementations
{
    /// <summary>
    /// 费卡文库对接服务
    /// </summary>
    public class FKLibService : IFKLibService
    {
        private readonly IEfRepository _rep;

        private readonly string _host = ConfigurationManager.AppSettings["FkLibHostUrl"];
        private readonly string _appId = ConfigurationManager.AppSettings["FkLibAppId"];

        public FKLibService(IEfRepository rep)
        {
            _rep = rep;
        }

        /// <summary>
        /// 签到接口
        /// </summary>
        /// <param name="wxUser">医生信息</param>
        /// <param name="fkMeetingId">费卡文库的会议Id</param>
        /// <returns></returns>
        public ReturnValueModel SyncCheckIn(WxUserModel wxUser, string fkMeetingId)
        {
            CheckInSyncModel model = new CheckInSyncModel();
            model.ActivityID = fkMeetingId;
            model.OpenId = wxUser.OpenId;
            model.UnionId = wxUser.UnionId;
            model.OpenName = wxUser.WxName;
            model.Name = wxUser.UserName;
            model.OneHCPID = wxUser.Id;
            model.OneHCPState = wxUser.status;
            model.OneHCPReason = wxUser.reason;
            model.YSID = wxUser.yunshi_doctor_id;

            return SyncCheckIn(model);
        }

        /// <summary>
        /// 签到接口
        /// 2、多福医生
        /// </summary>
        public ReturnValueModel SyncCheckIn(CheckInSyncModel checkInSyncModel)
        {
            // 由费卡文库科室会进入小程序后，判定医生是否注册
            // （1）已注册，调用费卡文库签到接口，进行状态变更，传递参数：活动ID，医生昵称，医生姓名，小程序UnionID，OneHCP医生唯一ID，OneHCP验证结果，云势ID
            // （2）未注册，医生进行注册，注册成功后，调用费卡文库签到接口，进行状态变更，传递参数：活动ID，医生昵称，医生姓名，小程序UnionID，OneHCP医生唯一ID，OneHCP验证结果，云势ID      

            ReturnValueModel rvm = new ReturnValueModel();
            var postStr = $"ActivityID={checkInSyncModel.ActivityID}";
            postStr += $"&openName={checkInSyncModel.OpenName}";
            postStr += $"&name={checkInSyncModel.Name}";
            postStr += $"&openId={checkInSyncModel.OpenId}";
            postStr += $"&UnionId={checkInSyncModel.UnionId}";
            postStr += $"&OneHCPID={checkInSyncModel.OneHCPID}";
            postStr += $"&OneHCPState={checkInSyncModel.OneHCPState}";
            postStr += $"&OneHCPReason={checkInSyncModel.OneHCPReason}";
            postStr += $"&YSID={checkInSyncModel.YSID}";
            var checkInPath = $@"{_host}/OneHCPService/CheckIn.ashx";
            var returnModel = HttpUtils.PostResponse<ReturnValueSyncModel>(checkInPath, postStr, "application/x-www-form-urlencoded");

            if (returnModel.status == "1")
            {
                rvm.Success = true;
                rvm.Msg = "success";
                rvm.Result = new
                {
                    IsFkLibSignUp = true
                };
            }
            else
            {
                rvm.Success = false;
                rvm.Msg = returnModel.message;
            }

            return rvm;
        }

        /// <summary>
        ///3、科室会同步
        /// </summary>
        public ReturnValueModel SyncMeetingInfo()
        {
            //两种方式：
            //1）费卡文库创建科室会，同步在小程序生成；缺点：一旦系统维护，不能及时同步
            //2）定期同步；缺点：非实时；每天凌晨科室会同步
            //
            //注：将采用定期同步：每天凌晨同步已完成的科室会到OneHCP数据库
            //同步内容：
            //科室会编号、科室会标题，活动状态，创建时间，召开时间，医院，科室，内容，参与人数

            ReturnValueModel rvm = new ReturnValueModel();
            var checkInPath = $@"{_host}/OneHCPService/ActivitySync.ashx";
            var returnModel = HttpUtils.PostResponse<ReturnValueSyncModel>(checkInPath, "", "application/x-www-form-urlencoded");
            if (returnModel.status == "1")
            {
                var sourceMeetings = JsonConvert.DeserializeObject<List<MeetingSyncModel>>(returnModel.result);

                var meetingList = new List<MeetInfo>();
                foreach (var item in sourceMeetings)
                {
                    var meet = new MeetInfo();
                    meet.Id = Guid.NewGuid().ToString();
                    meet.SourceId = item.ActivityID; //科室会ID
                    meet.MeetTitle = item.ActivityName; //科室会标题
                    meet.CreateTime = Convert.ToDateTime(item.CreatTime); //创建时间
                    meet.MeetDate = Convert.ToDateTime(item.HoldTime); //召开时间
                    meet.SourceHospital = item.Hospital; //医院
                    meet.SourceDepartment = item.KeShi; //科室
                    meet.MeetIntroduction = item.Context; //内容
                    meet.MeetingNumber = item.PartInNum; //参与人数
                    meet.IsCompleted = Core.Domain.Enums.EnumComplete.Approved;//已审核
                    meet.Source = _appId; //来源费卡文库
                    meetingList.Add(meet);
                }

                _rep.InsertList(meetingList);
                _rep.SaveChanges();
                rvm.Success = true;
                rvm.Msg = "success";
                rvm.Result = new
                {
                    done = meetingList.Count
                };
            }
            else
            {
                rvm.Success = false;
                rvm.Msg = returnModel.message;
            }

            return rvm;
        }

        /// <summary>
        /// 4、人员信息同步
        /// </summary>
        public ReturnValueModel SyncPersonInfo()
        {
            //将采用定期同步：每月凌晨同步更新，根据OneHCP唯一ID更新签到人员认证信息
            //同步字段：OneHCP医生验证状态、理由、云势ID

            ReturnValueModel rvm = new ReturnValueModel();
            var persons = _rep.Where<WxUserModel>(s => s.IsDeleted != 1).Select(x => new PerInfoSyncModel { OneHCPID = x.Id, YSID = x.yunshi_doctor_id, OneHCPReason = x.reason, OneHCPState = x.status }).ToList();
            if (persons.Count > 0)
            {
                var postStr = $"perInfos={JsonConvert.SerializeObject(persons)}";
                var syncPerInfoPath = $@"{_host}/OneHCPService/PerInfoSync.ashx";
                var returnModel = HttpUtils.PostResponse<ReturnValueSyncModel>(syncPerInfoPath, postStr, "application/x-www-form-urlencoded");
                if (returnModel.status == "1")
                {
                    rvm.Success = true;
                    rvm.Msg = "success";
                    rvm.Result = returnModel.result;
                }
                else
                {
                    rvm.Success = false;
                    rvm.Msg = returnModel.message;
                }
            }
            else
            {
                rvm.Success = true;
            }

            return rvm;
        }
    }
}
