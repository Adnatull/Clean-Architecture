using Core.Domain.Persistence.Common;

namespace Core.Domain.Persistence.Entities
{
    public class Comment : BaseEntity
    {
        public long Id { get; set; }
        public int PostId { get; set; }
        public long? ParentId { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public string AuthorIp { get; set; }
        public string Content { get; set; }
        public bool IsApproved { get; set; }

        public virtual Post Post { get; set; }

    }
}
