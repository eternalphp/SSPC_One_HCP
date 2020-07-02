using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Implementations
{
    /// <summary>
    /// 发送频率
    /// </summary>
    public class SendRateService: IBaseService<SendRate>
    {
        private readonly IEfRepository _rep;
        public SendRateService(IEfRepository rep)
        {
            _rep = rep;
        }

        /// <summary>
        /// 获取单个配置
        /// </summary>
        /// <param name="item"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetItem(SendRate item, WorkUser workUser)
        {
            var doctorId = item?.DoctorId ?? "";
            var result = new ReturnValueModel();
            if (!string.IsNullOrEmpty(doctorId))
            {
                result.Msg = "Invalid parameters.";
                result.Success = false;
                return result;
            }
            var data = _rep.Where<SendRate>(x => x.IsDefault == item.IsDefault);
            data.FirstOrDefault(x => x.DoctorId == doctorId);
            result.Msg = "Success";
            result.Success = true;
            result.Result = data;
            return result;
        }
         
        /// <summary>
        /// 增改
        /// ID 不存在则为新增，否则修改（默认状态请传入 ID）
        /// </summary>
        /// <param name="sendRate"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdateItem(SendRate sendRate, WorkUser workUser)
        {
            var result = new ReturnValueModel();
            if (sendRate?.SendCycleType == null)
            {
                result.Msg = "Invalid parameters.";
                result.Success = false;
                return result;
            }
            try
            {
                var item = _rep.FirstOrDefault<SendRate>(x => x.Id == sendRate.Id);               
                if (item==null)
                {
                   
                    sendRate.Id = Guid.NewGuid().ToString();
                    sendRate.CreateTime = DateTime.Now;
                    sendRate.CreateUser = workUser.User.Id;
                    _rep.Insert(sendRate);
                }
                else
                {
                    item.UpdateTime = DateTime.Now;
                    item.UpdateUser = workUser.User.Id;
                    _rep.Update(item);
                }

                bool data = _rep.SaveChanges() > 0 ? true : false;
                result.Msg = "Success";
                result.Success = data;             
            }
            catch (Exception e)
            {
                result.Msg = "Error";
                result.Success = false;
            }
            return result;
        }

        public ReturnValueModel GetList(RowNumModel<SendRate> item, WorkUser workUser)
        {
            throw new NotImplementedException();
        }

        public ReturnValueModel DeleteItem(SendRate item, WorkUser workUser)
        {
            throw new NotImplementedException();
        }

        public ReturnValueModel AddItem(SendRate data, WorkUser workUser)
        {
            throw new NotImplementedException();
        }
    }
}
