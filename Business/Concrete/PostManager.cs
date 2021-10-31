using Business.Abstract;
using Business.Constants;
using Business.Validation.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.ErrorHandling;
using Core.Aspects.Autofac.Validating;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Core.Extensions;
using Microsoft.AspNetCore.Http;
using Core.Utilities.Business;

namespace Business.Concrete
{
    public class PostManager:IPostService
    {
        IPostDal _postDal;
        IHttpContextAccessor _httpContextAccessor;
        public PostManager(IPostDal postDal,IHttpContextAccessor httpContextAccessor)
        {
            _postDal = postDal;
            _httpContextAccessor = httpContextAccessor;
        }

        [ErrorHandleAspect]
        [ValidationAspect(typeof(PostValidator))]
        public IResult Add(Post post)
        {
            _postDal.Add(post);
            return new SuccessDataResult<Post>(post);
        }

        [ErrorHandleAspect]
        public IResult Dislike(int postId)
        {
            var businessResult = BusinessRules.Run(CheckAlreadyVoted(postId));
            if (!businessResult.Success)
            {
                return new ErrorResult(businessResult.Message);
            }
            var postToAffect = _postDal.Get(p => p.Id == postId);
            postToAffect.DislikeCount++;
            _postDal.Update(postToAffect);
            _httpContextAccessor.HttpContext.Response.Cookies.Append($"react_{postId}","disliked");
            return new SuccessResult(Messages.Ok);
        }

        public IResult CheckAlreadyVoted(int postId)
        {
            try { 
                var cookie = _httpContextAccessor.HttpContext.Request.Cookies[$"react_{postId}"];
                if(cookie != null)
                {
                    return new ErrorResult("Zaten oylanmış.");
                }
                return new SuccessResult(Messages.Ok);
            }
            catch
            {
                return new SuccessResult(Messages.Ok);
            }
        }

        [CacheAspect]
        public IResult GetAll()
        {
            var result = _postDal.GetAll();
            return new SuccessDataResult<List<Post>>(result, Messages.Ok);
        }

        public IResult GetByAuthor(string authorId,int from,int count)
        {
            var result = _postDal.GetAll(p => p.AuthorId == authorId).Skip(from).Take(count).ToList();
            return new SuccessDataResult<List<Post>>(result, Messages.Ok);
        }

        public IResult GetById(int id)
        {
            var result = _postDal.Get(p => p.Id == id);
            return new SuccessDataResult<Post>(result);
        }

        public IResult GetLatests(int count)
        {
            var all = (IDataResult<List<Post>>)GetAll();
            var result = all.Data.OrderByDescending(p => p.AddedDate).Take(count).ToList();
            return new SuccessDataResult<List<Post>>(result);
        }

        [ErrorHandleAspect]
        public IResult Like(int postId)
        {
            var businessResult = BusinessRules.Run(CheckAlreadyVoted(postId));
            if (!businessResult.Success)
            {
                return new ErrorResult(businessResult.Message);
            }
            var postToAffect = _postDal.Get(p => p.Id == postId);
            postToAffect.LikeCount++;
            _postDal.Update(postToAffect);
            _httpContextAccessor.HttpContext.Response.Cookies.Append($"react_{postId}", "liked");
            return new SuccessResult(Messages.Ok);
        }

        public IResult Search(string q)
        {
            var all = (IDataResult<List<Post>>)GetAll();
            var result = all.Data.Where(p => p.Title.ToLower().ToEnglish().Contains(q.ToLower().Trim().ToEnglish()) || q.ToLower().Contains(p.FocusKeyword.ToLower())).ToList().OrderByDescending(p => p.AddedDate).ToList();
            return new SuccessDataResult<List<Post>>(result);
        }

        public IResult SkipTake(int skip, int take)
        {
            var all = (IDataResult<List<Post>>)GetAll();
            var result = all.Data.OrderByDescending(p => p.AddedDate).Skip(skip).Take(take).ToList();
            return new SuccessDataResult<List<Post>>(result);
        }

        [ErrorHandleAspect]
        [ValidationAspect(typeof(PostValidator))]
        public IResult Update(Post post)
        {
            _postDal.Update(post);
            return new SuccessResult(Messages.Ok);
        }
    }
}
