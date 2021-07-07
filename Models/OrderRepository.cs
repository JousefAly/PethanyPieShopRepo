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
            _appDbContext.Orders.Add(order);
            var shoppingCartItems = _shoppingCart.GetShoppingCartItems();
            foreach (var s in shoppingCartItems)
            {
                var orderDetail = new OrderDetail()
                {
                    OrderId = order.OrderId,
                    PieId = s.Pie.PieId,
                    Amount = s.Amount,
                    Price = s.Pie.Price
                };
                _appDbContext.OrderDetails.Add(orderDetail);

            }
            _appDbContext.SaveChanges();
        }
    }
}
