using Microsoft.AspNetCore.Mvc;
using MyShoppingStore.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyShoppingStore.Infrastructure
{
    public class ButtonCartViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            ButtonCartViewModel buttonCartVM;

            if (cart == null || cart.Count == 0)
            {
                buttonCartVM = null;
            }
            else
            {
                buttonCartVM = new ButtonCartViewModel
                {
                    NumberOfItems = cart.Sum(x => x.Quantity),
                    FinalAmount = cart.Sum(x => x.Quantity * x.Price)
                };
            }
            return View(buttonCartVM);
        }
    }
}
