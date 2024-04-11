using System.ComponentModel.DataAnnotations;

namespace Reddit.Dtos
{
    public class CreatePostDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "The title must be at least 3 characters long.")]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public int AuthorId { get; set; }
    }
}
