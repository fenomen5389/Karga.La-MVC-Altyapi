using Core.DataAccess.Concrete.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfAuthorDal : EfEntityRepositoryBase<Author, DatabaseContext>, IAuthorDal
    {
        public List<OperationClaim> GetClaims(Author author)
        {
            using (DatabaseContext databaseContext = new DatabaseContext())
            {
                var result = from operationClaim in databaseContext.OperationClaims
                             join userOperationClaim in databaseContext.AuthorOperationClaims
                                 on operationClaim.Id equals userOperationClaim.OperationClaimId
                             where userOperationClaim.UserId == author.Id
                             select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };
                return result.ToList();
            }
        }
    }
}
