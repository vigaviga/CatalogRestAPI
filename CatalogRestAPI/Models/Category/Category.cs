using System.ComponentModel.DataAnnotations;

namespace CatalogRestAPI.Models.Category
{
    public class Category
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
