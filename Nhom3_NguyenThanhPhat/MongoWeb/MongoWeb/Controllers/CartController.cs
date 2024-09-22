using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoWeb.Models;
using MongoWeb.Repositores;
using MongoWeb.Services;

namespace MongoWeb.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart

        private Cart _Cart;
        public ITodoRepository iTodoRepository;


        public CartController(Cart Cart, ITodoRepository todoRepository)
        {
            this._Cart = Cart;
            this.iTodoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
        }

        public ActionResult Index()
        {
            var cart = _Cart.GetCartItems();
            return View(cart);
        }

        public ActionResult AddToCart(string id, int quantity = 1)
        {
            _Cart.AddToCart(id, quantity);
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(string id)
        {
            _Cart.RemoveFromCart(id);
            return RedirectToAction("Index");
        }

        public ActionResult ClearCart()
        {
            _Cart.ClearCart();
            return RedirectToAction("Index");
        }

        // đặt hàng 

        //[HttpPost]
        //public ActionResult PlaceOrder(Order model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var cartItems = iTodoRepository.GetCartItems();

        //        // Kiểm tra nếu giỏ hàng rỗng
        //        if (cartItems.Count == 0)
        //        {
        //            ModelState.AddModelError("", "Giỏ hàng của bạn đang trống.");
        //            return View("Checkout", model);
        //        }

        //        // Tính toán tổng số tiền
        //        decimal totalAmount = cartItems.Sum(item => item.Product.Price * item.Quantity);

        //        // Tạo đối tượng đơn hàng
        //        var order = new Order
        //        {
        //            OrderId = Guid.NewGuid().ToString(), // Sinh mã đơn hàng
        //            Email = model.Email, // Sử dụng Email thay vì CustomerName
        //            OrderDate = DateTime.UtcNow,
        //            TotalAmount = totalAmount,
        //            Items = cartItems.Select(item => new OrderItem
        //            {
        //                ProductId = item.Product.Id.ToString(),
        //                Quantity = item.Quantity
        //            }).ToList(),
        //            ShippingAddress = model.ShippingAddress,
        //            PaymentMethod = model.PaymentMethod,
        //            Status = "Pending" // Hoặc trạng thái khác tùy theo logic của bạn
        //        };

        //        // Lưu đơn hàng vào cơ sở dữ liệu
        //        iTodoRepository.PlaceOrder(order);

        //        // Xóa giỏ hàng sau khi đặt hàng
        //        iTodoRepository.ClearCart(ObjectId.Empty); // Thay thế ObjectId.Empty bằng ID của người dùng hiện tại nếu có

        //        // Cập nhật ViewBag với tên người dùng
        //        ViewBag.UserName = Session["UserName"] ?? "Guest";

        //        return View("OrderConfirmation", order);
        //    }

        //    // Nếu không hợp lệ, trả về lại trang checkout
        //    ViewBag.UserName = Session["UserName"] ?? "Guest";
        //    return View("Checkout", model);
        //}
        [HttpGet]
        public ActionResult PlaceOrder()
        {
            // Đảm bảo người dùng đã đăng nhập
            ViewBag.UserName = Session["UserName"] ?? "Guest";

            var cartItems = iTodoRepository.GetCartItems();
            if (cartItems.Count == 0)
            {
                return RedirectToAction("Index"); // Quay lại giỏ hàng nếu giỏ hàng trống
            }

            var model = new Order(); // Tạo một mô hình đơn hàng rỗng hoặc tùy chỉnh thêm thông tin nếu cần
            return View(model);
        }

        [HttpPost]
        public ActionResult PlaceOrder(Order model)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    var cartItems = iTodoRepository.GetCartItems();

                    if (cartItems.Count == 0)
                    {
                        ModelState.AddModelError("", "Giỏ hàng của bạn đang trống.");
                        return View(model);
                    }
                    var totalAmount = cartItems.Sum(item => item.Product.Price * item.Quantity);
                    var order = new Order
                    {
                        OrderId = Guid.NewGuid().ToString(),
                        Email = model.Email,
                        OrderDate = DateTime.UtcNow,
                        //TotalAmount = cartItems.Sum(item => item.Product.Price * item.Quantity),
                        TotalAmount = totalAmount,
                        Items = cartItems.Select(item => new OrderItem
                        {
                            ProductId = item.Product.ProductId,
                            Quantity = item.Quantity
                        }).ToList(),
                        ShippingAddress = model.ShippingAddress,
                        PaymentMethod = model.PaymentMethod,
                        Status = "Pending"
                    };
                    foreach (var item in order.Items)
                    {

                        iTodoRepository.UpdateQuantityProduct(item.ProductId, item.Quantity);
                    }

                  
                    iTodoRepository.PlaceOrder(order);
                    iTodoRepository.ClearCart(ObjectId.Empty); // Thay thế ObjectId.Empty bằng ID của người dùng hiện tại nếu có

                    return RedirectToAction("OrderConfirmation");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.UserName = Session["UserName"] ?? "Guest";
            return View(model);
        }

        public ActionResult OrderConfirmation()
        {
            // Hiển thị thông tin xác nhận đơn hàng
            ViewBag.UserName = Session["UserName"] ?? "Guest";
            return View();
        }
        public ActionResult OrderList()
        {
            string email = (string)Session["UserName"] ?? "Guest";

            ViewBag.UserName = Session["UserName"] ?? "Guest";
            var orders = iTodoRepository.GetAllOrders(email);

            return View(orders);
        }
    }

}