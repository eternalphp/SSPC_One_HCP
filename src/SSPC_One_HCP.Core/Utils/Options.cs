using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Utils
{
    /// <summary>
    /// 注解相关值对应的含义
    /// </summary>
    public static class Options
    {
        #region 资料文档类别
        /// <summary>
        /// 资料类别PPT
        /// </summary>
        public const int DataType_PPT = 1;

        /// <summary>
        /// 资料类别PDF
        /// </summary>
        public const int DataType_PDF = 2;

        /// <summary>
        /// 资料类别音频
        /// </summary>
        public const int DataType_Audio = 3;

        /// <summary>
        /// 资料类别文章
        /// </summary>
        public const int DataType_DOC = 4;

        /// <summary>
        /// 资料类别视频
        /// </summary>
        public const int DataType_Vidio = 5;

        #endregion

        #region 会议类别

        /// <summary>
        /// 线下会议
        /// </summary>
        public const int MeetType_OffLine = 0;

        /// <summary>
        /// 线下会议
        /// </summary>
        public const int MeetType_Online = 1;

        #endregion

        #region 知识库分类

        /// <summary>
        /// 产品资料
        /// </summary>
        public const int ZishiKuType_Product = 1;

        /// <summary>
        /// 学术资料
        /// </summary>
        public const int ZishiKuType_XueShu = 2;

        /// <summary>
        /// 播客
        /// </summary>
        public const int ZishiKuType_BoKe = 3;
        #endregion
    }
}
