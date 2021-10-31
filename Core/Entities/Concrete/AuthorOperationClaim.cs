using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Concrete
{
    public class AuthorOperationClaim:IEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int OperationClaimId { get; set; }
    }
}
