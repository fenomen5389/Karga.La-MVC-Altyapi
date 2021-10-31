using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Core.Extensions;
using Core.Utilities.Results;
using Business.Constants;

namespace Business.BusinessAspects.Autofac
{
    public class SecuredOperation : MethodInterception
    {
        string[] roles;
        IHttpContextAccessor httpContextAccessor;
        public SecuredOperation(string roles)
        {
            this.roles = roles.Split(',');
            httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        protected override void OnBefore(IInvocation invocation)
        {
            var userRoles = httpContextAccessor.HttpContext.User.ClaimRoles();
            foreach (var role in userRoles)
            {
                foreach (var requiredRole in this.roles)
                {
                    if(requiredRole == role)
                    {
                        return;
                    }
                }
            }
            invocation.ReturnValue = new ErrorResult(Messages.PermissionDenied);
            throw new Exception(Messages.PermissionDenied);
        }
    }
}
