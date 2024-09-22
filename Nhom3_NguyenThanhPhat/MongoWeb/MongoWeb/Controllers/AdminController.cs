using MongoWeb.Models;
using MongoWeb.Repositores;
using MongoWeb.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace MongoWeb.Controllers
{
    public class AdminController : Controller
    {
        private AddTodo addTodo;
        private GetAll getAllTodos;
        private UserService userService;
        // GET: Admin
        private readonly TodoRepository _repository;

        public AdminController(AddTodo addTodo, GetAll getAllTodos, UserService userService, TodoRepository repository)
        {
            this.addTodo = addTodo;
            this.getAllTodos = getAllTodos;
            this.userService = userService;
            _repository = repository;
        }
        public ActionResult QuanLy()
        {
            var listproducts = getAllTodos.Excute();
            return View(listproducts);
        }
        public ActionResult QuanLyUser()
        {
            var listusers = userService.GetAllUser();
            return View(listusers);
        }
        public ActionResult ThemSanPham()
        {
            return View();
        }
        [HttpPost, ActionName("ThemSanPham")]
        public ActionResult ThemSanPham(Products product, IEnumerable<HttpPostedFileBase> ProductImages)
        {
            if (ModelState.IsValid)
            {
                var lastProductId = _repository.GetLastProductId();
                string newProductId = GenerateNewProductId(lastProductId);
                // Định nghĩa đường dẫn thư mục để lưu ảnh
                string directoryPath = Server.MapPath("~/assets/img/shop/");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Danh sách để lưu tên các ảnh đã tải lên
                var images = new List<string>();

                // Lưu từng ảnh và lưu tên ảnh
                if (ProductImages != null)
                {
                    foreach (var file in ProductImages)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = System.IO.Path.GetFileName(file.FileName);
                            var path = System.IO.Path.Combine(directoryPath, fileName);

                            try
                            {
                                // Lưu tệp vào đường dẫn đã chỉ định
                                using (var fileStream = new FileStream(path, FileMode.Create))
                                {
                                    file.InputStream.CopyTo(fileStream);
                                }

                                // Thêm tên tệp vào danh sách
                                images.Add(fileName);
                            }
                            catch (Exception ex)
                            {
                                ModelState.AddModelError("", $"Lỗi khi lưu tệp {fileName}: {ex.Message}");
                                return View(product);
                            }
                        }
                    }
                }

                // Đặt ID sản phẩm và tên các ảnh
                product.ProductId = newProductId;
                product.ProductImage = images;

                // Lưu chi tiết sản phẩm vào cơ sở dữ liệu
                addTodo.Excute(product);

                return RedirectToAction("QuanLy");
            }

            return View(product);
        }
        private string GenerateNewProductId(string lastProductId)
        {
            if (string.IsNullOrEmpty(lastProductId))
            {
                return "sp01";
            }

            var numericPart = int.Parse(lastProductId.Substring(2)) + 1;
            return "sp" + numericPart.ToString("D2");
        }
        public ActionResult Edit(string id)
        {
            var product = _repository.GetById(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Products product)
        {

            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState.Values.SelectMany(v => v.Errors);
            //    foreach (var error in errors)
            //    {
            //        // In ra lỗi xác thực để kiểm tra
            //        System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
            //    }
            //}

            //if (ModelState.IsValid)
            //{

            //    //var lastProductId = _repository.GetLastProductId();
            //    //string newProductId = GenerateNewProductId(lastProductId);
            //    // Định nghĩa đường dẫn thư mục để lưu ảnh
            //    //string directoryPath = Server.MapPath("~/assets/img/shop/");
            //    //if (!Directory.Exists(directoryPath))
            //    //{
            //    //    Directory.CreateDirectory(directoryPath);
            //    //}

            //    //// Danh sách để lưu tên các ảnh đã tải lên
            //    //var images = new List<string>();

            //    //// Lưu từng ảnh và lưu tên ảnh
            //    //if (ProductImages != null)
            //    //{
            //    //    foreach (var file in ProductImages)
            //    //    {
            //    //        if (file != null && file.ContentLength > 0)
            //    //        {
            //    //            var fileName = System.IO.Path.GetFileName(file.FileName);
            //    //            var path = System.IO.Path.Combine(directoryPath, fileName);

            //    //            try
            //    //            {
            //    //                // Lưu tệp vào đường dẫn đã chỉ định
            //    //                using (var fileStream = new FileStream(path, FileMode.Create))
            //    //                {
            //    //                    file.InputStream.CopyTo(fileStream);
            //    //                }

            //    //                // Thêm tên tệp vào danh sách
            //    //                images.Add(fileName);
            //    //            }
            //    //            catch (Exception ex)
            //    //            {
            //    //                ModelState.AddModelError("", $"Lỗi khi lưu tệp {fileName}: {ex.Message}");
            //    //                return View(product);
            //    //            }
            //    //        }
            //    //    }
            //    //}

            //    // Đặt ID sản phẩm và tên các ảnh
            //    //product.ProductId = newProductId;
            //    //var item = _repository.GetById(product.ProductId);
            //    //item.ProductId = product.ProductId;
            //    //item.ProductImage = images;

            //    // Lưu chi tiết sản phẩm vào cơ sở dữ liệu
            _repository.UpdateProduct(product.ProductId, product);

            return RedirectToAction("QuanLy");
        }

        public ActionResult Delete(string id)
        {
            var product = _repository.GetById(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfimred(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                var product = _repository.GetById(id);
                if (product != null)
                {
                    _repository.Delete(id);
                    return RedirectToAction("QuanLy");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi xóa sản phẩm: " + ex.Message);
                return View();
            }
        }
        [HttpGet]
        public ActionResult ThemUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemUser(Models.Register model)
        {
            if (ModelState.IsValid)
            {
                var lastUserId = _repository.GetLastUserId();
                string newUserId = GenerateNewUserId(lastUserId);
                var user = new Models.Register
                {
                    Username = model.Username,
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    DateRegistered = DateTime.UtcNow,
                    UserId = newUserId,
                    Role = "User"
                };

                _repository.Register(user);
                return RedirectToAction("QuanLyUser");
            }
            return View(model);
        }

        private string GenerateNewUserId(string lastUserId)
        {
            if (string.IsNullOrEmpty(lastUserId))
            {
                return "user001";
            }

            var numericPart = int.Parse(lastUserId.Substring(4)) + 1;
            return "user" + numericPart.ToString("D3");
        }
        public ActionResult EditUser(String id)
        {
            var user = userService.GetUserById(id);
            return View(user);
        }
        [HttpPost]
        public ActionResult EditUser(Models.Register user)
        {
            if (ModelState.IsValid)
            {
                var item = userService.GetUserById(user.UserId);
                if (item != null)
                {
                    //user.UserId = user.UserId;
                    item.Username = user.Username;
                    item.FullName = user.FullName;
                    item.Password = user.Password;
                    item.Email = user.Email;
                    item.PhoneNumber = user.PhoneNumber;
                    item.Address = user.Address;
                    item.DateRegistered = user.DateRegistered;

                    _repository.UpdateUser(user.UserId, item);
                }

                return RedirectToAction("QuanLyUser");
            }

            return View(user);
        }
        public ActionResult DeleteUser(string id)
        {
            var user = userService.GetUserById(id);
            return View(user);
        }
        [HttpPost, ActionName("DeleteUser")]
        public ActionResult DeleteUserConfirmed(string id)
        {
            _repository.DeleteUser(id);
            return RedirectToAction("QuanLyUser");
        }

        [HttpPost]
        public ActionResult UpdateStatus ( Order order)
        {
            _repository.UpdateOrder(order.Email,order);
            return RedirectToAction("OrderList");
        }
        public ActionResult OrderList()
        {
            string email = (string)Session["UserName"] ?? "Guest";

            ViewBag.UserName = Session["UserName"] ?? "Guest";

            var orders = _repository.GetAllOrdersAdmin();

            return View(orders);
        }

    }
}