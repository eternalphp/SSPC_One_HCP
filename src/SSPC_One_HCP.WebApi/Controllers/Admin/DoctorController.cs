using Bot.Tool;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.ViewModels.DoctorModels;
using SSPC_One_HCP.Services.Implementations.Dto;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Linq;


namespace SSPC_One_HCP.WebApi.Controllers.Admin
{
    /// <summary>
    /// 后台医生接口
    /// </summary>
    public class DoctorController : BaseApiController
    {
        private readonly IADDoctorService _iADDoctorService;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'DoctorController.DoctorController(IADDoctorService)'
        public DoctorController(IADDoctorService aDDoctorService)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'DoctorController.DoctorController(IADDoctorService)'
        {
            _iADDoctorService = aDDoctorService;
        }

        /// <summary>
        /// 获取医生列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetDoctorList(RowNumModel<DoctorViewModel> rowNum)
        {
            var ret = _iADDoctorService.GetDoctorList(rowNum);
            return Ok(ret);
        }
  
        /// <summary>
        /// 获取医生详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetDoctorDetail(DoctorViewModel viewModel)
        {
            var ret = _iADDoctorService.GetDoctorDetail(viewModel);
            return Ok(ret);
        }

        /// <summary>
        /// 手动给医生加标签
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateTagsOfDoctor(DoctorTagView viewModel)
        {
            var ret = _iADDoctorService.UpdateTagsOfDoctor(viewModel, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 新增或编辑标签
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateTagInfo(TagInfo viewModel)
        {
            var ret = _iADDoctorService.AddOrUpdateTagInfo(viewModel, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteTagInfo(TagInfo viewModel)
        {
            var ret = _iADDoctorService.DeleteTagInfo(viewModel, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 获取医生手动标签列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetTagList()
        {
            var ret = _iADDoctorService.GetTagList();
            return Ok(ret);
        }

        /// <summary>
        /// 导出医生信息
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ExportDoctorList(RowNumModel<DoctorViewModel> rowNum, [FromUri]List<string> ids = null)
        {
            var ret = _iADDoctorService.ExportDoctorList(rowNum, ids);
            return Ok(ret);
        }

        /// <summary>
        /// 获取医生学习时间
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetDoctorLearn(RowNumModel<DoctorLearnViewModel> rowNum)
        {
            var ret = _iADDoctorService.GetDoctorLearn(rowNum);
            return Ok(ret);
        }
        /// <summary>
        /// 导出-医生学习时间
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        public void ExportDoctorLearn(RowNumModel<DoctorLearnViewModel> rowNum)
        {

            HttpContext curContext = HttpContext.Current;
            var ret = _iADDoctorService.GetDoctorLearn(rowNum, true);
            if (ret.Success)
            {
                var _filePath = HostingEnvironment.MapPath("/ExcelTemplate/" + "学习时间模板.xlsx");
                var data = ret.Result as List<DoctorLearnViewModel>;
                List<DoctorLearnDto> dto = new List<DoctorLearnDto>();
                foreach (var item in data)
                {
                    dto.Add(new DoctorLearnDto
                    {
                        DoctorName = item.DoctorName,
                        Title = item.Title,
                        HospitalName = item.HospitalName,
                        DepartmentName = item.DepartmentName,
                        DocLearnTime = KBS.Tool.SecondsToDateTime(item.DocLearnTime.GetValueOrDefault()),
                        PodcastLearnTime = KBS.Tool.SecondsToDateTime(item.PodcastLearnTime.GetValueOrDefault()),
                        VideoLearnTime = KBS.Tool.SecondsToDateTime(item.VideoLearnTime.GetValueOrDefault()),
                        MeetCount = item.MeetCount,
                        BroadcastTime = KBS.Tool.SecondsToDateTime(item.BroadcastTime.GetValueOrDefault()),
                        GuidVistTime = KBS.Tool.SecondsToDateTime(item.GuidVistTime.GetValueOrDefault()),
                        MedicineVistTime = KBS.Tool.SecondsToDateTime(item.MedicineVistTime.GetValueOrDefault()),
                        BookVisitTime = KBS.Tool.SecondsToDateTime(item.BookVisitTime.GetValueOrDefault()),
                        DocTags = string.Join(",", item.DocTags),
                    });
                }

                var bytes = AsposeExcelTool.ExportTemplate(_filePath, "H", "医生学习时间", dto);

                string fileName = DateTime.Now.ToString("yyyyMMssff");

                curContext.Response.Clear();
                curContext.Response.Buffer = true;
                curContext.Response.Charset = "UTF-8";
                curContext.Response.AddHeader("content-disposition", $"attachment; filename={fileName}.xlsx");
                curContext.Response.ContentEncoding = Encoding.UTF8;  //必须写，否则会有乱码
                curContext.Response.ContentType = "application/octet-stream";
                curContext.Response.AddHeader("Content-Length", bytes.Length.ToString());
                curContext.Response.OutputStream.Write(bytes, 0, bytes.Length);
                curContext.Response.Flush();
                curContext.Response.Close();
            }


            //string path = HostingEnvironment.MapPath("/Upload/Export");

            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}
            //FileStream fs = new FileStream(path + @"/" + fileName, FileMode.Create);
            //fs.Write(excell, 0, excell.Length);
            //fs.Dispose();
            //string _host = ConfigurationManager.AppSettings["HostUrl"];
            //return _host + "/Upload/Export/" + fileName;
        }
        /// <summary>
        /// 获取医生学习时间详情列表
        /// </summary>
        /// <param name="wxuserId"> </param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetStudyList(RowNumModel<DoctorDetailViewModel> rowNum)
        {
            var ret = _iADDoctorService.GetStudyList(rowNum);
            return Ok(ret);
        }
        /// <summary>
        /// 获取用户模块浏览详情列表
        /// </summary>
        /// <param name="wxuserId"> </param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetVisitModulesList(RowNumModel<DoctorDetailViewModel> rowNum)
        {
            var ret = _iADDoctorService.GetVisitModulesList(rowNum);
            return Ok(ret);
        }
        /// <summary>
        /// 批量删除用户(IsDeleted=1)
        /// </summary>
        /// <param name="wxuserids"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DelWxUserModel(List<string> wxuserids)
        {
            var ret = _iADDoctorService.DelWxUserModel(wxuserids);
            return Ok();
        }
        /// <summary>
        /// 用户后台认证
        /// </summary>
        /// <param name="rownum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DoctorVerify(RowNumModel<DoctorVerifyViewModel> rownum)
        {
            var ret = _iADDoctorService.DoctorVerify(rownum);
            return Ok();
        }

        /// <summary>
        /// 申诉用户详情
        /// </summary>
        /// <param name="rownum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DoctorVerifyDetail(DoctorViewModel rownum)
        {
            var ret = _iADDoctorService.DoctorVerifyDetail(rownum);
            return Ok();
        }

        /// <summary>
        /// 医生列表
        /// 根据科室和标签分组
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetDoctorByTagDep(RowNumModel<DoctorMeeting> rowNum)
        {
            var ret = _iADDoctorService.GetDoctorByTagDep(rowNum);
            return Ok(ret);
        }

        /// <summary>
        /// 获取内部员工
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetInternalStaffList(RowNumModel<DoctorViewModel> rowNum)
        {
            var ret = _iADDoctorService.GetInternalStaffList(rowNum);
            return Ok(ret);
        }

        /// <summary>
        /// 导出内部员工信息
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ExportInternalStaffList(RowNumModel<DoctorViewModel> rowNum, [FromUri]List<string> ids = null)
        {
            var ret = _iADDoctorService.ExportInternalStaffList(rowNum, ids);
            return Ok(ret);
        }

    }
}
