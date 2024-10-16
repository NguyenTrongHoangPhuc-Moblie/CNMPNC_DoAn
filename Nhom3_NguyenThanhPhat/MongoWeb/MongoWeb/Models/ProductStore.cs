using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MongoWeb.Models
{
    public class ProductStore
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // Sử dụng string thay cho ObjectId

        [BsonElement("idstore")]
        public string IdStore { get; set; }

        [BsonElement("ngay_nhap")]
        public DateTime NgayNhap { get; set; }

        [BsonElement("tong_tien")]
        public decimal TongTien { get; set; }

        [BsonElement("danh_sach_san_pham")]
        public List<Product> DanhSachSanPham { get; set; } // Danh sách các sản phẩm nhúng
        
        [BsonElement("NhaCungCap")]
        public string NhaCungCap { get; set; }
    }

    public class Product
    {
        [BsonElement("product_name")]
        public string ProductName { get; set; }

        [BsonElement("so_luong")]
        public int SoLuong { get; set; }

        [BsonElement("gia_tien")]
        public decimal GiaTien { get; set; }

        [BsonElement("ngay_het_han")]
        public DateTime NgayHetHan { get; set; }

        [BsonElement("ngay_san_xuat")]
        public DateTime NgaySanXuat { get; set; }
    }
}
