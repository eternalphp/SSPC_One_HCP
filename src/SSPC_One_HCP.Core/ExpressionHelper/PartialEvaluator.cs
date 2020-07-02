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
    /// <summary>
    /// 
    /// </summary>
    public class PartialEvaluator : DbExpressionVisitor
    {
        #region Fields
        private Func<Expression, bool> fnCanBeEvaluated;
        private HashSet<Expression> candidates;
        #endregion

        #region Ctor
        public PartialEvaluator()
            : this(CanBeEvaluatedLocally)
        {
        }

        public PartialEvaluator(Func<Expression, bool> fnCanBeEvaluated)
        {
            this.fnCanBeEvaluated = fnCanBeEvaluated;
        }
        #endregion

        #region Methods
        public Expression Eval(Expression exp)
        {
            this.candidates = new Nominator(this.fnCanBeEvaluated).Nominate(exp);

            return this.VisitExpression(exp);
        }

        protected override Expression VisitExpression(Expression exp)
        {
            if (exp == null)
            {
                return null;
            }

            if (this.candidates.Contains(exp))
            {
                return this.Evaluate(exp);
            }

            return base.VisitExpression(exp);
        }

        private Expression Evaluate(Expression e)
        {
            if (e.NodeType == ExpressionType.Constant)
            {
                return e;
            }

            LambdaExpression lambda = Expression.Lambda(e);
            Delegate fn = lambda.Compile();

            return Expression.Constant(fn.DynamicInvoke(null), e.Type);
        }

        private static bool CanBeEvaluatedLocally(Expression exp)
        {
            return exp.NodeType != ExpressionType.Parameter;
        }
        #endregion

        #region Inner Class
        private class Nominator : DbExpressionVisitor
        {
            private Func<Expression, bool> fnCanBeEvaluated;
            private HashSet<Expression> candidates;
            private bool cannotBeEvaluated;

            internal Nominator(Func<Expression, bool> fnCanBeEvaluated)
            {
                this.fnCanBeEvaluated = fnCanBeEvaluated;
            }

            internal HashSet<Expression> Nominate(Expression expression)
            {
                this.candidates = new HashSet<Expression>();
                this.VisitExpression(expression);
                return this.candidates;
            }

            protected override Expression VisitExpression(Expression expression)
            {
                if (expression != null)
                {
                    bool saveCannotBeEvaluated = this.cannotBeEvaluated;
                    this.cannotBeEvaluated = false;

                    base.VisitExpression(expression);

                    if (!this.cannotBeEvaluated)
                    {
                        if (this.fnCanBeEvaluated(expression))
                        {
                            this.candidates.Add(expression);
                        }
                        else
                        {
                            this.cannotBeEvaluated = true;
                        }
                    }

                    this.cannotBeEvaluated |= saveCannotBeEvaluated;
                }

                return expression;
            }
        }
        #endregion
    }
}
