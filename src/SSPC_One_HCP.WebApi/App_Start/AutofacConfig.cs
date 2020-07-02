using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Integration.WebApi;
using SSPC_One_HCP.AOP.AopInterceptor;
using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Core.Cache;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Data.Data;
using SSPC_One_HCP.OAuth.Implementations;
using SSPC_One_HCP.OAuth.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using WebGrease;
using SSPC_One_HCP.Core.Utils;

namespace SSPC_One_HCP.WebApi.App_Start
{
    /// <summary>
    /// 
    /// </summary>
    public static class AutofacConfig
    {
        /// <summary>
        /// IOC 
        /// </summary>
        /// <param name="config"></param>
        public static void AutofacIoc(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            #region AOP

            builder.Register(c => new RepositoryInterceptor()).InstancePerLifetimeScope();
            builder.Register(c => new ServiceInterceptor()).InstancePerLifetimeScope();
            #endregion

            #region Repository

            string connectStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            builder.Register<string>(c => connectStr);
            //builder.Register<IDbContext>(c => new FacDbContext(connectStr)).InstancePerLifetimeScope();
            builder.Register<IDbContext>(c => new FacDbContext(connectStr)).InstancePerDependency();
            //builder.RegisterGeneric(typeof(EfRepository)).As(typeof(IEfRepository)).InstancePerLifetimeScope().EnableInterfaceInterceptors().InterceptedBy(typeof(RepositoryInterceptor));
            //builder.RegisterType<EfRepository>().As<IEfRepository>().InstancePerLifetimeScope().EnableInterfaceInterceptors().InterceptedBy(typeof(RepositoryInterceptor));
            builder.RegisterType<EfRepository>()
                .As<IEfRepository>()
                .InstancePerDependency()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(RepositoryInterceptor));

            //builder.RegisterType<EFRepositoryGeneric>().As<IEfRepository>().InstancePerLifetimeScope().EnableInterfaceInterceptors().InterceptedBy(typeof(RepositoryInterceptor));
            builder.RegisterGeneric(typeof(EFRepositoryGeneric<>)).As(typeof(IEfRepositoryGeneric<>));
            #endregion

            #region OAuth2.0
            //builder.RegisterType<AuthRepository>().As<IAuthRepository>().SingleInstance().EnableInterfaceInterceptors().InterceptedBy(typeof(RepositoryInterceptor));
            builder.RegisterType<AuthRepository>().As<IAuthRepository>().InstancePerLifetimeScope().EnableInterfaceInterceptors().InterceptedBy(typeof(RepositoryInterceptor));
            //builder.RegisterType<IdentityUserStore>().As<IIdentityUserStore>().SingleInstance().EnableInterfaceInterceptors().InterceptedBy(typeof(RepositoryInterceptor));
            builder.RegisterType<IdentityUserStore>().As<IIdentityUserStore>().InstancePerLifetimeScope().EnableInterfaceInterceptors().InterceptedBy(typeof(RepositoryInterceptor));
            #endregion

            #region ApiController
            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerLifetimeScope();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            #endregion

            #region Services
            var services = Assembly.Load("SSPC_One_HCP.Services");
            builder.RegisterAssemblyTypes(services)
                .Where(s => s.Name.EndsWith("Service"))
                .AsImplementedInterfaces().EnableInterfaceInterceptors().InterceptedBy(typeof(ServiceInterceptor)).InstancePerLifetimeScope();
            #endregion

            #region Cache
            builder.RegisterType<MemoryCacheManager>().As<Core.Cache.ICacheManager>().SingleInstance();
            #endregion

            #region 基本配置
            builder.RegisterType<BaseConfig>().As<IConfig>().SingleInstance();
            #endregion

            var container = builder.Build();
            if (ContainerManager.Container == null)
            {
                ContainerManager.Container = container;
            }
            config.DependencyResolver = new AutofacWebApiDependencyResolver(ContainerManager.Container);
        }
    }
}