using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Core.Aspects.Autofac.Caching
{
    public class CacheAspect:MethodInterception
    {
        public override void Intercept(IInvocation invocation)
        {
            var cacheManager = ServiceTool.ServiceProvider.GetService<IMemoryCache>();
            var methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}");
            var arguments = invocation.Arguments.ToList();
            var key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})";
            if (cacheManager.TryGetValue(key, out _))
            {
                invocation.ReturnValue = cacheManager.Get(key);
                Debug.WriteLine("Loaded from cache!");
                return;
            }
            invocation.Proceed();
            cacheManager.Set(key,invocation.ReturnValue);
        }
    }
}
