using BethanyPieShop.Models;
using BethanyPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanyPieShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartController(IPieRepository pieRepository,ShoppingCart shoppingCart)
        {
            _pieRepository = pieRepository;
            _shoppingCart = shoppingCart;
        }

        public ViewResult Index()
        {
            //just populating shoppingCartItems List in the _shoppingCart before passing it to the view model.
             _shoppingCart.GetShoppingCartItems();

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()

            };
            return View(shoppingCartViewModel);
        }
        public RedirectToActionResult AddToShoppingCart(int pieId)
        {
            var pie = _pieRepository.GetPieById(pieId);
            if(pie != null)
            {
                _shoppingCart.AddToCart(pie, 1);
               
            }
            return RedirectToAction("Index");
        }
        public RedirectToActionResult RemoveFromShoppingCart(int pieId)
        {
            var pie = _pieRepository.GetPieById(pieId);
            if (pie != null)
            {
                _shoppingCart.RemoveFromCart(pie);

            }
            return RedirectToAction("Index");
        }
    }
}
