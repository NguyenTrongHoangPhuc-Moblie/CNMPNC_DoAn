using MongoDB.Driver;
using MongoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MongoWeb.Controllers
{
    [RoutePrefix("api/productstore")]
    public class ProductStoreApiController : ApiController
    {
        private readonly IMongoCollection<ProductStore> _productstoreCollection;

        public ProductStoreApiController()
        {
            var client = new MongoClient("mongodb://localhost:27017"); // Kết nối đến MongoDB
            var database = client.GetDatabase("Cua_Hang_My_Pham"); // Đổi tên cơ sở dữ liệu của bạn ở đây
            
            _productstoreCollection = database.GetCollection<ProductStore>("Stores");
            // Đổi tên collection của bạn nếu cần
        }
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetProductsStore()
        {
            var productstore = await _productstoreCollection.Find(product => true).ToListAsync();

            foreach (var store in productstore)
            {
                store.TongTien = store.DanhSachSanPham.Sum(product => product.GiaTien * product.SoLuong);
            }
            return Ok(productstore);
        }
    }
}
