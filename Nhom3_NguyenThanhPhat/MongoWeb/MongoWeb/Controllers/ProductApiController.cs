using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoWeb.Models;
using MongoWeb.Services;

namespace MongoWeb.Controllers
{
    [RoutePrefix("api/products")]
public class ProductsApiController : ApiController
{
        public ProductsApiController(GetAll getAllService)
        {
            _getAllService = getAllService ?? throw new ArgumentNullException(nameof(getAllService));
        }

        private readonly GetAll _getAllService;
        [HttpGet]
        [Route("debug")]
        public IHttpActionResult Debug()
        {
            return Ok("Debugging ProductsApiController");
        }


        [HttpGet]
        [Route("")]
        public IHttpActionResult GetProducts()
        
        {
            var products = _getAllService.Excute();
            
            return Ok(products);
        }

        [HttpGet]
        [Route("test")]
        public IHttpActionResult Test()
        {
            return Ok("Controller is working!");
        }


    }

}

//     private readonly GetAll _getAllService;
//// GET api/products
//[HttpGet]
//[Route("")]
//public IHttpActionResult GetProducts()
//{
//    var products = _getAllService.Excute();
//    return Ok(products);
//}

//// GET api/products/{id}
//[HttpGet]
//[Route("{id}")]
//public IHttpActionResult GetProduct(string id)
//{
//    var product = _getAllService.GetById(id);
//    if (product == null)
//    {
//        return NotFound();
//    }
//    return Ok(product);
//}

//// GET api/products/categories
//[HttpGet]
//[Route("categories")]
//public IHttpActionResult GetProductCategories()
//{
//    var categories = _getAllService.GetProductCategories();
//    return Ok(categories);
//}

//// GET api/products/search?query={query}
//[HttpGet]
//[Route("search")]
//public IHttpActionResult Search(string query)
//{
//    var results = _getAllService.Search(query);
//    return Ok(results);
//}