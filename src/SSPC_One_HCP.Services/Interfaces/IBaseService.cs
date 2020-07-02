using SSPC_One_HCP.Core.Domain.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SSPC_One_HCP.Services.Interfaces
{
    /// <summary>
    /// 增删改
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseService<T> where T : class, new()
    {
        ReturnValueModel GetList(RowNumModel<T> item, WorkUser workUser);
        ReturnValueModel GetItem(T item, WorkUser workUser);     
        ReturnValueModel AddOrUpdateItem(T data, WorkUser workUser);
        ReturnValueModel DeleteItem(T item, WorkUser workUser);
        ReturnValueModel AddItem(T data, WorkUser workUser);
    }
}
