using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoWeb.Models
{
    public class Products
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [BsonElement("product_id")]
        public string ProductId { get; set; }

        [BsonElement("product_name")]
        public string ProductName { get; set; }

        [BsonElement("brandName")]
        public string BrandName { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("productImage")]
        public List<string> ProductImage { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("reviews")]
        public List<Review> Reviews { get; set; }

        [BsonElement("countInstock")]
        public int CountInStock { get; set; }
    }

    public class Review
    {
        [BsonElement("user")]
        public string User { get; set; }
           
        [BsonElement("rating")]
        public int Rating { get; set; }

        [BsonElement("comment")]
        public string Comment { get; set; }
    }
}