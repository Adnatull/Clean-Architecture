using System.Collections.Generic;
using Core.Domain.Persistence.Common;

namespace Core.Domain.Persistence.Entities
{
    public class Post : BaseEntity
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }

        public string Slug { get; set; }
        public string Summary { get; set; }

        public string Content { get; set; }
        public int PostViews { get; set; }
        public bool IsPublished { get; set; }
        public virtual Post ParentPost { get; set; }
        public virtual ICollection<Post> ChildPosts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<PostCategory> Categories { get; set; }
        public virtual ICollection<PostTag> Tags { get; set; }
    }
}
