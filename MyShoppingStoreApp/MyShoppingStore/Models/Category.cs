using System.ComponentModel.DataAnnotations;

namespace MyShoppingStore.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, MinLength(2, ErrorMessage = "Minimum Length is 2")]
        [RegularExpression(@"^[a-zA-Z- ]+$", ErrorMessage = "Please use letter only")]
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Sorting { get; set; }
    }
}
