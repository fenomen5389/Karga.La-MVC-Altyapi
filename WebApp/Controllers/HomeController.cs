using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        IChatMessageService _chatMessageService;
        IPostService _postService;
        IAuthorService _authorService;
        public HomeController(IChatMessageService chatMessageService,IPostService postService,IAuthorService authorService)
        {
            _chatMessageService = chatMessageService;
            _postService = postService;
            _authorService = authorService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{postTitle}/{postId}")]
        public IActionResult ShowPost(string postId)
        {
            ViewBag.PostId = int.Parse(postId);
            return View();
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string? q,[FromQuery] string? author, [FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            var result = new List<Post>() { };
            List<Post> finalResult = new List<Post>();
            if(q != null)
            {
                var search = (IDataResult<List<Post>>)_postService.Search(q);
                result = search.Data;
            }
            else
            {
                var all = (IDataResult<List<Post>>)_postService.GetAll();
                result = all.Data;
            }
            finalResult = result;
            if(author != null)
            {
                finalResult = finalResult.Where(p => p.AuthorId == author).ToList();
            }
            if(start != null)
            {
                finalResult = finalResult.Where(p => p.AddedDate > start).ToList();
            }
            if(end != null)
            {
                finalResult = finalResult.Where(p => p.AddedDate < end).ToList();
            }
            finalResult = finalResult.OrderByDescending(p => p.AddedDate).ToList();
            ViewBag.Data = finalResult;
            ViewBag.Query = q;
            return View();
        }

        [HttpGet("{authorId}")]
        public IActionResult ShowAuthor(string authorId,[FromQuery] int from = 0, [FromQuery] int count = 10)
        {
            var postResult = (IDataResult<List<Post>>)_postService.GetByAuthor(authorId,from,count);
            var authorResult = (IDataResult<Author>)_authorService.GetById(authorId);
            ViewBag.Data = postResult.Data;
            ViewBag.Author = authorResult.Data;
            return View();
        }

        [HttpGet("mobile/chat")]
        public IActionResult MobileChat([FromQuery] string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl == null ? "/" : returnUrl;
            return View();
        }

        [HttpGet("category/{urlName}.{id}")]
        public IActionResult Category(int id)
        {
            return Ok(id);
        }
    }
}
