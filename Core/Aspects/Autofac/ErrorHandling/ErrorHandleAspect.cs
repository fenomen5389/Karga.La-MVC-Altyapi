using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Aspects.Autofac.ErrorHandling
{
    public class ErrorHandleAspect:MethodInterception
    {
        public override void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch(Exception e)
            {
                invocation.ReturnValue = new ErrorResult($"Sunucu özel hata döndürdü: {e.Message}");
                return;
            }
        }
    }
}
