using System.Collections.Generic;

namespace MyShoppingStore.Models
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
