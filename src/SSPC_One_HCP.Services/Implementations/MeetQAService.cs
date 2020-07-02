using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels.MeetModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SSPC_One_HCP.Services.Implementations
{
    public class MeetQAService : IMeetQAService
    {
        private readonly IEfRepository _rep;

        public MeetQAService(IEfRepository rep)
        {
            _rep = rep;
        }

        /// <summary>
        /// 新增或修改问卷
        /// </summary>
        /// <param name="meetQA"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdateMeetQA(MeetQAContentViewModel meetQA, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            meetQA = FillToChangeQAModel(meetQA);
            var question = _rep.FirstOrDefault<QuestionModel>(s => s.Id == meetQA.Question.Id);
            if (question == null)
            {
                using (var tran = _rep.Database.BeginTransaction())
                {
                    try
                    {
                        meetQA.Question.Id = Guid.NewGuid().ToString();
                        meetQA.Question.CreateTime = DateTime.Now;
                        meetQA.Question.CreateUser = workUser.User.Id;

                        _rep.Insert(meetQA.Question);
                        _rep.SaveChanges();
                        foreach (var item in meetQA.Answers)
                        {
                            item.QuestionId = meetQA.Question.Id;
                            item.Id = Guid.NewGuid().ToString();
                            item.CreateTime = DateTime.Now;
                            item.CreateUser = workUser.User.Id;
                            _rep.Insert(item);
                        }
                        _rep.SaveChanges();
                        tran.Commit();
                        rvm.Msg = "success";
                        rvm.Success = true;
                        rvm.Result = meetQA;
                    }
                    catch (Exception ex)
                    {
                        rvm.Msg = "fail";
                        rvm.Success = false;
                        tran.Rollback();
                    }
                }
            }
            else
            {
                using (var tran = _rep.Database.BeginTransaction())
                {
                    try
                    {
                        question.QuestionContent = meetQA.Question.QuestionContent;
                        question.QuestionType = meetQA.Question.QuestionType;
                        question.MeetId = meetQA.Question.MeetId;
                        question.UpdateTime = DateTime.Now;
                        question.UpdateUser = workUser.User.Id;

                        _rep.Update(question);
                        _rep.SaveChanges();

                        var list = _rep.Where<AnswerModel>(s => s.QuestionId == question.Id);
                        _rep.DeleteList(list);
                        _rep.SaveChanges();

                        foreach (var item in meetQA.Answers)
                        {
                            item.QuestionId = meetQA.Question.Id;
                            item.Id = Guid.NewGuid().ToString();
                            item.CreateTime = DateTime.Now;
                            item.CreateUser = workUser.User.Id;
                            _rep.Insert(item);
                        }
                        _rep.SaveChanges();
                        tran.Commit();
                        rvm.Msg = "success";
                        rvm.Success = true;
                        rvm.Result = meetQA;
                    }
                    catch (Exception ex)
                    {
                        rvm.Msg = "fail";
                        rvm.Success = false;
                        tran.Rollback();
                    }
                }
            }

            return rvm;
        }

        public ReturnValueModel DeleteMeetQA(QuestionModel meetQA, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var question = _rep.FirstOrDefault<QuestionModel>(s => s.Id == meetQA.Id);
            if (question != null)
            {
                using (var tran = _rep.Database.BeginTransaction())
                {
                    try
                    {
                        var list = _rep.Where<AnswerModel>(s => s.QuestionId == question.Id);
                        _rep.Delete(question);
                        _rep.DeleteList(list);
                        _rep.SaveChanges();

                        tran.Commit();
                        rvm.Msg = "sucess";
                        rvm.Success = true;
                    }
                    catch (Exception ex)
                    {
                        rvm.Msg = "fail";
                        rvm.Success = false;
                        tran.Rollback();
                    }
                }
            }

            return rvm;
        }

        public ReturnValueModel GetMeetQA(QuestionModel question, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var meetQA = new MeetQAContentViewModel();
            meetQA.Question = _rep.FirstOrDefault<QuestionModel>(s => s.Id == question.Id);
            meetQA.Answers = _rep.Where<AnswerModel>(s => s.IsDeleted != 1 && s.QuestionId == meetQA.Question.Id).OrderBy(s => s.Sort);

            if (meetQA.Question != null)
            {
                meetQA = FillFromQA(meetQA);

                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = new
                {
                    meetQA = meetQA
                };
            }
            else
            {
                rvm.Msg = "fail";
                rvm.Success = false;
            }

            return rvm;
        }

        /// <summary>
        /// 获取问卷列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public ReturnValueModel GetMeetQAList(RowNumModel<MeetQAContentViewModel> rowNum)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var list = from a in _rep.All<QuestionModel>()
                       join b in _rep.All<AnswerModel>() on a.Id equals b.QuestionId
                       where a.IsDeleted != 1 && b.IsDeleted != 1 && string.IsNullOrEmpty(a.MeetId)
                       group b by a
                       into j
                       select new MeetQAContentViewModel
                       {
                           Question = j.Key,
                           Answers = j.Select(s => s)
                       };
            list = list.Where(rowNum.SearchParams);

            var total = list.Count();
            var rows = list.OrderByDescending(s => s.Question.UpdateTime).ToPaginationList(rowNum.PageIndex, rowNum.PageSize);

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                total = total,
                rows = rows
            };

            return rvm;
        }

        /// <summary>
        /// 获取问卷列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public ReturnValueModel GetQuestionList(RowNumModel<QuestionModel> rowNum)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var list = _rep.Where<QuestionModel>(s => s.IsDeleted != 1 && string.IsNullOrEmpty(s.MeetId)).Where(rowNum.SearchParams);

            var total = list.Count();
            var rows = list.OrderByDescending(s => s.CreateTime).ToPaginationList(rowNum.PageIndex, rowNum.PageSize);

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                total = total,
                rows = rows
            };

            return rvm;
        }

        private MeetQAContentViewModel FillToChangeQAModel(MeetQAContentViewModel meetQAView)
        {
            MeetQAContentViewModel meetQA = new MeetQAContentViewModel();
            meetQA = meetQAView;
            //填空做转换
            if (meetQA.Question.QuestionType == 3)
            {
                string a = meetQAView.Question.QuestionContent;
                var r = Regex.Matches(a, @"\{(.+?)\}");
                int sort = 1;
                List<AnswerModel> list = new List<AnswerModel>();
                AnswerModel item;
                foreach (Match x in r)
                {
                    item = new AnswerModel();
                    item.AnswerContent = x.Groups[1].Value;
                    item.IsRight = true;
                    item.Sort = sort.ToString();
                    meetQA.Question.QuestionContent = meetQA.Question.QuestionContent.Replace(x.Groups[0].Value, "{" + sort + "}");
                    list.Add(item);
                    sort++;
                }

                meetQA.Answers = list;
            }

            return meetQA;
        }

        private MeetQAContentViewModel FillFromQA(MeetQAContentViewModel meetQAView)
        {
            MeetQAContentViewModel meetQA = meetQAView;
            //填空做转换
            if (meetQA.Question?.QuestionType == 3)
            {
                string a = meetQAView.Question.QuestionContent;
                var r = Regex.Matches(a, @"\{(.+?)\}");
                foreach (Match x in r)
                {
                    string item = x.Groups[1].Value;
                    var answer = meetQAView.Answers.FirstOrDefault(s => s.IsRight == true && s.Sort.Equals(item)).AnswerContent;
                    meetQA.Question.QuestionContent = meetQAView.Question.QuestionContent.Replace(item, answer);
                }
            }

            return meetQA;
        }
    }
}
