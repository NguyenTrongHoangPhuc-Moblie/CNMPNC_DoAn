using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoWeb.Models
{
    public class Users
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; } // MongoDB ID

        [BsonElement("user_id")]
        public string UserId { get; set; } // ID của người dùng

        [BsonElement("username")]
        public string Username { get; set; } // Tên đăng nhập

        [BsonElement("full_name")]
        public string FullName { get; set; } // Tên đầy đủ

        [BsonElement("email")]
        public string Email { get; set; } // Địa chỉ email

        [BsonElement("password")]
        public string Password { get; set; } // Mật khẩu đã mã hóa

        [BsonElement("phone_number")]
        public string PhoneNumber { get; set; } // Số điện thoại

        [BsonElement("role")]
        public string Role { get; set; } // Vai trò của người dùng

        [BsonElement("address")]
        public string Address { get; set; } // Địa chỉ của người dùng

        [BsonElement("date_registered")]
        public DateTime DateRegistered { get; set; } // Ngày đăng ký

        [BsonElement("orders")]
        public List<OrderSummary> Orders { get; set; } // Danh sách đơn hàng

        [BsonElement("payment_methods")]
        public List<string> PaymentMethods { get; set; } // Phương thức thanh toán

        [BsonElement("active")]
        public bool Active { get; set; } // Trạng thái hoạt động
    }

    //public class Address
    //{
    //    [BsonElement("street")]
    //    public string Street { get; set; } // Đường

    //    [BsonElement("city")]
    //    public string City { get; set; } // Thành phố

    //    [BsonElement("country")]
    //    public string Country { get; set; } // Quốc gia

    //    [BsonElement("zip_code")]
    //    public string ZipCode { get; set; } // Mã bưu chính
    //}

    public class OrderSummary
    {
        [BsonElement("order_id")]
        public string OrderId { get; set; } // ID đơn hàng

        [BsonElement("order_date")]
        public DateTime OrderDate { get; set; } // Ngày đặt hàng

        [BsonElement("total_amount")]
        public decimal TotalAmount { get; set; } // Tổng số tiền
    }
}