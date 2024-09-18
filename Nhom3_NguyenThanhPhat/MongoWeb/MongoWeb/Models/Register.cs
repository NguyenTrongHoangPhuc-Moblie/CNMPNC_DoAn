using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.ComponentModel.DataAnnotations;

namespace MongoWeb.Models
{
    public class Register
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        
        public string UserId { get; set; }
        public string Role { get; set; }
        public DateTime DateRegistered { get; set; }
    }

}
