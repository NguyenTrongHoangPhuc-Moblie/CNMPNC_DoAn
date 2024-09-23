using MongoWeb.Models;
using MongoWeb.Repositores;
using MongoWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZstdSharp.Unsafe;

namespace MongoWeb.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductController : ApiController
    {
        private readonly GetAll _repoGetAll;
        private readonly AddTodo _repoAddToDo;
        private readonly TodoRepository _repoToDo;

        public ProductController(GetAll repoGetAll, AddTodo repoAddToDo, TodoRepository repoToDo)
        {
            _repoGetAll = repoGetAll;
            _repoAddToDo = repoAddToDo;
            _repoToDo = repoToDo;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllProduct()
        {
            try
            {
                var products = _repoGetAll.Excute();
                return Ok(products);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } 
            
            
        }

        [HttpGet()]
        [Route("{id}")]
        public IHttpActionResult GetProductById(string id)
        {
            try
            {
                var product = _repoGetAll.GetById(id);
                if(product != null)
                {
                    return Ok(product);
                }
                else
                {
                    return NotFound();
                }
                
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateProduct(Products model)
        {
            if (model == null)
            {
                return BadRequest("Invalid product data.");
            }

            try
            {
                _repoAddToDo.Excute(model);
                return CreatedAtRoute("GetProductById", new { id = model.ProductId }, model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateProduct(string id, Products model)
        {
            if (model == null || model.ProductId != id)
            {
                return BadRequest("Invalid product data.");
            }

            try
            {
                _repoToDo.UpdateProduct(id, model);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteProduct(string id)
        {
            try
            {
                _repoToDo.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("search")]
        public IHttpActionResult SearchProduct(string name)
        {
            try
            {
                var product = _repoToDo.SearchProducts(name);
                if (product != null)
                {
                    return Ok(product);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
