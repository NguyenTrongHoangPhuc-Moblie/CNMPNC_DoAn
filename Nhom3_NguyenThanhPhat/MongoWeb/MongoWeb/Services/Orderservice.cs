//using MongoDB.Bson;
//using MongoWeb.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace MongoWeb.Services
//{
//    public class Orderservice
//    {
//        private readonly ITodoRepository _repository;

//        public Orderservice(ITodoRepository repository)
//        {
//            _repository = repository;
//        }

//        public void PlaceOrder(string userId, List<CartItem> cartItems, string shippingAddress, string paymentMethod)
//        {
//            if (cartItems == null || !cartItems.Any())
//            {
//                throw new ArgumentException("Cart is empty.");
//            }

//            // Tính toán tổng số tiền đơn hàng
//            var totalAmount = cartItems.Sum(item => item.Quantity * item.Product.Price);

//            // Tạo một đơn hàng mới
//            var order = new Order
//            {
//                OrderId = Guid.NewGuid().ToString(),
//                CustomerName = GetUserNameById(userId), // Lấy tên người dùng từ userId
//                OrderDate = DateTime.UtcNow,
//                TotalAmount = totalAmount,
//                Items = cartItems.Select(item => new OrderItem
//                {
//                    ProductId = item.Product.ProductId,
//                    Quantity = item.Quantity
//                }).ToList(),
//                ShippingAddress = shippingAddress,
//                PaymentMethod = paymentMethod,
//                Status = "Pending"
//            };

//            // Lưu đơn hàng vào cơ sở dữ liệu
//            _repository.AddOrder(order);
           
//            // Xóa giỏ hàng
//            _repository.ClearCart(new ObjectId(userId)); // Giả sử bạn có phương thức ClearCart trong repository
//        }

//        public Order GetOrderById(string orderId)
//        {
//            return _repository.GetOrderById(orderId);
//        }



//        private string GetUserNameById(string userId)
//        {
//            // Lấy thông tin người dùng từ repository (hoặc dịch vụ khác) để lấy tên người dùng
//            var user = _repository.GetOrderById(userId);
//            return user?.CustomerName ?? "Unknown";
//        }
//    }
//}