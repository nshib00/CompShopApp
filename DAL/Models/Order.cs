using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Order
    {
        public int Id { get; set; }

        public decimal DeliveryCost { get; set; }

        public DateTime OrderDate { get; set; }
        public int StatusId { get; set; }
        public int CustomerId { get; set; }

        [ForeignKey("StatusId")]
        public virtual OrderStatus Status { get; set; }

        [ForeignKey("CustomerId")]
        public virtual User Customer { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual Delivery Delivery { get; set; }
    }
}