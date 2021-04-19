using System.ComponentModel.DataAnnotations;
using CA.Core.Application.Contracts.Response;
using CA.Core.Application.Contracts.ValidationAttributes;
using MediatR;

namespace CA.Core.Application.Contracts.Handlers.Category.Commands
{
    public class AddCategoryCommand : IRequest<Response<int>>
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [SlugValidate]
        [MaxLength(20, ErrorMessage = "Too Long")]
        public string Slug { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
