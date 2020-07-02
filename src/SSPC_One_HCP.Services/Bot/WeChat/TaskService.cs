using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSPC_One_HCP.KBS.OutDto;
using System.Configuration;
using System.Dynamic;
using SSPC_One_HCP.KBS.Webs.Clients;
using SSPC_One_HCP.KBS;
using SSPC_One_HCP.KBS.InputDto;
using System.Threading.Tasks;
using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Services.Bot.Dto;
using SSPC_One_HCP.Services.Interfaces;
using System.Data.SqlClient;

namespace SSPC_One_HCP.Services.Bot
{
    public class TaskService : ITaskService
    {
        private readonly IEfRepository _rep;
        public TaskService(IEfRepository rep)
        {
            _rep = rep;
        }

        /// <summary>
        /// 获取欢迎语
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        public async Task<ReturnValueModel> BOTWelcoming(string appId, int sex)
        {

            ReturnValueModel rvm = new ReturnValueModel
            {
                Msg = "success",
                Success = true
            };
            if (string.IsNullOrEmpty(appId))
            {
                rvm.Success = false;
                rvm.Msg = "fail";
                rvm.Result = "Bot配置异常，请联系管理员或在线客服。";
                return rvm;
            }
            try
            {
                var configure = _rep.FirstOrDefault<BotSaleConfigure>(o => o.IsDeleted == 0 && o.AppId == appId);
                if (configure == null)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = "Bot配置异常，请联系管理员或在线客服。";
                    return rvm;
                }
                string _host = ConfigurationManager.AppSettings["KBSUrl"];

                string loginSecretkey = ConfigurationManager.AppSettings["LoginSecretkey"];
                string sign = Tool.Sign(new Dictionary<string, object>
                {
                    { "botManageId", configure.KBSBotId },
                    { "sex", sex}
                 }, loginSecretkey);

                string url = $"{_host}{"TaskManage/Welcoming"}{"?botManageId="}{configure.KBSBotId}{"&sex="}{sex}{"&sign="}{sign}";

                var ret = await new WebClient<Result>()
                    .Get(url)
                    .ResultFromJsonAsync();
                if (ret?.Code == 0)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = ret?.Message;
                }

                rvm.Result = new
                {
                    botManageId = configure.KBSBotId,
                    welcome = ret.Data.ToString(),
                };

            }
            catch (Exception ex)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = ex.Message;
            }

            return rvm;
        }


        /// <summary>
        /// 用户会话主入口
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public async Task<ReturnValueModel> Entrance(TaskInputDto dto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel
            {
                Msg = "success",
                Success = true
            };
            if (string.IsNullOrEmpty(workUser?.WxSaleUser?.Id))
            {
                rvm.Msg = "NOT_LOGIN";
                rvm.Success = false;
                return rvm;
            }

            var user = _rep.FirstOrDefault<WxSaleUserModel>(s => s != null && s.IsDeleted != 1 && s.Id == workUser.WxSaleUser.Id);
            if (user == null)
            {
                rvm.Msg = "NOT_LOGIN";
                rvm.Success = false;
                return rvm;
            }
            if (string.IsNullOrEmpty(user.ADAccount))
            {
                rvm.Msg = "NOT_LOGIN";
                rvm.Success = false;
                return rvm;
            }
            try
            {
                string _host = ConfigurationManager.AppSettings["KBSUrl"];
                string url = $"{_host}{"TaskManage"}";

                string loginSecretkey = ConfigurationManager.AppSettings["LoginSecretkey"];
                string sign = Tool.Sign(new Dictionary<string, object>
                {
                    { "BotManageId", dto.BotManageId }
                 }, loginSecretkey);

                dto.Sign = sign;
                dto.UserId = workUser.WxSaleUser.Id;
                var ret = await new WebClient<Result>()
                    .Post(url)
                    .JsonData(dto)
                    .ResultFromJsonAsync();

                if (ret?.Code == 0)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = ret?.Message;
                    return rvm;
                }
                List<dynamic> entranceOutDtos = new List<dynamic>();
                var taskOutDto = KBS.Helpers.Json.ToObject<TaskOutDto>(ret.Data.ToString());

                if (taskOutDto.NodeType == 3)
                {
                    entranceOutDtos.Add(new { type = "main", content = taskOutDto.Result, hasSatisfied = true, selectSatisfied = 0 });
                }
                else
                {
                    entranceOutDtos.Add(new { type = "main", content = taskOutDto.Result });
                }

                if (taskOutDto.FAQRecommends != null && taskOutDto.FAQRecommends.Count > 0)
                {
                    var totalCount = taskOutDto.FAQRecommends.Count();//获取数
                    var pageSize = 5;//每页显示数
                    var totalPage = 0;
                    if ((totalCount % pageSize) == 0)
                        totalPage = totalCount / pageSize;
                    totalPage = (totalCount / pageSize) + 1;
                    List<List<string>> contents = new List<List<string>>();
                    for (int i = 0; i < totalPage; i++)
                    {
                        var content = taskOutDto.FAQRecommends.OrderBy(o => o).Skip(i).Take(pageSize).ToList();
                        contents.Add(content);
                    }
                    //faq 问题推荐
                    entranceOutDtos.Add(new { type = "faq", content = contents, curIndex = 0, maxIndex = totalPage - 1 });
                }

                //勋章返回
                var medals = AddUserMedal(taskOutDto, workUser);
                string _hostUrl = ConfigurationManager.AppSettings["HostUrl"];
                foreach (var item in medals)
                {
                    entranceOutDtos.Add(new { type = "medal", content = item.content, url = $"{_hostUrl}/{item.url}", name = item.name });
                }

                rvm.Result = new
                {
                    processId = taskOutDto.ProcessId,
                    taskId = taskOutDto.TaskId,
                    activityId = taskOutDto.ActivityId,
                    luisAppId = taskOutDto.LuisAppId,
                    botManageId = taskOutDto.BotManageId,
                    faqPackageId = taskOutDto.FaqPackageId,
                    faqId = taskOutDto.FaqId,
                    taskItemId = taskOutDto.TaskItemId,
                    msgResult = entranceOutDtos
                };

            }
            catch (Exception ex)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = ex.Message;
            }

            return rvm;
        }
        /// <summary>
        /// 满意度
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public async Task<ReturnValueModel> SatisfactionDegree(SatisfactionDegreeInputDto dto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel
            {
                Msg = "success",
                Success = true
            };
            try
            {
                dynamic obj = new ExpandoObject();
                string _host = ConfigurationManager.AppSettings["KBSUrl"];
                string url = $"{_host}{"TaskManage/SatisfactionDegree"}";
                string loginSecretkey = ConfigurationManager.AppSettings["LoginSecretkey"];

                var ret = await new WebClient<Result>()
                    .Post(url)
                    .JsonData(dto)
                    .ResultFromJsonAsync();

                if (ret?.Code == 0)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Result = ret?.Message;
                    return rvm;
                }
            }
            catch (Exception ex)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = ex.Message;
            }

            return rvm;
        }
        private List<UserMedalOutDto> AddUserMedal(TaskOutDto dto, WorkUser workUser)
        {
            List<UserMedalOutDto> medals = new List<UserMedalOutDto>();
            //var rep = ContainerManager.Resolve<IEfRepository>();
            using (var tran = _rep.Database.BeginTransaction())
            {
                try
                {
                    var recordData = _rep.FirstOrDefault<BotSaleUserTotalRecord>(o => o != null && o.IsDeleted != 1 && o.SaleUserId == workUser.WxSaleUser.Id);
                    if (recordData == null)
                    {
                        recordData = new BotSaleUserTotalRecord
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleUserId = workUser.WxSaleUser.Id,
                            Total = 1,
                            CreateTime = DateTime.Now,
                            CreateUser = workUser.WxSaleUser.Id,
                        };
                        _rep.Insert(recordData);
                    }
                    else
                    {
                        if (dto.IsNewTask)
                        {
                            recordData.Total += 1;
                            recordData.UpdateTime = DateTime.Now;
                            recordData.UpdateUser = workUser.WxSaleUser.Id;
                            _rep.Update(recordData);
                        }
                    }

                    //按次数
                    var standardDatas = _rep.Where<BotMedalStandardConfigure>(o => o != null && o.IsDeleted != 1 && o.Ruletotal == recordData.Total && o.KBSBotId == dto.BotManageId).ToList();
                    if (standardDatas != null && standardDatas.Count > 0)
                    {
                        foreach (var item in standardDatas)
                        {
                            var model = _rep.FirstOrDefault<BotSaleUserMedalInfo>(o => o != null && o.IsDeleted != 1 && o.SaleUserId == workUser.WxSaleUser.Id && o.BotMedalRuleId == item.Id);
                            if (model == null)
                            {
                                _rep.Insert(new BotSaleUserMedalInfo
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    BotMedalRuleId = item.Id,
                                    SaleUserId = workUser.WxSaleUser.Id,
                                    MedalSrc = item.MedalYSrc,
                                    MedalName = item.MedalName,
                                    MedalType = (int)SaleUserMedalType.Number,
                                    CreateTime = DateTime.Now,
                                    CreateUser = workUser.WxSaleUser.Id,
                                });
                                medals.Add(new UserMedalOutDto
                                {
                                    content = $"您已经和我对话{recordData.Total}次啦，搬个奖章给您",
                                    url = item.MedalYSrc,
                                    name = item.MedalName,
                                });
                            }
                        }
                    }


                    //按激活知识包
                    var businesModel = _rep.FirstOrDefault<BotMedalBusinessConfigure>(o => o != null && o.IsDeleted != 1 && o.FaqPackageId == dto.FaqPackageId && o.KBSBotId == dto.BotManageId);
                    if (businesModel != null)
                    {
                        var userMedal = _rep.FirstOrDefault<BotSaleUserMedalInfo>(o => o != null && o.IsDeleted != 1 && o.SaleUserId == workUser.WxSaleUser.Id && o.BotMedalRuleId == businesModel.Id);
                        if (userMedal == null)
                        {
                            _rep.Insert(new BotSaleUserMedalInfo
                            {
                                Id = Guid.NewGuid().ToString(),
                                BotMedalRuleId = businesModel.Id,
                                SaleUserId = workUser.WxSaleUser.Id,
                                MedalSrc = businesModel.MedalYSrc,
                                MedalName = businesModel.MedalName,
                                MedalType = (int)SaleUserMedalType.Pack,
                                CreateTime = DateTime.Now,
                                CreateUser = workUser.WxSaleUser.Id,
                            });
                            medals.Add(new UserMedalOutDto
                            {
                                content = $"恭喜您解锁知识勋章：{businesModel.MedalName}",
                                url = businesModel.MedalYSrc,
                                name = businesModel.MedalName,
                            });
                        }
                    }
                    _rep.SaveChanges();
                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                }
            }

            return medals;
        }

        /// <summary>
        /// 获取推荐
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public ReturnValueModel BOTRecommend(string appId, WorkUser workUser)
        {

            ReturnValueModel rvm = new ReturnValueModel
            {
                Msg = "success",
                Success = true
            };
            if (string.IsNullOrEmpty(appId))
            {
                rvm.Success = false;
                rvm.Msg = "fail";
                rvm.Result = "Bot配置异常，请联系管理员或在线客服。";
                return rvm;
            }

            SqlParameter[] paras = new SqlParameter[]
            {
               new SqlParameter("@id", appId)
            };
            var botManageOutDto = _rep.SqlQuery<BotManageOutDto>("select * from  KBS_BOT_MANAGE where BOT_MANAGE_ID=@id", paras).FirstOrDefault();
            if (!string.IsNullOrEmpty(botManageOutDto?.Recommend))
            {

                var recommends = KBS.Helpers.Json.ToObject<List<string>>(botManageOutDto.Recommend);
                rvm.Result = recommends;
            }

            return rvm;
        }

    }
}
