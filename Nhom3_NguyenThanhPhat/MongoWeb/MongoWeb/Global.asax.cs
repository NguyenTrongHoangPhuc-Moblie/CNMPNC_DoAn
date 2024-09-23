using MongoDB.Driver;
using MongoWeb.Controllers;
using MongoWeb.Models;
using MongoWeb.Repositores; 
using Unity.Mvc5;
using Unity;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Web.Http;

namespace MongoWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
           

            try
            {
                // Initialize Unity container
                var container = new UnityContainer();

                // Register repositories and services
                container.RegisterType<ITodoRepository, TodoRepository>();
                container.RegisterType<Models.Register>(); // Ensure this matches your actual service

                // Initialize MongoDB connection
                var client = new MongoClient("mongodb://localhost:27017/");
                var database = client.GetDatabase("Cua_Hang_My_Pham");
                var todoCollection = database.GetCollection<Products>("Products");
                var userCollection = database.GetCollection<Users>("Users");
                var orderCollection = database.GetCollection<Order>("Orders");

                // Register MongoDB collections
                container.RegisterInstance(todoCollection);
                container.RegisterInstance(userCollection);
                container.RegisterInstance(orderCollection);

                // Register MyDependencyResolver
                container.RegisterType<HomeController>();
                container.RegisterType<CartController>();
                container.RegisterType<AccountController>();

                // Thiết lập Dependency Resolver
                DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            }
            catch (MongoConnectionException ex)
            {
                // Handle MongoDB connection error
                Console.WriteLine($"Lỗi kết nối MongoDB: {ex.Message}");
                throw new InvalidOperationException("Không thể kết nối đến MongoDB", ex);
            }
            catch (MongoException ex)
            {
                // Handle general MongoDB error
                Console.WriteLine($"Lỗi MongoDB: {ex.Message}");
                throw new InvalidOperationException("Lỗi khi làm việc với MongoDB", ex);
            }
            catch (Exception ex)
            {
                // Handle other errors
                Console.WriteLine($"Lỗi không xác định: {ex.Message}");
                throw new InvalidOperationException("Lỗi không xác định", ex);
            }
        }
    }

    public class MyDependencyResolver : IDependencyResolver
    {
        private readonly IUnityContainer _container;

        public MyDependencyResolver(IUnityContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return Enumerable.Empty<object>();
            }
        }
    }
}
