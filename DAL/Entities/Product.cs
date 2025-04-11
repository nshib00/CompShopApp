namespace DAL
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int? CartId { get; set; }
        public int? OrderDetailId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual OrderDetail OrderDetail { get; set; }
    }
}
