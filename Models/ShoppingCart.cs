using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanyPieShop.Models
{
    public class ShoppingCart
    {
        private readonly AppDbContext _appDbContext;

        public string ShoppingCartId { get; set; }
       
        private ShoppingCart(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        /**
         * Because that is not a controller class we need a special way to access services like the session in HttpContext:
         * 
         *we need to access services collection that is being managed with dependency injection container.
         *To get the session we need to access the HttpContext(request) then access the session in it.
         *we ask the services collection for a dbContext 
         *check if there is a CartID or create one.
         *populate the session["CartId"] with the cartId
         *create instance of ShoppingCart and populate it with the shopping id.
         *return the created instance with the dbContext parameter
         */
        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            var context = services.GetService<AppDbContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);
            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }
        /**
         * check if the given pie is already a shoppingCarItem then increase amount by one,
         * otherwise create a shopping cart item
         */
        public void AddToCart(Pie pie, int amount)
        {
            var shoppingCartItem = _appDbContext.ShoppingCartItems.FirstOrDefault(
                    s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId);
            if(shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    Pie = pie,
                    ShoppingCartId = ShoppingCartId,
                    Amount = 1
                };
                _appDbContext.ShoppingCartItems.Add(shoppingCartItem);
                
            }
            else
            {
                shoppingCartItem.Amount++;
                
            }
            _appDbContext.SaveChanges();
        }
        public int RemoveFromCart(Pie pie)
        {
            var shoppingCartItem =
                 _appDbContext.ShoppingCartItems.FirstOrDefault(
                     s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId);
            int localAmount = 0;
            if(shoppingCartItem != null)
            {
                if(shoppingCartItem.Amount > 1)
                { 
                    localAmount = --shoppingCartItem.Amount;    
                }
                else
                {
                    _appDbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }    
            }
            _appDbContext.SaveChanges();
            return localAmount;
        }
        public void ClearCart()
        {
            var cartItems = _appDbContext
                .ShoppingCartItems
                .Where(c => c.ShoppingCartId == ShoppingCartId);
            _appDbContext.ShoppingCartItems.RemoveRange(cartItems);
            _appDbContext.SaveChanges();
        }
        //return the total price of the cart
        public decimal GetShoppingCartTotal()
        {
            var total = _appDbContext.ShoppingCartItems
                .Where(s => s.ShoppingCartId == ShoppingCartId)
                .Select(s => s.Amount * s.Pie.Price).Sum();
            return total;      
        }
        /*
         * Populate the current shopping cart from the database. If it is populated return the items
         */
        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            var items = _appDbContext.ShoppingCartItems
                .Where(s => s.ShoppingCartId == ShoppingCartId)
                .Include(s => s.Pie).ToList();
            return items;
                
        }




    }
}
