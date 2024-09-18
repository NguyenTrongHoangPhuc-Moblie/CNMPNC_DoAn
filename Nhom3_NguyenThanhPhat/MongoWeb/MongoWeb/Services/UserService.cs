using MongoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoWeb.Services
{
    public class UserService
    {
        public ITodoRepository todoRepository;

        public UserService(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;

        }
        public List<Users> GetAllUser()
        {
            return todoRepository.GetAllUsers();
        }
        public Users GetUserById(string id)
        {
            return todoRepository.GetUserById(id);
        }
        //public void Updateuser(Users user)
        //{
        //    todoRepository.Updateuser(user);
        //}
    }
}