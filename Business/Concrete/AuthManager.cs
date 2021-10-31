using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Entities.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        IAuthorService _authorService;
        IHttpContextAccessor _httpContextAccessor;
        public AuthManager(IAuthorService authorService,IHttpContextAccessor httpContextAccessor)
        {
            _authorService = authorService;
            _httpContextAccessor = httpContextAccessor;
        }
        public IResult Login(LoginDto loginDto)
        {
            var userToLogin = _authorService.GetByMail(loginDto.Mail);
            if(userToLogin == null)
            {
                return new ErrorDataResult<Author>(Messages.Null);
            }

            if (HashingHelper.VerifyPasswordHash(userToLogin.PasswordHash, userToLogin.PasswordSalt, loginDto.Password))
            {
                var token = _authorService.CreateToken(userToLogin);
                return new SuccessDataResult<dynamic>(new { User=userToLogin, Token=token },Messages.Ok);
            }
            return new ErrorDataResult<Author>(Messages.WrongCredentials);
        }
    }
}
