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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Collection;

namespace SSPC_One_HCP.Core.ExpressionHelper
{
    /// <summary>
    /// 
    /// </summary>
    public class ConditionBuilder : DbExpressionVisitor
    {
        #region Fields
        /// <summary>
        /// 参数列表（有顺序的列表）
        /// </summary>
        private readonly List<object> arguments = null;

        /// <summary>
        /// 堆栈列表（后进先出）
        /// </summary>
        private readonly Stack<string> conditionParts = null;

        /// <summary>
        /// 转换二元运算符为数据库关键字
        /// </summary>
        private readonly IDictionary<ExpressionType, string> operatorList = null;

        /// <summary>
        /// 转换方法调用为数据库关键字
        /// </summary>
        private readonly IDictionary<string, string> methodList = null;

        private string condition = null;

        private string _tableName;
        #endregion

        #region Ctor
        /// <summary>
        /// 构造函数
        /// </summary>
        public ConditionBuilder()
        {
            this.arguments = new List<object>();
            this.conditionParts = new Stack<string>();

            // 初始化转换二元运算符为数据库关键字字典
            this.operatorList = new Dictionary<ExpressionType, string>();
            this.operatorList.Add(ExpressionType.Equal, "=");
            this.operatorList.Add(ExpressionType.NotEqual, "<>");
            this.operatorList.Add(ExpressionType.GreaterThan, ">");
            this.operatorList.Add(ExpressionType.GreaterThanOrEqual, ">=");
            this.operatorList.Add(ExpressionType.LessThan, "<");
            this.operatorList.Add(ExpressionType.LessThanOrEqual, "<=");
            this.operatorList.Add(ExpressionType.AndAlso, "AND");
            this.operatorList.Add(ExpressionType.OrElse, "OR");
            this.operatorList.Add(ExpressionType.Add, "+");
            this.operatorList.Add(ExpressionType.Subtract, "-");
            this.operatorList.Add(ExpressionType.Multiply, "*");
            this.operatorList.Add(ExpressionType.Divide, "/");

            // 转换方法调用为数据库关键字
            this.methodList = new Dictionary<string, string>();
            this.methodList.Add("StartsWith", "({0} LIKE {1}+'%')");
            this.methodList.Add("Contains", "({0} LIKE '%'+{1}+'%')");
            this.methodList.Add("EndsWith", "({0} LIKE '%'+{1})");
            this.methodList.Add("Equals", "({0} = {1})");
        }
        #endregion

        #region Properties
        /// <summary>
        /// where条件的string
        /// </summary>
        public string Condition
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.condition))
                {
                    this.condition = this.conditionParts.Count > 0 ? this.conditionParts.Pop() : null;
                }
                return this.condition;
            }
        }

        /// <summary>
        /// 参数列表
        /// </summary>
        public object[] Arguments
        {
            get
            {
                return this.arguments.ToArray();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 加载条件表达式树
        /// </summary>
        /// <param name="expression"></param>
        public void Build(Expression expression, string tableName)
        {
            _tableName = tableName;
            PartialEvaluator evaluator = new PartialEvaluator();
            Expression evaluatedExpression = evaluator.Eval(expression);
            this.VisitExpression(evaluatedExpression);
        }

        /// <summary>
        /// 重写二元运算符条件表达式解析
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression b)
        {
            if (b == null)
            {
                return b;
            }

            // 转换二元运算符为数据库关键字
            string opratorExpression = this.operatorList.GetValue<ExpressionType, string>(b.NodeType);
            if (opratorExpression == null)
            {
                return b;
            }

            // 获取表达式左边部分
            this.VisitExpression(b.Left);

            // 获取表达式右边部分
            this.VisitExpression(b.Right);

            string right = this.conditionParts.Pop();
            string left = this.conditionParts.Pop();

            string condition = String.Format("({0} {1} {2})", left, opratorExpression, right);
            this.conditionParts.Push(condition);

            return b;
        }

        /// <summary>
        /// 重写常量表达式的解析
        /// </summary>
        /// <param name="constant"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression constant)
        {
            if (constant == null)
            {
                return constant;
            }
            if (!constant.Value.GetType().Equals(typeof(List<string>)) &&
                !constant.Value.GetType().Equals(typeof(List<long>)) &&
                !constant.Value.GetType().Equals(typeof(List<int>)))
            {
                this.arguments.Add(constant.Value);
            }
            this.conditionParts.Push(String.Format("@Param{0}", this.arguments.Count - 1));
            return constant;
        }

        /// <summary>
        /// 重写字段属性的访问
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            if (m == null)
            {
                return m;
            }

            PropertyInfo propertyInfo = m.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                return m;
            }

            this.conditionParts.Push(String.Format("{0}", this._tableName + "." + propertyInfo.Name));

            return m;
        }

        /// <summary>
        /// 重写方法的调用
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m == null)
            {
                return m;
            }

            // 转换方法调用数据库关键字
            string format = string.Empty;
            if (!this.methodList.TryGetValue(m.Method.Name, out format) || string.IsNullOrWhiteSpace(format))
            {
                throw new NotSupportedException(m.NodeType + " is not supported.");
            }

            this.VisitExpression(m.Object);
            this.VisitExpression(m.Arguments[0]);
            string right = this.conditionParts.Pop();
            string left = this.conditionParts.Pop();
            if (m.Object.Type.Equals(typeof(List<string>)))
            {
                format = "{1} in ('{0}')";
                left = string.Join("','", m.Object.Eval() as List<string>);
            }
            else if (m.Object.Type.Equals(typeof(List<int>)))
            {
                format = "{1} in ('{0}')";
                left = string.Join("','", m.Object.Eval() as List<int>);
            }
            else if (m.Object.Type.Equals(typeof(List<long>)))
            {
                format = "{1} in ('{0}')";
                left = string.Join("','", m.Object.Eval() as List<long>);
            }
            this.conditionParts.Push(String.Format(format, left, right));

            return m;
        }
        #endregion
    }
}
