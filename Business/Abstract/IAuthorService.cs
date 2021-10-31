using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IAuthorService
    {
        Author GetByMail(string mail);
        List<OperationClaim> GetClaims(Author author);
        IResult GetAll();
        IResult Add(AuthorAddDto authorAddDto);
        IResult Delete(string authorId);
        IResult Update(Author author);
        IResult GetById(string authorId);
        AccessToken CreateToken(Author author);
    }
}
