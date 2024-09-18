using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MongoWeb.Models
{
    public class ProductModel
    {
        [Required]
        public string ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }

        [Required]
        public string BrandName { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public List<string> ProductImage { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int CountInStock { get; set; }
    }
}