namespace CA.Core.Application.Contracts.Features.Post.Queries.GetPostById
{
    public class GetPostByIdQueryViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Slug { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
    }
}
