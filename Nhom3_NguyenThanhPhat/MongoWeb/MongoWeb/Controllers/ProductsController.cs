using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MongoDB.Driver;
using MongoWeb.Models;

namespace MongoWeb.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        private readonly IMongoCollection<Products> _productsCollection;
       

        public ProductsController()
        {
            var client = new MongoClient("mongodb://localhost:27017"); // Kết nối đến MongoDB
            var database = client.GetDatabase("Cua_Hang_My_Pham"); // Đổi tên cơ sở dữ liệu của bạn ở đây
            _productsCollection = database.GetCollection<Products>("Products");
          
            // Đổi tên collection của bạn nếu cần
        }

        // API Get: /api/products
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetProducts()
        {
            var products = await _productsCollection.Find(product => true).ToListAsync();
            return Ok(products);
        }
       

        // API Get: /api/products/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetProduct(string id)
        {
            var product = await _productsCollection.Find(p => p.ProductId == id).FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
       


        // API Post: /api/products
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> AddProduct([FromBody] Products product)
        {
            await _productsCollection.InsertOneAsync(product);
            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }
        
    }
}
