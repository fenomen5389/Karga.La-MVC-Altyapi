using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Business
{
    public static class BusinessRules
    {
        public static IResult Run(params IResult[] rules)
        {
            foreach (var item in rules)
            {
                if (!item.Success)
                {
                    return item;
                }
            }
            return new SuccessResult("İş kuralları ana çerçevesi onaylaması başarılı.");
        }
    }
}
