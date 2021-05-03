namespace Core.Application.Contracts.HandlerExchanges.Post.Queries
{
    public class GetAllPostQueryResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Slug { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
    }
}
