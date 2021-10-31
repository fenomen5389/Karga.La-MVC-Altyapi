using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IPostService
    {
        IResult GetAll();
        IResult GetByAuthor(string authorId, int from, int count);
        IResult Add(Post post);
        IResult GetLatests(int count);
        IResult SkipTake(int skip, int take);
        IResult GetById(int id);
        IResult Update(Post post);
        IResult Search(string q);
        IResult Like(int postId);
        IResult Dislike(int postId);
    }
}
