using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.Validation.FluentValidation;
using Core.Aspects.Autofac.ErrorHandling;
using Core.Aspects.Autofac.Validating;
using Core.Entities.Concrete;
using Core.Utilities.Business;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class AuthorManager:IAuthorService
    {
        IAuthorDal _authorDal;
        IHttpContextAccessor _httpContextAccessor;
        ITokenHelper _tokenHelper;
        public AuthorManager(IAuthorDal authorDal, IHttpContextAccessor httpContextAccessor,ITokenHelper tokenHelper)
        {
            _authorDal = authorDal;
            _httpContextAccessor = httpContextAccessor;
            _tokenHelper = tokenHelper;
        }

        public IResult GetAll()
        {
            var result = _authorDal.GetAll();
            var businessResult = BusinessRules.Run(CheckResultLengthZero(result));
            if (businessResult.Success)
            {
                return new SuccessDataResult<List<Author>>(result,Messages.Ok);
            }
            return new ErrorResult(businessResult.Message);
        }

        public IResult CheckResultLengthZero(List<Author> authors)
        {
            if(authors.Count == 0)
            {
                return new ErrorResult(Messages.ListEmpty);
            }
            return new SuccessResult(Messages.Ok);
        }

        public IResult CheckResultNull(object result)
        {
            if(result == null)
            {
                return new ErrorResult(Messages.Null);
            }
            return new SuccessResult(Messages.Ok);
        }

        public Author GetByMail(string mail)
        {
            var result = _authorDal.Get(p => p.Mail == mail);
            return result;
        }

        public List<OperationClaim> GetClaims(Author author)
        {
            var result = _authorDal.GetClaims(author);
            return result;
        }

        [SecuredOperation("Admin")]
        [ErrorHandleAspect]
        [ValidationAspect(typeof(AuthorAddDtoValidator))]
        public IResult Add(AuthorAddDto authorAddDto)
        {
            var author = new Author();
            var passwordHash = new byte[] { };
            var passwordSalt = new byte[] { };
            HashingHelper.CreatePasswordHash(authorAddDto.Password, out passwordHash, out passwordSalt);

            author.Id = authorAddDto.Id;
            author.PasswordSalt = passwordSalt;
            author.PasswordHash = passwordHash;
            author.FirstName = authorAddDto.FirstName;
            author.LastName = authorAddDto.LastName;
            author.Biography = authorAddDto.Biography;
            author.Mail = authorAddDto.Mail;
            author.VisibleName = authorAddDto.VisibleName;
            
            _authorDal.Add(author);
            return new SuccessResult(Messages.Ok);
        }

        [SecuredOperation("Admin")]
        [ErrorHandleAspect]
        public IResult Delete(string authorId)
        {
            _authorDal.Delete(_authorDal.Get(p => p.Id == authorId));
            return new SuccessResult(Messages.Ok);
        }

        [SecuredOperation("Admin")]
        [ErrorHandleAspect]
        [ValidationAspect(typeof(AuthorValidator))]
        public IResult Update(Author author)
        {
            var authorToUpdate = _authorDal.Get(p => p.Id == author.Id);
            authorToUpdate.FirstName = author.FirstName;
            authorToUpdate.LastName = author.LastName;
            authorToUpdate.Mail = author.Mail;
            _authorDal.Update(authorToUpdate);
            return new SuccessResult(Messages.Ok);
        }

        public AccessToken CreateToken(Author author)
        {
            var claims = _authorDal.GetClaims(author);
            return _tokenHelper.CreateToken(author, claims);
        }

        [ErrorHandleAspect]
        public IResult GetById(string authorId)
        {
            var result = _authorDal.Get(p => p.Id == authorId);
            return new SuccessDataResult<Author>(result);
        }
    }
}
