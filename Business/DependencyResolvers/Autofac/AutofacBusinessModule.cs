using Autofac;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EfAuthorDal>().As<IAuthorDal>();
            builder.RegisterType<EfCategoryDal>().As<ICategoryDal>();
            builder.RegisterType<EfPostDal>().As<IPostDal>();
            builder.RegisterType<EfChatMessageDal>().As<IChatMessageDal>();

            builder.RegisterType<AuthorManager>().As<IAuthorService>();
            builder.RegisterType<CategoryManager>().As<ICategoryService>();
            builder.RegisterType<PostManager>().As<IPostService>();
            builder.RegisterType<ChatMessageManager>().As<IChatMessageService>();


            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces().EnableInterfaceInterceptors(new Castle.DynamicProxy.ProxyGenerationOptions() { Selector = new AspectInterceptorSelector() }).SingleInstance();
        }
    }
}
