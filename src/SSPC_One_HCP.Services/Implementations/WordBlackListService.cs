using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Linq;

namespace SSPC_One_HCP.Services.Implementations
{
    public class WordBlackListService : IWordBlackListService
    {
        private readonly IEfRepository _rep;

        public WordBlackListService(IEfRepository rep)
        {
            _rep = rep;
        }

        /// <summary>
        /// 新增或更新关键词
        /// </summary>
        /// <param name="meetInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdateWords(WordBlackList viewModel, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            if (string.IsNullOrEmpty(viewModel?.Type))
            {
                rvm.Msg = "The parameter 'Type' is required.";
                rvm.Success = false;
                return rvm;
            }

            WordBlackList model = null;
            if (!string.IsNullOrEmpty(viewModel.Id))
            {
                model = _rep.FirstOrDefault<WordBlackList>(s => s.Id == viewModel.Id && s.IsDeleted != 1);
                if (model == null)
                {
                    rvm.Msg = "Invalid Id";
                    rvm.Success = false;
                    return rvm;
                }
            }
            else
            {
                model = _rep.FirstOrDefault<WordBlackList>(s => s.Type == viewModel.Type && s.IsDeleted != 1);
            }

            bool isAdd = model == null;

            if (isAdd)
            {
                model = new WordBlackList();
                model.Id = Guid.NewGuid().ToString();
                model.CreateTime = DateTime.Now;
                model.CreateUser = workUser.User.Id;
            }
            else
            {
                model.UpdateTime = DateTime.Now;
                model.UpdateUser = workUser.User.Id;
            }

            model.Type = viewModel.Type;
            model.Words = viewModel.Words;

            if (isAdd)
            {
                _rep.Insert(model);
            }
            else
            {
                _rep.Update(model);
            }
            _rep.SaveChanges();

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = model;

            return rvm;
        }

        /// <summary>
        /// 删除关键词
        /// </summary>
        /// <param name="meetInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel DeleteWords(WordBlackList viewModel, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var model = _rep.FirstOrDefault<WordBlackList>(s => s.Id == viewModel.Id);

            if (model == null)
            {
                rvm.Success = false;
                rvm.Msg = "Invalid Id";
                return rvm;
            }

            if (model.IsDeleted == 1)
            {
                rvm.Msg = "It has been deleted already.";
                rvm.Success = true;
                return rvm;
            }

            model.IsDeleted = 1;

            _rep.Update(model);
            _rep.SaveChanges();

            rvm.Msg = "success";
            rvm.Success = true;

            return rvm;
        }

        /// <summary>
        /// 获取关键词列表
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetWordBlackLists(RowNumModel<WordBlackList> rowNum, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var list = _rep.Where<WordBlackList>(s => s.IsDeleted != 1);

            if (rowNum != null)
            {
                if (rowNum.SearchParams != null)
                {
                    if (!string.IsNullOrEmpty(rowNum.SearchParams.Type))
                    {
                        list = list.Where(s => s.Type == rowNum.SearchParams.Type);
                    }
                    if (!string.IsNullOrEmpty(rowNum.SearchParams.Words))
                    {
                        list = list.Where(s => s.Words.Contains(rowNum.SearchParams.Words));
                    }
                }
            }

            var total = list.Count();
            var rows = list.OrderBy(s => s.CreateTime).ToPaginationList(rowNum.PageIndex, rowNum.PageSize).ToList();

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                total,
                rows
            };
            return rvm;
        }

        /// <summary>
        /// 根据类型获取关键词
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string[] GetWordBlackListByType(string type, char separator = ',')
        {
            var model = _rep.FirstOrDefault<WordBlackList>(s => s.IsDeleted != 1 && s.Type == type);
            string words = model?.Words ?? "";
            return string.IsNullOrEmpty(words) ? new string[0] : words.Split(separator);
        }
    }
}
