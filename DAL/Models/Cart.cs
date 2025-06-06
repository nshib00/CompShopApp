﻿using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("CustomerId")]
        public virtual User Customer { get; set; }

        public virtual ICollection<CartItem> Items { get; set; }
    }
}