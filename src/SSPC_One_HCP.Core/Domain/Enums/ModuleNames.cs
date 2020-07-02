using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Enums
{
    /// <summary>
    /// 模块名称枚举
    /// </summary>
    public enum ModuleNames
    {
        /// <summary>
        /// 用户授权
        /// </summary>
        UserAuth = 1,
        /// <summary>
        /// 用户注册
        /// </summary>
        UserRegister = 2,
        /// <summary>
        /// 用户申诉
        /// </summary>
        UserComplain = 3,
        /// <summary>
        /// 用户注销
        /// </summary>
        UserCancle = 4,
        /// <summary>
        /// 扫码添加其他医生名片
        /// </summary>
        ScanAddCard = 5,
        /// <summary>
        /// 扫码签到会议
        /// </summary>
        ScanSignMeet = 6,
        /// <summary>
        /// 查看用药参考
        /// </summary>
        CheckMedicateRefence = 7,
        /// <summary>
        /// 查看其他医生的具体名片
        /// </summary>
        CheckOtherDocCard = 8,
        /// <summary>
        /// 查看临床指南
        /// </summary>
        CheckGuid = 9,
        /// <summary>
        /// 参加某某的会议
        /// </summary>
        AttendMeet = 10,
        /// <summary>
        /// 查看会议中主讲医生的名片
        /// </summary>
        CheckMeetCard = 11,
        /// <summary>
        /// 查看具体的临床指南
        /// </summary>
        CheckGuidDetail = 12,

        /// <summary>
        /// 参加某某某会议的问卷
        /// </summary>
        CheckMeetQues = 13,
        /// <summary>
        /// 参看某某产品资料
        /// </summary>
        CheckProductInfo = 14,
        /// <summary>
        /// 收听某某播客
        /// </summary>
        ListenBoardCast = 15,
        /// <summary>
        /// 收藏某某会议，产品资料，播客
        /// </summary>
        CollectSth = 16,
        /// <summary>
        /// 分享某某会议，产品资料，播客
        /// </summary>
        ShareSth = 17,
        /// <summary>
        /// 进行意见反馈
        /// </summary>
        FeedBack = 18,

        /// <summary>
        /// 用户验证
        /// </summary>
        UserCheck=19

    }
}
