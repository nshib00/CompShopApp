using System.Collections.Generic;

namespace DAL
{
    public class Cart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        public virtual User Customer { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
