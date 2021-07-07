using BethanyPieShop.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanyPieShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ShoppingCart _shoppingCart;

        public OrderController(IOrderRepository orderRepository,ShoppingCart shoppingCart)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
        }
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            List<ShoppingCartItem> shoppingCartItems = _shoppingCart.GetShoppingCartItems();
            if(shoppingCartItems.Count < 0)
            {
                ModelState.AddModelError("", "Your cart is empty. Go add some pies first!");
            }
            if(ModelState.IsValid)
            {
                _orderRepository.CreateOrder(order);
                _shoppingCart.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }
            return View(order);
        }
        public ViewResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Thanks for your order. Your will enjoy your pies soon!";
            return View();
        }
    }
}
