using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.ViewModels.DoctorModels;
using System.Collections.Generic;

namespace SSPC_One_HCP.Services.Interfaces
{
    /// <summary>
    /// 后台医生接口
    /// </summary>
    public interface IADDoctorService
    {
        /// <summary>
        /// 获取医生列表
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetDoctorList(RowNumModel<DoctorViewModel> rowNum);



        /// <summary>
        /// [废弃]导出医生信息
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        ReturnValueModel ExportDoctorList(RowNumModel<WxUserModel> rowNum, List<string> ids = null);

        /// <summary>
        /// 导出医生信息
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        ReturnValueModel ExportDoctorList(RowNumModel<DoctorViewModel> rowNum, List<string> ids = null);

        /// <summary>
        /// 医生学习时间列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        ReturnValueModel GetDoctorLearn(RowNumModel<DoctorLearnViewModel> rowNum, bool IsExported = false);

        /// <summary>
        /// 获取医生详情
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetDoctorDetail(DoctorViewModel viewModel);

        /// <summary>
        /// 给医生加手动标签
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        ReturnValueModel UpdateTagsOfDoctor(DoctorTagView viewModel, WorkUser workUser);

        /// <summary>
        /// 新增或编辑标签
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdateTagInfo(TagInfo viewModel, WorkUser workUser);

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        ReturnValueModel DeleteTagInfo(TagInfo viewModel, WorkUser workUser);

        /// <summary>
        /// 获取医生手动标签列表
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetTagList();

        /// <summary>
        /// 获取医生的学习浏览列表
        /// </summary>
        /// <param name="wxUserId">wxUserId</param>
        /// <returns></returns>

        ReturnValueModel GetStudyList(RowNumModel<DoctorDetailViewModel> rownum);


        /// <summary>
        /// 获取用户详情的模块浏览记录
        /// </summary>
        /// <param name="wxUserId"></param>
        /// <param name="rownum"></param>
        /// <returns></returns>
        ReturnValueModel GetVisitModulesList(RowNumModel<DoctorDetailViewModel> rownum);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="wxuserids"></param>
        /// <returns></returns>
        ReturnValueModel DelWxUserModel(List<string> wxuserids);


        /// <summary>
        /// 后台用户认证审核
        /// </summary>
        /// <param name="rownum"></param>
        /// <returns></returns>
        ReturnValueModel DoctorVerify(RowNumModel<DoctorVerifyViewModel> rownum);


        /// <summary>
        /// 申诉用户详情
        /// </summary>
        /// <param name="rownum"></param>
        /// <returns></returns>
        ReturnValueModel DoctorVerifyDetail(DoctorViewModel rownum);

        /// <summary>
        ///  医生列表
        /// 根据科室和标签分组
        /// </summary>
        /// <param name="rownum"></param>
        /// <returns></returns>
        ReturnValueModel GetDoctorByTagDep(RowNumModel<DoctorMeeting> rownum);

        /// <summary>
        /// 获取内部员工
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        ReturnValueModel GetInternalStaffList(RowNumModel<DoctorViewModel> rowNum);
        /// <summary>
        /// 导出内部员工
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        ReturnValueModel ExportInternalStaffList(RowNumModel<DoctorViewModel> rowNum, List<string> ids = null);
    }
}
