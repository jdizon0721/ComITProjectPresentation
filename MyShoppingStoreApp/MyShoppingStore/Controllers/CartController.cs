using Microsoft.AspNetCore.Mvc;
using MyShoppingStore.Infrastructure;
using MyShoppingStore.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyShoppingStore.Controllers
{
    public class CartController : Controller
    {
        private readonly MyShoppingStoreContext context;

        public CartController(MyShoppingStoreContext context)
        {
            this.context = context;
        }

        //Get /Cart
        public IActionResult Index()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            CartViewModel cartVM = new CartViewModel
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Price * x.Quantity)
            };

            return View(cartVM);
        }
    }
}
