using MongoWeb.Models;
using MongoWeb.Repositores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoWeb.Services
{
    public class Cart
    {
        private readonly ITodoRepository _todoRepository;

        public Cart(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
        }

        public List<CartItem> GetCartItems()
        {
            if (HttpContext.Current.Session["Cart"] == null)
            {
                HttpContext.Current.Session["Cart"] = new List<CartItem>();
            }
            return (List<CartItem>)HttpContext.Current.Session["Cart"];
        }

        public void AddToCart(string productId, int quantity)
        {
            var product = _todoRepository.GetById(productId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            var cartItems = GetCartItems();
            var cartItem = cartItems.FirstOrDefault(item => item.Product.ProductId == productId);

            if (cartItem == null)
            {
                cartItems.Add(new CartItem
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            HttpContext.Current.Session["Cart"] = cartItems;
        }

        public void RemoveFromCart(string productId)
        {
            var cartItems = GetCartItems();
            var cartItem = cartItems.FirstOrDefault(item => item.Product.ProductId == productId);

            if (cartItem != null)
            {
                cartItems.Remove(cartItem);
                HttpContext.Current.Session["Cart"] = cartItems;
            }
        }

        public void ClearCart()
        {
            HttpContext.Current.Session["Cart"] = new List<CartItem>();
        }

        public void PlaceOrder(string email, string shippingAddress, string paymentMethod)
        {
            var cartItems = GetCartItems();
            if (!cartItems.Any())
            {
                throw new Exception("Giỏ hàng trống.");
            }

            decimal totalAmount = cartItems.Sum(item => item.Product.Price * item.Quantity);

            var order = new Order
            {
                OrderId = Guid.NewGuid().ToString(),
                Email = email,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Items = cartItems.Select(item => new OrderItem
                {
                    ProductId = item.Product.ProductId, // Đảm bảo ProductId đúng thuộc tính của Product
                    Quantity = item.Quantity
                }).ToList(),
                ShippingAddress = shippingAddress,
                PaymentMethod = paymentMethod,
                Status = "Pending"
            };

            _todoRepository.PlaceOrder(order);
            ClearCart(); // Xóa giỏ hàng sau khi đặt hàng
        }
    }
}
