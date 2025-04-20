using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class Delivery
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        public DateTime? DeliveryDate { get; set; }
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}