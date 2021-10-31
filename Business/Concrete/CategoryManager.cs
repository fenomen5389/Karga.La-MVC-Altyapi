using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.Validation.FluentValidation;
using Core.Aspects.Autofac.ErrorHandling;
using Core.Aspects.Autofac.Validating;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class CategoryManager:ICategoryService
    {
        ICategoryDal _categoryDal;
        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public IResult GetAll()
        {
            var result = _categoryDal.GetAll();
            var businessResult = BusinessRules.Run(CheckResultLengthZero(result));
            if (businessResult.Success)
            {
                return new SuccessDataResult<List<Category>>(result,Messages.Ok);
            }
            return new ErrorResult(businessResult.Message);
        }

        public IResult CheckResultLengthZero(List<Category> categories)
        {
            if (categories.Count == 0)
            {
                return new ErrorResult(Messages.ListEmpty);
            }
            return new SuccessResult(Messages.Ok);
        }

        [SecuredOperation("Admin")]
        [ValidationAspect(typeof(CategoryValidator))]
        [ErrorHandleAspect]
        public IResult Add(Category category)
        {
            if(category.ParentCategoryId == -1)
            {
                category.ParentCategoryId = null;
            }
            _categoryDal.Add(category);
            return new SuccessResult(Messages.Ok);
        }

        [SecuredOperation("Admin")]
        [ErrorHandleAspect]
        public IResult Delete(int categoryId)
        {
            Category category = _categoryDal.Get(p => p.Id == categoryId);
            _categoryDal.Delete(category);
            return new SuccessResult(Messages.Ok);
        }

        [SecuredOperation("Admin")]
        [ErrorHandleAspect]
        public IResult DeleteAll()
        {
            var categories = _categoryDal.GetAll();
            categories.ForEach(p =>
            {
                _categoryDal.Delete(p);
            });
            return new SuccessResult(Messages.Ok);
        }

        [ErrorHandleAspect]
        public IResult Update(Category category)
        {
            var businessResult = BusinessRules.Run(CheckCategoryUpdateValid(category));
            if (businessResult.Success)
            {
                _categoryDal.Update(category);
                return new SuccessResult(Messages.Ok);
            }
            return new ErrorResult(businessResult.Message);
        }

        public IResult CheckCategoryUpdateValid(Category category)
        {
            if (category.ParentCategoryId == category.Id)
            {
                return new ErrorResult(Messages.CategoryParentCannotBeItself);
            }
            return new SuccessResult();
        }

        [ErrorHandleAspect]
        public IResult GetById(int categoryId)
        {
            var result = _categoryDal.Get(p => p.Id == categoryId);
            return new SuccessDataResult<Category>(result);
        }
    }
}
