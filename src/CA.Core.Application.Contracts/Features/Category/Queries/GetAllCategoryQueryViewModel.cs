using System.Collections.Generic;

namespace CA.Core.Application.Contracts.Features.Category.Queries
{
    public class GetAllCategoryQueryViewModel
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
    }
}
