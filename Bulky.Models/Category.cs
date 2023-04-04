using System.ComponentModel.DataAnnotations;

namespace Bulky.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        [Required]
        [Range(1, 100, ErrorMessage = "The field DisplayOrder must be between 1 and 100.")]
        public int DisplayOrder { get; set; }
    }
}
