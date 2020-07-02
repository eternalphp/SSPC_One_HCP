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
    public class ParameterReset : ExpressionVisitor
    {
        private Dictionary<ParameterExpression, ParameterExpression> Dictionary { get; set; }

        public ParameterReset(Dictionary<ParameterExpression, ParameterExpression> dictionary)
        {
            this.Dictionary = dictionary ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameterList(Dictionary<ParameterExpression, ParameterExpression> dictionary, Expression expression)
        {
            return new ParameterReset(dictionary).Visit(expression);
        }

        protected override Expression VisitParameter(ParameterExpression param)
        {
            ParameterExpression replaceParam;
            if (Dictionary.TryGetValue(param, out replaceParam))
            {
                param = replaceParam;
            }
            return base.VisitParameter(param);
        }
    }
}
