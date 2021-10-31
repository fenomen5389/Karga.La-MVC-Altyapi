using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Post:IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime AddedDate { get; set; }
        public string AuthorId { get; set; }
        public string CategoryId { get; set; }
        public bool Published { get; set; }
        public string FocusKeyword { get; set; }
        public string MetaDescription { get; set; }
        public string Keywords { get; set; }
        public bool CornerStone { get; set; }
        public bool IndexPage { get; set; }
        public bool FollowLinks { get; set; }
        public string Thumbnail { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
    }
}
