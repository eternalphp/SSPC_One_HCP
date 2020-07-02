using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;

namespace SSPC_One_HCP.Services.Interfaces
{
    public interface IWxKnowledgeService
    {
        /// <summary>
        /// 知识库页面数据
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetWxKnowledgePage(WorkUser workUser);

        /// <summary>
        /// 播客列表
        /// </summary>
        /// <param name="rowData">分页</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel PodcastList(RowNumModel<DataInfo> rowData, WorkUser workUser);

        /// <summary>
        /// 更改已读未读状态
        /// </summary>
        /// <param name="myReadRecord">阅读记录</param>
        /// <param name="workUser">当前操作人</param>
        /// <returns></returns>
        ReturnValueModel IsReadStatus(MyReadRecord myReadRecord, WorkUser workUser);

        /// <summary>
        /// 产品资料列表
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetProductMediaList(RowNumModel<MediaDataRelViewModel> dataInfo, WorkUser workUser);

        /// <summary>
        /// 播客详情
        /// </summary>
        /// <param name="dataInfo">传入Id</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AudioMediaDetail(DataInfo dataInfo, WorkUser workUser);

        /// <summary>
        /// 临床指南列表
        /// </summary>
        /// <param name="dataInfo">分页、搜索</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetGuideList(RowNumModel<DataInfo> dataInfo, WorkUser workUser);

        /// <summary>
        /// 知识库产品详情
        /// </summary>
        /// <param name="dataInfo">传入Id</param>
        /// <returns></returns>
        ReturnValueModel WxKnowledgeDetail(DataInfo dataInfo);
        // <summary>
        /// 根据用户获取用药参考
        /// </summary>
        /// <param name="wxuser"></param>
        /// <returns></returns>
        ReturnValueModel ClinicalguidelinesByUser(WorkUser workUser);

        /// <summary>
        /// 赞同不赞同
        /// </summary>
        /// <param name="like"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel IsLike(ProductInfoLikeView data);

    }
}
