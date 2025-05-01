using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Manufacturer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
