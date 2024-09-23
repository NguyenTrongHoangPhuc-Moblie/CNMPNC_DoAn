using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoWeb.Models;
using MongoDB.Bson;
using System.Web.Helpers;


namespace MongoWeb.Repositores
{
    public class TodoRepository : ITodoRepository
    {
        public readonly IMongoCollection<Products> collection;
        public readonly IMongoCollection<Users> collectionUser;
        public readonly IMongoCollection<Order> collectionOrder; // Thêm IMongoCollection cho Order


        public TodoRepository(IMongoCollection<Products> database, IMongoCollection<Users> userCollection , IMongoCollection<Order> orderCollection)
        {
            collection = database;
            collectionUser = userCollection;
            collectionOrder = orderCollection;
        }
        //Them sp
        public void Add(Products products)
        {
            collection.InsertOne(products);
        }
        //Lay tat ca sp
        public List<Products> GetAll()
        {
            return collection.Find(_ => true).ToList();
        }
        //Lay sp theo id
        public Products GetById(string id) // Thực hiện truy vấn theo ID
        {
            return collection.Find(p => p.ProductId == id).FirstOrDefault();
        }

        // Xóa sản phẩm theo ID
        public void Delete(string id)
        {
            var product = GetById(id);
            if (product != null)
            {
                var filter = Builders<Products>.Filter.Eq(p => p.ProductId, id);
                collection.DeleteOne(filter);
            }
        }

        // Sửa sản phẩm theo ID
        public void UpdateProduct(string id, Products updatedProduct)
        {
            var filter = Builders<Products>.Filter.Eq(u => u.ProductId, id);
            var updateDefinition = Builders<Products>.Update
                .Set(u => u.ProductName, updatedProduct.ProductName)
                .Set(u => u.BrandName, updatedProduct.BrandName)
                .Set(u => u.ProductImage, updatedProduct.ProductImage)
                .Set(u => u.Price, updatedProduct.Price)
                .Set(u => u.Category, updatedProduct.Category)
                .Set(u => u.Description, updatedProduct.Description)
                .Set(u => u.CountInStock, updatedProduct.CountInStock);

            var updateResult = collection.UpdateOne(filter, updateDefinition);

            if (!updateResult.IsAcknowledged || updateResult.ModifiedCount == 0)
            {
                throw new Exception("Update failed");
            }
        }

        // Sửa sản phẩm theo ID
        public void UpdateOrder(string email,Order updated)
        {
            var filter = Builders<Order>.Filter.Eq(u => u.Email, email);
            var updateDefinition = Builders<Order>.Update
                .Set(u => u.Status, updated.Status);

            var updateResult = collectionOrder.UpdateOne(filter, updateDefinition);

            if (!updateResult.IsAcknowledged || updateResult.ModifiedCount == 0)
            {
                throw new Exception("Update failed");
            }
        }

        public void UpdateQuantityProduct(string productId, int quantity)
        {
            // Lấy sản phẩm hiện tại để kiểm tra số lượng tồn kho
            var productFilter = Builders<Products>.Filter.Eq(p => p.ProductId, productId);
            var product = collection.Find(productFilter).FirstOrDefault();

            if (product == null)
            {
                throw new Exception("Product not found");
            }

            // Kiểm tra số lượng tồn kho đủ để giảm không
            if (product.CountInStock < quantity)
            {
                throw new Exception("Not enough stock available");
            }

            // Cập nhật số lượng tồn kho
            var updateFilter = Builders<Products>.Filter.Eq(p => p.ProductId, productId);
            var updateDefinition = Builders<Products>.Update
                .Set(p => p.CountInStock, product.CountInStock - quantity);

            var updateResult = collection.UpdateOne(updateFilter, updateDefinition);

            if (!updateResult.IsAcknowledged || updateResult.ModifiedCount == 0)
            {
                throw new Exception("Update failed");
            }
        }

        //public void UpdateQuantityProduct(int Quantity , string productID)
        //{
        //    var filter = Builders<Products>.Filter.Eq(u => u.ProductId, productID);
        //    var updateDefinition = Builders<Products>.Update
        //        .Set(u => u.CountInStock, Quantity);

        //    var updateResult = collection.UpdateOne(filter, updateDefinition);

        //    if (!updateResult.IsAcknowledged || updateResult.ModifiedCount == 0)
        //    {
        //        throw new Exception("Update failed");
        //    }
        //}


        //Phat sửa
        //public void UpdateUser(string id, Users updatedUser)
        //{
        //    var filter = Builders<Users>.Filter.Eq(u => u.UserId, id);
        //    var updateDefinition = Builders<Users>.Update
        //        .Set(u => u.Username, updatedUser.Username)
        //        .Set(u => u.FullName, updatedUser.FullName)
        //        .Set(u => u.Email, updatedUser.Email)
        //        .Set(u => u.Password, updatedUser.Password)
        //        .Set(u => u.PhoneNumber, updatedUser.PhoneNumber)
        //        .Set(u => u.Address, updatedUser.Address)
        //        .Set(u => u.DateRegistered, updatedUser.DateRegistered);

        //    var updateResult = collectionUser.UpdateOne(filter, updateDefinition);

        //    if (!updateResult.IsAcknowledged || updateResult.ModifiedCount == 0)
        //    {
        //        throw new Exception("Update failed");
        //    }
        //}

        // User Methods
        //Them user
        public void Add(Users users)
        {
            collectionUser.InsertOne(users);
        }
        //Lay tat ca user
        public List<Users> GetAllUser()
        {
            return collectionUser.Find(_ => true).ToList();
        }
        //Lay user theo id
        public Users GetUserById(string id) // Thực hiện truy vấn theo ID
        {
            return collectionUser.Find(p => p.UserId == id).FirstOrDefault();
        }

        // Xóa sản phẩm theo ID
        public void DeleteUser(string id)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                var filter = Builders<Users>.Filter.Eq(p => p.UserId, id);
                collectionUser.DeleteOne(filter);
            }
        }

        // Sửa User theo ID
        //public void UpdateUser(string id, Users updatedUser)
        //{
        //    var existingProduct = GetUserById(id);
        //    if (existingProduct != null)
        //    {
        //        var filter = Builders<Users>.Filter.Eq(p => p.UserId, id);
        //        collectionUser.ReplaceOne(filter, updatedUser);
        //    }
        //}
        //Lay danh muc sp
        public List<string> GetProductCategories()
        {
            var distinctCategories = collection.Distinct<string>("category", FilterDefinition<Products>.Empty).ToList();
            return distinctCategories;
        }

        

        //Tim sp
        public List<Products> SearchProducts(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                // Trả về tất cả sản phẩm nếu query rỗng hoặc null
                return collection.Find(_ => true).ToList(); // Trả về tất cả sản phẩm
            }

            // Loại bỏ khoảng trắng thừa từ query
            query = query.Trim();

            // Tạo biểu thức chính quy với tùy chọn không phân biệt chữ hoa chữ thường
            var filter = Builders<Products>.Filter.Regex("ProductName", new MongoDB.Bson.BsonRegularExpression($".*{query}.*", "i"));

            // Thực hiện tìm kiếm với filter
            return collection.Find(filter).ToList();


          

        }

        //Login
        public Users Login(string email, string password)
        {
            var user = collectionUser.Find(u => u.Email == email && u.Password == password).FirstOrDefault();
            if (user == null)
            {
                throw new Exception("Invalid login credentials.");
            }
            return user;
        }


        public void CheckRole(string gmail, string password,string role)
        {
            var user = collectionUser.Find(u => u.Email == gmail && u.Password == password ).FirstOrDefault();
            if (user == null)
            {
                throw new Exception("Invalid login credentials.");
            }

        }

        //Get UserId mới
        public string GetLastUserId()
        {
            var lastUser = collectionUser.Find(FilterDefinition<Users>.Empty)
                                          .SortByDescending(u => u.DateRegistered)
                                          .FirstOrDefault();
            return lastUser?.UserId;
        }
        //Get PductId mới
        public string GetLastProductId()
        {
            var lastProduct = collection.Find(FilterDefinition<Products>.Empty)
                                        .SortByDescending(p => p.ProductId)
                                        .FirstOrDefault();
            return lastProduct?.ProductId;
        }
        public void Register(Register model)
        {
            var user = new Users
            {
                UserId = model.UserId,
                Username = model.Username,
                FullName = model.FullName,
                Email = model.Email,
                Password = model.Password,
                PhoneNumber = model.PhoneNumber,
                Role = model.Role,
                Address = model.Address,
                DateRegistered = model.DateRegistered,
            };

            collectionUser.InsertOne(user);
        }

        //kiem tra mail trùng
        public bool IsEmailExists(string email)
        {
            // Kiểm tra email trong cơ sở dữ liệu
            var existingUser = collectionUser.Find(user => user.Email == email).FirstOrDefault();
            return existingUser != null;
        }

        public List<Users> GetAllUsers()
        {
            return collectionUser.Find(_ => true).ToList();
        }

        public Users Getuser(string id)
        {
            return collectionUser.Find(user => user.Id == new ObjectId(id)).FirstOrDefault();
        }
        // get role
        public Users GetUserByEmail(string email)
        {
            return collectionUser.Find(user => user.Email == email).FirstOrDefault();
        }

        public void Deleteuser(string id)
        {
            collectionUser.DeleteOne(user => user.Id == new ObjectId(id));
        }
        public void UpdateUser(string id, Users updatedUser)
        {
            var filter = Builders<Users>.Filter.Eq(u => u.UserId, id);
            var updateDefinition = Builders<Users>.Update
                .Set(u => u.Username, updatedUser.Username)
                .Set(u => u.FullName, updatedUser.FullName)
                .Set(u => u.Email, updatedUser.Email)
                .Set(u => u.Password, updatedUser.Password)
                .Set(u => u.PhoneNumber, updatedUser.PhoneNumber)
                .Set(u => u.Address, updatedUser.Address)
                .Set(u => u.DateRegistered, updatedUser.DateRegistered);

            var updateResult = collectionUser.UpdateOne(filter, updateDefinition);

            if (!updateResult.IsAcknowledged || updateResult.ModifiedCount == 0)
            {
                throw new Exception("Update failed");
            }
        }
        //Cart
        public List<CartItem> GetCartItems()
        {
            if (HttpContext.Current.Session["Cart"] == null)
            {
                HttpContext.Current.Session["Cart"] = new List<CartItem>();
            }
            return (List<CartItem>)HttpContext.Current.Session["Cart"];
        }

        public void AddToCart(ObjectId userId, Products product, int quantity)
        {
            var cartItems = GetCartItems();
            var cartItem = cartItems.FirstOrDefault(item => item.Product.Id == product.Id);

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

        //public void RemoveFromCart(ObjectId userId, ObjectId productId)
        //{
        //    var cartItems = GetCartItems();
        //    var cartItem = cartItems.FirstOrDefault(item => item.Product.Id == productId);

        //    if (cartItem != null)
        //    {
        //        cartItems.Remove(cartItem);
        //    }

        //    HttpContext.Current.Session["Cart"] = cartItems;
        //}

        public void ClearCart(ObjectId userId)
        {
            HttpContext.Current.Session["Cart"] = new List<CartItem>();
        }


        //order
        //public void AddOrder(Order order)
        //{
        //    collectionOrder.InsertOne(order);
        //}

        //public Order GetOrderById(string id)
        //{
        //    return collectionOrder.Find(o => o.Id == id).FirstOrDefault();
        //}

        //public List<Order> GetAllOrders()
        //{
        //    return collectionOrder.Find(_ => true).ToList();
        //}

        //public void UpdateOrder(Order order)
        //{
        //    collectionOrder.ReplaceOne(o => o.Id == order.Id, order);
        //}

        //public void DeleteOrder(string orderId)
        //{
        //    collectionOrder.DeleteOne(o => o.Id == orderId);
        //}

        //public void PlaceOrder(string customerName, string shippingAddress, string paymentMethod)
        //{
        //    var cartItems = GetCartItems();
        //    if (!cartItems.Any())
        //    {
        //        throw new Exception("Cart is empty.");
        //    }

        //    var orderId = Guid.NewGuid().ToString();
        //    var totalAmount = cartItems.Sum(item => item.Product.Price * item.Quantity);

        //    var order = new Order
        //    {
        //        OrderId = orderId,
        //        CustomerName = customerName,
        //        OrderDate = DateTime.UtcNow,
        //        TotalAmount = totalAmount,
        //        Items = cartItems.Select(item => new OrderItem
        //        {
        //            ProductId = item.Product.ProductId,
        //            Quantity = item.Quantity
        //        }).ToList(),
        //        ShippingAddress = shippingAddress,
        //        PaymentMethod = paymentMethod,
        //        Status = "Pending"
        //    };

        //    AddOrder(order);
        //    ClearCart(ObjectId.Empty);
        //}
        public void PlaceOrder(Order order)
        {
            collectionOrder.InsertOne(order);
        }

        public Order GetOrderById(string id)
        {
            return collectionOrder.Find(o => o.OrderId == id).FirstOrDefault();
        }

        public List<Order> GetAllOrders(string email)
        {
            return collectionOrder.Find(order => order.Email == email).ToList();
        }

        public List<Order> GetAllOrdersAdmin()
        {
            return collectionOrder.Find(_ => true).ToList();
        }

        public void UpdateOrder(Order order)
        {
            collectionOrder.ReplaceOne(o => o.OrderId == order.OrderId, order);

        }

        public void DeleteOrder(string orderId)
        {
            collectionOrder.DeleteOne(o => o.OrderId == orderId);
        }
    }
}