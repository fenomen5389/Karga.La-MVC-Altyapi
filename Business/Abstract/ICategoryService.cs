using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        IResult GetAll();
        IResult GetById(int categoryId);
        IResult Add(Category category);
        IResult Delete(int categoryId);
        IResult DeleteAll();
        IResult Update(Category category);
    }
}
