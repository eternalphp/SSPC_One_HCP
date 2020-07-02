using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Services.Bot
{
    public interface IBotManageService
    {
        /// <summary>
        ///BOT配置- 新增或修改
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdateSaleConfigure(BotSaleConfigure dto, WorkUser workUser);

        /// <summary>
        /// BOT配置- 获取配置信息
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetMenusSaleConfigure();

        /// <summary>
        /// BOT配置- 获取配置列表信息
        /// </summary>
        /// <param name="row"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetMenusSaleConfigureList(RowNumModel<BotSaleConfigure> row, WorkUser workUser);
        /// <summary>
        /// BOT配置- 删除
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel DeleteMenusSaleConfigure(BotSaleConfigure dto, WorkUser workUser);
        /// <summary>
        /// 勋章标准规则配置-新增或修改
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdateMedalStandardConfigure(BotMedalStandardConfigure dto, WorkUser workUser);
        /// <summary>
        /// 勋章标准规则配置- 根据ID获取配置信息
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetMedalStandardConfigure(string id);
        /// <summary>
        /// 勋章标准规则配置- 分页查询
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetMedalStandardConfigureList(RowNumModel<BotMedalStandardConfigure> row, WorkUser workUser);
        /// <summary>
        /// 勋章标准规则配置- 删除
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel DeleteMedalStandardConfigure(BotMedalStandardConfigure dto, WorkUser workUser);



        /// <summary>
        /// 勋章业务规则配置-新增或修改
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdateMedalBusinessConfigure(BotMedalBusinessConfigure dto, WorkUser workUser);

        /// <summary>
        /// 勋章业务规则配置- 根据ID获取配置信息
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetMedalBusinessConfigure(string id);
        /// <summary>
        /// 勋章业务规则配置- 分页查询
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetMedalBusinessConfigureList(RowNumModel<BotMedalBusinessConfigure> row, WorkUser workUser);
        /// <summary>
        /// 勋章业务规则配置- 删除
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel DeleteMedalBusinessConfigure(BotMedalBusinessConfigure dto, WorkUser workUser);
    }
}
