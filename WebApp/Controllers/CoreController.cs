using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WebApp.Models;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoreController : ControllerBase
    {
        ICategoryService _categoryService;
        IAuthorService _authorService;
        IPostService _postService;
        IAuthService _authService;
        IChatMessageService _chatMessageService;
        public CoreController(ICategoryService categoryService,
            IAuthorService authorService,
            IPostService postService,
            IAuthService authService,
            IChatMessageService chatMessageService)
        {
            _categoryService = categoryService;
            _authorService = authorService;
            _postService = postService;
            _authService = authService;
            _chatMessageService = chatMessageService;
        }

        [HttpGet("Authors/GetAll")]
        public IActionResult GetAuthors()
        {
            var result = _authorService.GetAll();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Authors/Add")]
        public IActionResult AddAuthor(AuthorAddDto authorAddDto)
        {
            var result = _authorService.Add(authorAddDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("Authors/Delete")]
        public IActionResult DeleteAuthor([FromQuery] string id)
        {
            var result = _authorService.Delete(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Authors/Update")]
        public IActionResult UpdateAuthor(Author author)
        {
            var result = _authorService.Update(author);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("Categories/GetAll")]
        public IActionResult GetCategories()
        {
            var result = _categoryService.GetAll();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Categories/Add")]
        public IActionResult AddCategory(Category category)
        {
            var result = _categoryService.Add(category);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Categories/Update")]
        public IActionResult UpdateCategory(Category category)
        {
            var result = _categoryService.Update(category);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("Categories/Delete")]
        public IActionResult DeleteCategory([FromQuery] int id)
        {
            var result = _categoryService.Delete(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("Categories/DeleteAll")]
        public IActionResult DeleteAll()
        {
            var result = _categoryService.DeleteAll();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Posts/Save")]
        public IActionResult SavePost(Post post)
        {
            var result = _postService.Add(post);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Posts/Update")]
        public IActionResult UpdatePost(Post post)
        {
            var result = _postService.Update(post);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("Posts/GetById")]
        public IActionResult GetPostById([FromQuery] int id)
        {
            var result = _postService.GetById(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("Posts/Like")]
        public IActionResult LikePost([FromQuery] int id)
        {
            var result = _postService.Like(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("Posts/Dislike")]
        public IActionResult DislikePost([FromQuery] int id)
        {
            var result = _postService.Dislike(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("Posts/GetAll")]
        public IActionResult GetAllPosts()
        {
            var result = _postService.GetAll();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("Library/GetPhotos")]
        public IActionResult GetPhotos()
        {
            var totalFiles = new List<LibraryImage>();
            var directoryInfo = new DirectoryInfo("wwwroot/uploads/");
            var years = directoryInfo.GetDirectories();
            foreach (var year in years)
            {
                var months = new DirectoryInfo($"wwwroot/uploads/{year.Name}/").GetDirectories();
                foreach (var month in months)
                {
                    var images = new DirectoryInfo($"wwwroot/uploads/{year.Name}/{month.Name}").GetFiles();
                    foreach (var image in images)
                    {
                        totalFiles.Add(new LibraryImage() { Year = int.Parse(year.Name), Month = int.Parse(month.Name), Path = image.FullName });
                    }
                }
            }
            return Ok(new SuccessDataResult<List<LibraryImage>>(totalFiles));
        }

        [HttpPost("FakeWordPressRequest")]
        public IActionResult FakeWordPressRequest_Post([FromForm] object data)
        {
            return Ok();
        }

        [HttpGet("FakeWordPressRequest")]
        public IActionResult FakeWordPressRequest_Get([FromForm] object data)
        {
            return Ok();
        }

        [HttpPost("WpSystem/wp/v2/media")]
        public IActionResult WordPressMedia([FromForm] WordPressUpload wordPressUpload)
        {
            var file = wordPressUpload.File;
            var path = $"uploads/{DateTime.Now.Year}/{DateTime.Now.Month}/{new Random().Next(111111, 999999)}_{file.FileName}";
            using (var stream = new FileStream($"wwwroot/{path}",FileMode.OpenOrCreate))
            {
                file.CopyTo(stream);
            }
            using (var image = Image.Load($"wwwroot/{path}"))
            {
                if(image.Width > 966 && image.Height > 544)
                    image.Clone(c => c.Crop(966, 544)).SaveAsPng($"wwwroot/{path}_slider");
            }
            return Ok(new { Id=1, Date=DateTime.Now, Mime_Type=file.ContentType, Media_Details= new{  },Source_Url = $"https://localhost:44302/api/Core/Library/ShowFile?path=wwwroot/{path}" });
        }

        [HttpPost("WpSystem/KargaAjax")]
        public IActionResult KargaAjax([FromQuery] int? year,[FromQuery] int? monthnum, [FromForm] WordPressAjaxRequest wordPressAjaxRequest)
        {
            if(wordPressAjaxRequest.Action == "query-attachments")
            {
                var files = GetFiles(year, monthnum);
                List<WordPressMediaModel> wordPressMedias = new List<WordPressMediaModel>();
                files.ForEach(f =>
                {
                    string fileName = f.Path.Split("/")[f.Path.Split("/").Length - 1];
                    wordPressMedias.Add(new CoreController.WordPressMediaModel() { Url = $"https://localhost:44302/api/Core/Library/ShowFile?path=wwwroot/{f.Path}", Title = fileName, FileName=fileName });
                });
                return Ok(new SuccessDataResult<List<WordPressMediaModel>>(wordPressMedias));
            }
            return Ok();
        }

        class WordPressMediaModel
        {
            public int Id { get
                {
                    return new Random().Next(11111, 99999);
                }
            }
            public string Title { get; set; }
            public string FileName { get; set; }
            public string Url { get; set; }
        }

        public List<LibraryImage> GetFiles(int? filterYear = null, int? filterMonth = null)
        {
            var totalFiles = new List<LibraryImage>();
            var directoryInfo = new DirectoryInfo("wwwroot/uploads/");
            var years = directoryInfo.GetDirectories();
            foreach (var year in years)
            {
                if (filterYear != null && filterYear != int.Parse(year.Name)) continue;
                var months = new DirectoryInfo($"wwwroot/uploads/{year.Name}/").GetDirectories();
                foreach (var month in months)
                {
                    if (filterMonth != null && filterMonth != int.Parse(month.Name)) continue;
                    var images = new DirectoryInfo($"wwwroot/uploads/{year.Name}/{month.Name}").GetFiles();
                    foreach (var image in images)
                    {
                        totalFiles.Add(new LibraryImage() { Year = int.Parse(year.Name), Month = int.Parse(month.Name), Path = image.FullName });
                    }
                }
            }
            return totalFiles;
        }

        [HttpGet("Library/ShowFile")]
        public IActionResult ShowFile(string path)
        {
            var imageBytes = new byte[] { };
            using (FileStream fileStream = new FileStream(path,FileMode.Open))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    fileStream.CopyTo(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }
            }
            return File(imageBytes, "image/*");
        }

        [HttpGet("Library/SelfCheck")]
        public IActionResult SelfCheck()
        {
            Dictionary<string, byte[]> fileMap = new Dictionary<string, byte[]>();

            string uploadDir = "wwwroot/uploads";
            DirectoryInfo directoryInfo = new DirectoryInfo(uploadDir);
            var files = directoryInfo.GetFiles();
            foreach (var file in files)
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = System.IO.File.OpenRead(file.FullName))
                    {
                        var hash = md5.ComputeHash(stream);
                        foreach(KeyValuePair<string,byte[]> pair in fileMap)
                        {
                            if(pair.Value.SequenceEqual(hash))
                            {
                                Debug.WriteLine($"Same file detected! | {pair.Key}");
                            }
                        }
                        fileMap.Add(file.Name, hash);
                    }
                }
            }
            return Ok("System checkup complete! All same files deleted.(Unused variations)");
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginDto loginDto)
        {
            var result = _authService.Login(loginDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
