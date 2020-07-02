/* ---------------------------------------------------------------------    
 * Copyright:
 * Wuxi ZoomSoft Technology Co., Ltd. All rights reserved. 
 * 
 * Class Description:
 * 
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2016/3/16 10:08:06 
 *
 * ------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.ExpressionHelper
{
    public class QueryCondition<T>
    {
        public Expression<Func<T, bool>> Where { get; set; }

        public QueryCondition(Expression<Func<T, bool>> condition = null)
        {
            if (condition != null)
            {
                this.Where = condition;
            }
        }

        public static QueryCondition<T> operator &(QueryCondition<T> condition, Expression<Func<T, bool>> right)
        {
            if (condition != null && condition.Where != null)
            {
                condition.Where = condition.Where.Splice(right, Expression.AndAlso);
            }
            else
            {
                condition.Where = right;
            }
            return condition;
        }
        public static QueryCondition<T> operator |(QueryCondition<T> condition, Expression<Func<T, bool>> right)
        {
            if (condition != null && condition.Where != null)
            {
                condition.Where = condition.Where.Splice(right, Expression.OrElse);
            }
            else
            {
                condition.Where = right;
            }
            return condition;
        }

        public static QueryCondition<T> operator &(QueryCondition<T> condition, QueryCondition<T> right)
        {
            if (right != null || right.Where != null)
            {
                condition &= right.Where;
            }
            return condition;
        }
        public static QueryCondition<T> operator |(QueryCondition<T> condition, QueryCondition<T> right)
        {
            if (right != null && right.Where != null)
            {
                condition |= right.Where;
            }
            return condition;
        }
    }
}
