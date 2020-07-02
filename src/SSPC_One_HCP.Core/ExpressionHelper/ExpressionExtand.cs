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
    public static class ExpressionExtand
    {
        public static object Eval(this Expression expr)
        {
            return expr.Eval<object>();
        }

        public static T Eval<T>(this Expression expr)
        {
            var constantExpr = expr as ConstantExpression;
            if (constantExpr != null)
            {
                return (T)constantExpr.Value;
            }
            var fun = Expression.Lambda<Func<object>>(Expression.Convert(expr, typeof(object))).Compile();

            return (T)(fun());
        }


        //整合两个表达式
        public static Expression<T> Splice<T>(this Expression<T> left, Expression<T> right, Func<Expression, Expression, Expression> method)
        {
            var dictionary = left.Parameters.Select((f, i) => new { f, s = right.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            //替换参数
            var secondBody = ParameterReset.ReplaceParameterList(dictionary, right.Body);
            return Expression.Lambda<T>(method(left.Body, secondBody), left.Parameters);
        }
    }
}
