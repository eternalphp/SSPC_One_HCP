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
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.ExpressionHelper
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DbExpressionVisitor
    {
        #region Ctor
        protected DbExpressionVisitor()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// 解析表达式树
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected virtual Expression VisitExpression(Expression expression)
        {
            if (expression == null)
            {
                return expression;
            }
            switch (expression.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                    return this.VisitUnary(expression as UnaryExpression);
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                    return this.VisitBinary(expression as BinaryExpression);
                case ExpressionType.TypeIs:
                    return this.VisitTypeIs(expression as TypeBinaryExpression);
                case ExpressionType.Conditional:
                    return this.VisitConditional(expression as ConditionalExpression);
                case ExpressionType.Constant:
                    return this.VisitConstant(expression as ConstantExpression);
                case ExpressionType.Parameter:
                    return this.VisitParameter(expression as ParameterExpression);
                case ExpressionType.MemberAccess:
                    return this.VisitMemberAccess(expression as MemberExpression);
                case ExpressionType.Call:
                    return this.VisitMethodCall(expression as MethodCallExpression);
                case ExpressionType.Lambda:
                    return this.VisitLambda(expression as LambdaExpression);
                case ExpressionType.New:
                    return this.VisitNew(expression as NewExpression);
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    return this.VisitNewArray(expression as NewArrayExpression);
                case ExpressionType.Invoke:
                    return this.VisitInvocation(expression as InvocationExpression);
                case ExpressionType.MemberInit:
                    return this.VisitMemberInit(expression as MemberInitExpression);
                case ExpressionType.ListInit:
                    return this.VisitListInit(expression as ListInitExpression);
                default:
                    throw new ArgumentException(string.Format("Unhandled expression Type: '{0}'", expression.NodeType));
            }
        }

        /// <summary>
        /// 成员的访问
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        protected virtual MemberBinding VisitBinding(MemberBinding binding)
        {
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    return this.VisitMemberAssignment(binding as MemberAssignment);
                case MemberBindingType.MemberBinding:
                    return this.VisitMemberMemberBinding(binding as MemberMemberBinding);
                case MemberBindingType.ListBinding:
                    return this.VisitMemberListBinding(binding as MemberListBinding);
                default:
                    throw new ArgumentException(string.Format("Unhandled binding Type '{0}'", binding.BindingType));
            }
        }

        /// <summary>
        /// 集合的单个元素的初始值设定项
        /// </summary>
        /// <param name="initializer"></param>
        /// <returns></returns>
        protected virtual ElementInit VisitElementInitializer(ElementInit initializer)
        {
            ReadOnlyCollection<Expression> arguments = this.VisitExpressionList(initializer.Arguments);
            if (arguments != initializer.Arguments)
            {
                return Expression.ElementInit(initializer.AddMethod, arguments);
            }
            return initializer;
        }

        /// <summary>
        /// 包含一元运算符的表达式
        /// </summary>
        /// <param name="unary"></param>
        /// <returns></returns>
        protected virtual Expression VisitUnary(UnaryExpression unary)
        {
            Expression operand = this.VisitExpression(unary.Operand);
            if (operand != unary.Operand)
            {
                return Expression.MakeUnary(unary.NodeType, operand, unary.Type, unary.Method);
            }
            return unary;
        }

        /// <summary>
        /// 包含二元运算符的表达式
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected virtual Expression VisitBinary(BinaryExpression b)
        {
            Expression left = this.VisitExpression(b.Left);
            Expression right = this.VisitExpression(b.Right);
            Expression conversion = this.VisitExpression(b.Conversion);
            if (left != b.Left || right != b.Right || conversion != b.Conversion)
            {
                if (b.NodeType == ExpressionType.Coalesce && b.Conversion != null)
                    return Expression.Coalesce(left, right, conversion as LambdaExpression);
                else
                    return Expression.MakeBinary(b.NodeType, left, right, b.IsLiftedToNull, b.Method);
            }
            return b;
        }

        /// <summary>
        /// 表达式和类型之间的操作
        /// </summary>
        /// <param name="typeItem"></param>
        /// <returns></returns>
        protected virtual Expression VisitTypeIs(TypeBinaryExpression typeItem)
        {
            Expression expr = this.VisitExpression(typeItem.Expression);
            if (expr != typeItem.Expression)
            {
                return Expression.TypeIs(expr, typeItem.TypeOperand);
            }
            return typeItem;
        }

        /// <summary>
        /// 常量值的表达式
        /// </summary>
        /// <param name="constant"></param>
        /// <returns></returns>
        protected virtual Expression VisitConstant(ConstantExpression constant)
        {
            return constant;
        }

        /// <summary>
        /// 条件运算符的表达式
        /// </summary>
        /// <param name="conditional"></param>
        /// <returns></returns>
        protected virtual Expression VisitConditional(ConditionalExpression conditional)
        {
            Expression test = this.VisitExpression(conditional.Test);
            Expression ifTrue = this.VisitExpression(conditional.IfTrue);
            Expression ifFalse = this.VisitExpression(conditional.IfFalse);
            if (test != conditional.Test || ifTrue != conditional.IfTrue || ifFalse != conditional.IfFalse)
            {
                return Expression.Condition(test, ifTrue, ifFalse);
            }
            return conditional;
        }

        /// <summary>
        /// 命名参数的表达式
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected virtual Expression VisitParameter(ParameterExpression parameter)
        {
            return parameter;
        }

        /// <summary>
        /// 字段属性的表达式
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        protected virtual Expression VisitMemberAccess(MemberExpression member)
        {
            Expression exp = this.VisitExpression(member.Expression);
            if (exp != member.Expression)
            {
                return Expression.MakeMemberAccess(exp, member.Member);
            }
            return member;
        }

        /// <summary>
        /// 方法调用的表达式
        /// </summary>
        /// <param name="methodCall"></param>
        /// <returns></returns>
        protected virtual Expression VisitMethodCall(MethodCallExpression methodCall)
        {
            Expression method = this.VisitExpression(methodCall.Object);
            IEnumerable<Expression> args = this.VisitExpressionList(methodCall.Arguments);
            if (method != methodCall.Object || args != methodCall.Arguments)
            {
                return Expression.Call(method, methodCall.Method, args);
            }
            return methodCall;
        }

        /// <summary>
        /// 字段属性的赋值运算表达式
        /// </summary>
        /// <param name="assignment"></param>
        /// <returns></returns>
        protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
        {
            Expression e = this.VisitExpression(assignment.Expression);
            if (e != assignment.Expression)
            {
                return Expression.Bind(assignment.Member, e);
            }
            return assignment;
        }

        /// <summary>
        /// 初始化新创建对象的成员的成员
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
        {
            IEnumerable<MemberBinding> bindings = this.VisitBindingList(binding.Bindings);
            if (bindings != binding.Bindings)
            {
                return Expression.MemberBind(binding.Member, bindings);
            }
            return binding;
        }

        /// <summary>
        /// 初始化新创建对象的集合成员的元素
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding binding)
        {
            IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(binding.Initializers);
            if (initializers != binding.Initializers)
            {
                return Expression.ListBind(binding.Member, initializers);
            }
            return binding;
        }

        protected virtual ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
        {
            List<Expression> list = null;
            for (int i = 0, n = original.Count; i < n; i++)
            {
                Expression p = this.VisitExpression(original[i]);
                if (list != null)
                {
                    list.Add(p);
                }
                else if (p != original[i])
                {
                    list = new List<Expression>(n);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }
                    list.Add(p);
                }
            }
            if (list != null)
            {
                return list.AsReadOnly();
            }
            return original;
        }

        protected virtual IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> original)
        {
            List<MemberBinding> list = null;
            for (int i = 0, n = original.Count; i < n; i++)
            {
                MemberBinding b = this.VisitBinding(original[i]);
                if (list != null)
                {
                    list.Add(b);
                }
                else if (b != original[i])
                {
                    list = new List<MemberBinding>(n);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }
                    list.Add(b);
                }
            }
            if (list != null)
                return list;
            return original;
        }

        protected virtual IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> original)
        {
            List<ElementInit> list = null;
            for (int i = 0, n = original.Count; i < n; i++)
            {
                ElementInit init = this.VisitElementInitializer(original[i]);
                if (list != null)
                {
                    list.Add(init);
                }
                else if (init != original[i])
                {
                    list = new List<ElementInit>(n);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }
                    list.Add(init);
                }
            }
            if (list != null)
                return list;
            return original;
        }

        /// <summary>
        /// 获取lambda表达式的语句块
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        protected virtual Expression VisitLambda(LambdaExpression lambda)
        {
            Expression body = this.VisitExpression(lambda.Body);
            if (body != lambda.Body)
            {
                return Expression.Lambda(lambda.Type, body, lambda.Parameters);
            }
            return lambda;
        }

        /// <summary>
        /// 构造函数调用的表达式
        /// </summary>
        /// <param name="nex"></param>
        /// <returns></returns>
        protected virtual NewExpression VisitNew(NewExpression nex)
        {
            IEnumerable<Expression> args = this.VisitExpressionList(nex.Arguments);
            if (args != nex.Arguments)
            {
                if (nex.Members != null)
                    return Expression.New(nex.Constructor, args, nex.Members);
                else
                    return Expression.New(nex.Constructor, args);
            }
            return nex;
        }

        /// <summary>
        /// 调用构造函数并初始化新对象的一个或多个成员
        /// </summary>
        /// <param name="init"></param>
        /// <returns></returns>
        protected virtual Expression VisitMemberInit(MemberInitExpression init)
        {
            NewExpression n = this.VisitNew(init.NewExpression);
            IEnumerable<MemberBinding> bindings = this.VisitBindingList(init.Bindings);
            if (n != init.NewExpression || bindings != init.Bindings)
            {
                return Expression.MemberInit(n, bindings);
            }
            return init;
        }

        /// <summary>
        /// 包含集合初始值设定项的构造函数调用
        /// </summary>
        /// <param name="init"></param>
        /// <returns></returns>
        protected virtual Expression VisitListInit(ListInitExpression init)
        {
            NewExpression n = this.VisitNew(init.NewExpression);
            IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(init.Initializers);
            if (n != init.NewExpression || initializers != init.Initializers)
            {
                return Expression.ListInit(n, initializers);
            }
            return init;
        }

        /// <summary>
        /// 创建新数组并可能初始化该新数组的元素
        /// </summary>
        /// <param name="na"></param>
        /// <returns></returns>
        protected virtual Expression VisitNewArray(NewArrayExpression na)
        {
            IEnumerable<Expression> exprs = this.VisitExpressionList(na.Expressions);
            if (exprs != na.Expressions)
            {
                if (na.NodeType == ExpressionType.NewArrayInit)
                {
                    return Expression.NewArrayInit(na.Type.GetElementType(), exprs);
                }
                else
                {
                    return Expression.NewArrayBounds(na.Type.GetElementType(), exprs);
                }
            }
            return na;
        }

        /// <summary>
        /// 将委托或 lambda 表达式应用于参数表达式列表的表达式
        /// </summary>
        /// <param name="iv"></param>
        /// <returns></returns>
        protected virtual Expression VisitInvocation(InvocationExpression iv)
        {
            IEnumerable<Expression> args = this.VisitExpressionList(iv.Arguments);
            Expression expr = this.VisitExpression(iv.Expression);
            if (args != iv.Arguments || expr != iv.Expression)
            {
                return Expression.Invoke(expr, args);
            }
            return iv;
        }
        #endregion
    }
}
