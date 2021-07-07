using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanyPieShop.Models
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly ShoppingCart _shoppingCart;

        public OrderRepository(AppDbContext appDbContext, ShoppingCart shoppingCart)
        {
            _appDbContext = appDbContext;
            _shoppingCart = shoppingCart;
        }
        public void CreateOrder(Order order)
        {
            order.OrderPlaced = DateTime.Now;
            order.OrderTotal = _shoppingCart.GetShoppingCartTotal();
            var shoppingCartItems = _shoppingCart.GetShoppingCartItems();
            order.OrderDetails = new List<OrderDetail>();
            foreach (var s in shoppingCartItems)
            {
                var orderDetail = new OrderDetail()
                {
                    Amount = s.Amount,
                    PieId = s.Pie.PieId,
                    Price = s.Pie.Price
                };
                order.OrderDetails.Add(orderDetail);
            }

            _appDbContext.Orders.Add(order);
            _appDbContext.SaveChanges();
        }
    }
}
