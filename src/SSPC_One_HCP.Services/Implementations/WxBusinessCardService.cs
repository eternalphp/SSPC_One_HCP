using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Linq;

namespace SSPC_One_HCP.Services.Implementations
{
    public class WxBusinessCardService : IWxBusinessCardService
    {
        private readonly IEfRepository _rep;

        public WxBusinessCardService(IEfRepository rep)
        {
            _rep = rep;
        }
        public ReturnValueModel AddBusinessCard(AddBusinessCardViewModel viewModel, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            if (string.IsNullOrEmpty(viewModel?.OwnerWxUserId)) //string.IsNullOrEmpty(viewModel.ObjectUnionId) && 
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'OwnerWxUserId' is required.";
                return rvm;
            }

            //名片上是谁的信息，谁就是cardOwner
            var cardOwner = _rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1
                && (!string.IsNullOrEmpty(s.Id) && s.Id == viewModel.OwnerWxUserId)); //|| (!string.IsNullOrEmpty(s.UnionId) && s.UnionId == viewModel.ObjectUnionId)

            if (cardOwner == null)
            {
                rvm.Success = false;
                rvm.Msg = "Target user not found.";
                return rvm;
            }

            if (cardOwner.Id == workUser.WxUser.Id)
            {
                rvm.Success = false;
                rvm.Msg = "不能添加自己的名片";
                return rvm;
            }

            //自己名片夹的名片
            var card1 = _rep.FirstOrDefault<BusinessCard>(s => s.IsDeleted != 1 && s.WxUserId == workUser.WxUser.Id && s.OwnerWxUserId == cardOwner.Id);
            if (card1 != null)
            {
                rvm.Msg = "名片已存在";
                rvm.Success = false;
                return rvm;
            }

            //对方名片夹的名片
            var card2 = _rep.FirstOrDefault<BusinessCard>(s => s.IsDeleted != 1 && s.OwnerWxUserId == workUser.WxUser.Id && s.WxUserId == cardOwner.Id);

            using (var tran = _rep.Database.BeginTransaction())
            {
                try
                {
                    //插入本人名片夹
                    card1 = new BusinessCard();
                    card1.Id = Guid.NewGuid().ToString();
                    card1.WxUserId = workUser.WxUser.Id;
                    card1.OwnerWxUserId = cardOwner.Id;
                    card1.CreateTime = DateTime.Now;
                    card1.IsDeleted = 0;
                    card1.IsEnabled = 0;
                    _rep.Insert(card1);
                    _rep.SaveChanges();

                    //插入对方名片夹
                    if (card2 == null)
                    {
                        card2 = new BusinessCard();
                        card2.Id = Guid.NewGuid().ToString();
                        card2.WxUserId = card1.OwnerWxUserId;
                        card2.OwnerWxUserId = card1.WxUserId;
                        card2.CreateTime = DateTime.Now;
                        card2.IsDeleted = 0;
                        card2.IsEnabled = 0;
                        _rep.Insert(card2);
                        _rep.SaveChanges();
                    }

                    tran.Commit();

                }
                catch (Exception ex)
                {
                    rvm.Msg = ex.Message;
                    rvm.Success = false;
                    tran.Rollback();
                    return rvm;
                }
            }
            
            rvm.Success = true;
            rvm.Msg = "";
            return rvm;
        }
        public ReturnValueModel GetBusinessCardList(RowNumModel<BusinessCard> businessCard, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var list = (from a in _rep.Where<BusinessCard>(s => s.IsDeleted != 1 && s.OwnerWxUserId != null && s.WxUserId == workUser.WxUser.Id)
                        join b in _rep.Where<WxUserModel>(s => s.IsDeleted != 1&&s.HospitalName!=null) on a.OwnerWxUserId equals b.Id
                        into ab
                        from bb in ab.DefaultIfEmpty()
                        select new BusinessCardViewModel
                        {
                            UserName = bb.UserName == null ? "" : bb.UserName,
                            WxPicture = bb.WxPicture == null ? "" : bb.WxPicture,
                            Mobile = bb.Mobile == null ? "" : bb.Mobile,
                            HospitalName = bb.HospitalName == null ? "" : bb.HospitalName,
                            DepartmentName = bb.DepartmentName == null ? "" : bb.DepartmentName,
                            CreateTime = a.CreateTime
                        }).Where(s=>s.DepartmentName!="");

            //var list = _rep.Table<WxUserModel>().Where(s => s.UnionId != null &&  s.UnionId == workUser.WxUser.UnionId).ToList();

            //增加名片的关键字过滤
            if (businessCard.SearchParams.Remark != null)
            {
                var keyword = businessCard.SearchParams.Remark;
                list = list.Where(s =>
                    s.UserName.Contains(keyword) || s.DepartmentName.Contains(keyword) ||
                    s.HospitalName.Contains(keyword)  );
            }

            var total = list.Count();
            var rows = list.OrderByDescending(o => o.CreateTime)
                .ToPaginationList(businessCard.PageIndex, businessCard.PageSize);


            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                total = total,
                rows = rows
            };
            return rvm;
        }

    }
}
