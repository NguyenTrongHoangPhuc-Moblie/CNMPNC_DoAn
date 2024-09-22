using MongoWeb.Models;
using MongoWeb.Repositores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 
namespace MongoWeb.Services
{
    public class GetAll
    {
        public ITodoRepository todoRepository;

        
        public GetAll(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }
        public List<Products> Excute()
        {
            return todoRepository.GetAll();
        }
        public List<string> GetProductCategories()
        {
            return todoRepository.GetProductCategories();
        }
        public Products GetById(string id)
        {
            return todoRepository.GetById(id);
        }
        public List<Products> Search(string query)
        {
            return todoRepository.SearchProducts(query);
        }
    }
}