using MongoWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MongoWeb.Controllers
{
    public class ProductApiController : ApiController
    {
        
            private readonly GetAll _getAllService;

        public ProductApiController() { }
        public ProductApiController(GetAll getAllService)
            {
                _getAllService = getAllService ?? throw new ArgumentNullException(nameof(getAllService));
            }

        [HttpGet]
        [Route("api/products")]
        public IHttpActionResult GetProducts()

        {
            var products = _getAllService.Excute();

            return Ok(products);
        }


        [HttpGet]
            [Route("debug")]    
            public IHttpActionResult Debug()
            {
                return Ok("Debugging ProductsApiController");
            }


           

            [HttpGet]
            [Route("test")]
            public IHttpActionResult Test()
            {
                return Ok("Controller is working!");
            }


        }

    }

