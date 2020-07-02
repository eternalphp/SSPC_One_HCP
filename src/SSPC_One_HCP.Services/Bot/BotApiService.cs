using SSPC_One_HCP.Core.Cache;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.KBS;
using SSPC_One_HCP.KBS.OutDto;
using SSPC_One_HCP.KBS.Webs.Clients;
using SSPC_One_HCP.Services.Bot.Dto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace SSPC_One_HCP.Services.Bot
{
    public class BotApiService : IBotApiService
    {
        private readonly IEfRepository _rep;
        private readonly ICacheManager _cache;
        public BotApiService(IEfRepository rep, ICacheManager cache)
        {
            _rep = rep;
            _cache = cache;
        }
        /// <summary>
        /// 获取KBS bot信息
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValueModel> BotInfo(WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            try
            {
                var token = _cache.Get<KBSTokenOutDto>(workUser.User.Id + "KBS");
                string _host = ConfigurationManager.AppSettings["KBSUrl"];
                var result = await new WebClient<Result>()
                            .Get($"{_host}BotManage/Select")
                            .Header("Authorization", $"Bearer { token.Auth_token}")
                            .ResultFromJsonAsync();
                if (result?.Code == 0)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = result?.Message;
                    return rvm;
                }
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = result.Data;
            }
            catch (Exception e)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = e.Message;
            }
            return rvm;
        }

        /// <summary>
        /// 根据KBS BOT ID查询知识包
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValueModel> GetByBotIdPacks(string id, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(id))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Id' is required.";
                return rvm;
            }
            try
            {
                var token = _cache.Get<KBSTokenOutDto>(workUser.User.Id + "KBS");
                string _host = ConfigurationManager.AppSettings["KBSUrl"];
                var result = await new WebClient<Result>()
                            .Get($"{_host}BotManage/{id}")
                                .Header("Authorization", $"Bearer { token.Auth_token}")
                            .ResultFromJsonAsync();
                if (result?.Code == 0)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = result?.Message;
                    return rvm;
                }
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = result.Data;
            }
            catch (Exception e)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = e.Message;
            }
            return rvm;

        }

        /// <summary>
        /// 内容自动标签-添加
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public async Task<ReturnValueModel> CreateAutomaticContentTag(AutomaticContentTagInputDto dto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            try
            {
                var token = _cache.Get<KBSTokenOutDto>(workUser.User.Id + "KBS");
                string _host = ConfigurationManager.AppSettings["KBSUrl"];
                var result = await new WebClient<Result>()
                            .Post($"{_host}AutomaticContentTag/CreateTag")
                                    .Header("Authorization", $"Bearer { token.Auth_token}")
                            .JsonData(dto)
                            .ResultFromJsonAsync();
                if (result?.Code == 0)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = result?.Message;
                    return rvm;
                }
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = result.Data;
            }
            catch (Exception e)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = e.Message;
            }
            return rvm;
        }
        /// <summary>
        /// 内容自动标签-根据内容ID 获取自动标签
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public async Task<ReturnValueModel> GetAutomaticContentTag(string id, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(id))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Id' is required.";
                return rvm;
            }
            try
            {
                var token = _cache.Get<KBSTokenOutDto>(workUser.User.Id + "KBS");
                string _host = ConfigurationManager.AppSettings["KBSUrl"];
                var result = await new WebClient<Result>()
                            .Get($"{_host}AutomaticContentTag/QueryTag/{id}")
                                     .Header("Authorization", $"Bearer { token.Auth_token}")
                            .ResultFromJsonAsync();
                if (result?.Code == 0)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = result?.Message;
                    return rvm;
                }
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = result.Data;
            }
            catch (Exception e)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = e.Message;
            }
            return rvm;
        }
        /// <summary>
        /// 内容自动标签-删除
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public async Task<ReturnValueModel> DeleteAutomaticContentTag(string id, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            try
            {
                var token = _cache.Get<KBSTokenOutDto>(workUser.User.Id + "KBS");
                string _host = ConfigurationManager.AppSettings["KBSUrl"];
                var result = await new WebClient<Result>()
                            .Delete($"{_host}AutomaticContentTag/DeleteTag/{id}")
                                    .Header("Authorization", $"Bearer { token.Auth_token}")
                            .ResultFromJsonAsync();
                if (result?.Code == 0)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = result?.Message;
                    return rvm;
                }
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = result.Data;
            }
            catch (Exception e)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = e.Message;
            }
            return rvm;
        }

        /// <summary>
        /// 内容业务标签-添加
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public async Task<ReturnValueModel> CreateBusinessContentTag(BusinessContentTagInputDto dto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            try
            {
                var token = _cache.Get<KBSTokenOutDto>(workUser.User.Id + "KBS");
                string _host = ConfigurationManager.AppSettings["KBSUrl"];
                var result = await new WebClient<Result>()
                            .Post($"{_host}BusinessContentTag/CreateTag")
                                    .Header("Authorization", $"Bearer { token.Auth_token}")
                            .JsonData(dto)
                            .ResultFromJsonAsync();
                if (result?.Code == 0)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = result?.Message;
                    return rvm;
                }
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = result.Data;
            }
            catch (Exception e)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = e.Message;
            }
            return rvm;
        }

        /// <summary>
        /// 内容业务标签-根据内容ID 获取业务标签
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public async Task<ReturnValueModel> GetBusinessContentTagDto(string id, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(id))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Id' is required.";
                return rvm;
            }
            try
            {
                var token = _cache.Get<KBSTokenOutDto>(workUser.User.Id + "KBS");
                string _host = ConfigurationManager.AppSettings["KBSUrl"];
                var result = await new WebClient<Result>()
                            .Get($"{_host}BusinessContentTag/QueryTag/{id}")
                                     .Header("Authorization", $"Bearer { token.Auth_token}")
                            .ResultFromJsonAsync();
                if (result?.Code == 0)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = result?.Message;
                    return rvm;
                }
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = result.Data;
            }
            catch (Exception e)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = e.Message;
            }
            return rvm;
        }
        /// <summary>
        /// 内容业务标签-删除
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public async Task<ReturnValueModel> DeleteBusinessContentTagDto(string id, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(id))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Id' is required.";
                return rvm;
            }
            try
            {
                var token = _cache.Get<KBSTokenOutDto>(workUser.User.Id + "KBS");
                string _host = ConfigurationManager.AppSettings["KBSUrl"];
                var result = await new WebClient<Result>()
                            .Delete($"{_host}BusinessContentTag/DeleteTag/{id}")
                                     .Header("Authorization", $"Bearer { token.Auth_token}")
                            .ResultFromJsonAsync();
                if (result?.Code == 0)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = result?.Message;
                    return rvm;
                }
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = result.Data;
            }
            catch (Exception e)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = e.Message;
            }
            return rvm;
        }

        /// <summary>
        /// 内容：上传文件 
        /// </summary>
        /// <param name="httpFile"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public async Task<ReturnValueModel> Upload(HttpFileCollection httpFile, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            try
            {
                List<ContentUploadInputDto> contentInputs = new List<ContentUploadInputDto>();
                for (int i = 0; i < httpFile.Count; i++)
                {
                    HttpPostedFile file = httpFile[i];
                    //获得到文件名
                    string fileName = System.IO.Path.GetFileName(file.FileName.ToString());
                    ////获得文件扩展名
                    //string fileNameEx = System.IO.Path.GetExtension(fileName);
                    ////没有扩展名的文件名
                    //string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);
                    contentInputs.Add(new ContentUploadInputDto
                    {
                        FileName = fileName,
                        ContentType = file.ContentType,
                        InputStream = StreamToBytes(file.InputStream)
                    });
                }
               // var token = _cache.Get<KBSTokenOutDto>(workUser.User.Id + "KBS");
                string _host = ConfigurationManager.AppSettings["KBSUrl"];
                var result = await new WebClient<Result>()
                            .Post($"{_host}Content/Upload")
                                  //  .Header("Authorization", $"Bearer { token.Auth_token}")
                            .JsonData(contentInputs)
                            .ResultFromJsonAsync();
                if (result?.Code == 0)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = result?.Message;
                    return rvm;
                }
                dynamic obj = new ExpandoObject();
                obj.Url = ConfigurationManager.AppSettings["KBSImageUrl"];
                obj.Data = result.Data;
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = obj;
            }
            catch (Exception e)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = e.Message;
            }
            return rvm;
        }

        /// 将 Stream 转成 byte[]
        byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }


        /// <summary>
        /// 内容： 
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public async Task<ReturnValueModel> CreateContent(ContentCreateDto dto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            try
            {
                var token = _cache.Get<KBSTokenOutDto>(workUser.User.Id + "KBS");
                string _host = ConfigurationManager.AppSettings["KBSUrl"];
                var result = await new WebClient<Result>()
                            .Post($"{_host}Content")
                            .BearerToken(token.Auth_token)
                            .JsonData(dto)
                            .ResultFromJsonAsync();
                if (result?.Code == 0)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = result?.Message;
                    return rvm;
                }
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = result.Data;
            }
            catch (Exception e)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = e.Message;
            }
            return rvm;
        }


    }
}
