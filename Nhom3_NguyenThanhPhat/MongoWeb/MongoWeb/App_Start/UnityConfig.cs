using Unity;
using Unity.WebApi;
using MongoDB.Driver;
using MongoWeb.Models;
using MongoWeb.Repositores;
using MongoWeb.Services;
using System.Web.Http;
using MongoWeb;
using MongoWeb.Controllers;

public static class UnityConfig
{
    public static void RegisterComponents()
    {
        var container = new UnityContainer();

        // Đăng ký các repository và service
        container.RegisterType<ITodoRepository, TodoRepository>();
        // container.RegisterType<Models.Register>(); // Đảm bảo dịch vụ này chính xác
       

        // Khởi tạo kết nối MongoDB
        var client = new MongoClient("mongodb://localhost:27017/");
        var database = client.GetDatabase("Cua_Hang_My_Pham");
        var todoCollection = database.GetCollection<Products>("Products");
        var userCollection = database.GetCollection<Users>("Users");
        var orderCollection = database.GetCollection<Order>("Orders");
      

        // Đăng ký các collection MongoDB
        container.RegisterInstance(todoCollection);
        container.RegisterInstance(userCollection);
        container.RegisterInstance(orderCollection);
        container.RegisterType<GetAll>();
        container.RegisterType<AddTodo>();
        container.RegisterType<ProductsApiController>();


        // Thiết lập Unity Dependency Resolver cho Web API
        GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
    }
}
