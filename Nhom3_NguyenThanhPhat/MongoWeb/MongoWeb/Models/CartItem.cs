using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoWeb.Models
{
    public class CartItem
    {

        [BsonElement("product")]
        public Products Product { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }


    }
}