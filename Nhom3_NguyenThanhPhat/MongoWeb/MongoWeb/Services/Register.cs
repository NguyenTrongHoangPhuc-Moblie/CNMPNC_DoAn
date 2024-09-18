using MongoWeb.Models;
using MongoWeb.Repositores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoWeb.Services
{
    public class Register
    {
        public readonly ITodoRepository _todoRepository;

        public Register(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public bool Authenticate(Models.Register model)
        {
            try
            {
                _todoRepository.Register(model);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}