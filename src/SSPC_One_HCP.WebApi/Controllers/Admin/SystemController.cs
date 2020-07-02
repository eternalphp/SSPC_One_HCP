using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.ViewModels.MeetModels;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.Admin
{
    /// <summary>
    /// 系统设置
    /// </summary>
    public class SystemController : BaseApiController
    {
        private readonly IWechatPublicAccountService _wxPublicAccService;
        private readonly ISystemService _systemService;
        private readonly IWordBlackListService _wordBlackListService;

        /// <summary>
        /// 系统设置
        /// </summary>
        /// <param name="wxPublicAccService">微信公众号</param>
        /// <param name="systemService"></param>
        /// <param name="wordBlackListService"></param>
        public SystemController(IWechatPublicAccountService wxPublicAccService, ISystemService systemService, IWordBlackListService wordBlackListService)
        {
            _wxPublicAccService = wxPublicAccService;
            _systemService = systemService;
            _wordBlackListService = wordBlackListService;
        }
        
        #region 微信公众号管理

        /// <summary>
        /// 获取微信公众号列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetWechatPublicAccountList(RowNumModel<WechatPublicAccount> rowNum)
        {
            var ret = _wxPublicAccService.GetWechatPublicAccountList(rowNum, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 获取单个微信公众号信息
        /// </summary>
        /// <param name="accountInfo">微信公众号信息</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetWechatPublicAccountInfo(WechatPublicAccount accountInfo)
        {
            var ret = _wxPublicAccService.GetWechatPublicAccountInfo(accountInfo, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 新增或修改微信公众号信息
        /// </summary>
        /// <param name="accountInfo">微信公众号信息</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateWechatPublicAccount(WechatPublicAccount accountInfo)
        {
            var ret = _wxPublicAccService.AddOrUpdateWechatPublicAccount(accountInfo, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 删除微信公众号信息
        /// </summary>
        /// <param name="accountInfo">微信公众号信息</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteWechatPublicAccount(WechatPublicAccount accountInfo)
        {
            var ret = _wxPublicAccService.DeleteWechatPublicAccount(accountInfo, WorkUser);
            return Ok(ret);
        }

        #endregion

        /// <summary>
        /// 是否启用管理员审核功能
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetAdminApprovalEnabled()
        {
            var ret = _systemService.GetAdminApprovalEnabled();
            return Ok(ret);
        }

        /// <summary>
        /// 设置是否启用管理员审核功能
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SetAdminApprovalEnabled(ConfigurationViewModel<bool?> model)
        {
            var ret = _systemService.SetAdminApprovalEnabled(model, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 获取意见反馈列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetFeedbackList(RowNumModel<FeedbackListViewModel> rowNum)
        {
            var ret = _systemService.GetFeedbackList(rowNum);
            return Ok(ret);
        }

        /// <summary>
        /// 新增或更新关键词黑名单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateWordBlackList(WordBlackList viewModel)
        {
            var ret = _wordBlackListService.AddOrUpdateWords(viewModel, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 删除关键词黑名单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteWordBlackList(WordBlackList viewModel)
        {
            var ret = _wordBlackListService.DeleteWords(viewModel, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 获取关键词列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
    
        public IHttpActionResult GetWordBlackLists(RowNumModel<WordBlackList> rowNum)
        {
            var ret = _wordBlackListService.GetWordBlackLists(rowNum, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetConfig()
        {
            var ret = _systemService.GetConfig("MeetInfoEnabled");
            return Ok(ret);
        }

    }
}
