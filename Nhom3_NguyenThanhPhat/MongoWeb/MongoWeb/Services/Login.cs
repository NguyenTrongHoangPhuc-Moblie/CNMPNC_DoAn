using MongoWeb.Models;
using MongoWeb.Repositores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoWeb.Services
{
    public class Login
    {
        public ITodoRepository todoRepository;

        public Login(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }

        //public void Excute(string email, string password)
        //{
        //    // Gọi phương thức Login của repository để xác thực người dùng
        //    todoRepository.Login(email, password);
        //}
        public Users Authenticate(string email, string password)
        {
            try
            {
                return todoRepository.Login(email, password); // Đảm bảo rằng Login trả về kiểu Users
            }
            catch
            {
                return null; // Trả về null nếu xác thực không thành công
            }
        }



        //public bool AuthenticateRole(string role)
        //{
        //    try
        //    {

        //        todoRepository.CheckRole()

        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
    }
}