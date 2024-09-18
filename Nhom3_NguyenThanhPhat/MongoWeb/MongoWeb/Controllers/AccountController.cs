using MongoWeb.Models;
using MongoWeb.Repositores;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MongoWeb.Controllers
{
    public class AccountController : Controller
    {
        public readonly TodoRepository _repository;

        public AccountController(TodoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                if (_repository.IsEmailExists(model.Email))
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại.");
                    return View(model);
                }
                var lastUserId = _repository.GetLastUserId();
                string newUserId = GenerateNewUserId(lastUserId);

                

                // Create a new Register object to include additional fields
                var userToRegister = new Register
                {
                    Username = model.Username,
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    DateRegistered = DateTime.UtcNow,
                    UserId = newUserId,
                    Role = "user"
                };

                _repository.Register(userToRegister);
                return RedirectToAction("Login","Home");
            }
            return View();
        }

        private string GenerateNewUserId(string lastUserId)
        {
            if (string.IsNullOrEmpty(lastUserId))
            {
                return "user001";
            }

            var numericPart = int.Parse(lastUserId.Substring(4)) + 1;
            return "user" + numericPart.ToString("D3");
        }
    }
}
