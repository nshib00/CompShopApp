using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class OrderStatus
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}