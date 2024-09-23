using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MongoWeb.Models; // Đảm bảo bạn đã sử dụng đúng namespace cho mô hình Products

namespace MongoWeb.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService()
        {
            //_httpClient = new HttpClient
            //{
            //    BaseAddress = new Uri("http://localhost:44389/api/products/") // Thay thế your_port bằng cổng thực tế của bạn
            //};
        }

        // Lấy danh sách sản phẩm
        public async Task<List<Products>> GetProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("");

                // Kiểm tra trạng thái phản hồi
                response.EnsureSuccessStatusCode();

                // Đọc nội dung và chuyển đổi thành danh sách Products
                var products = await response.Content.ReadAsAsync<List<Products>>();

                // Trả về danh sách sản phẩm hoặc danh sách rỗng nếu null
                return products ?? new List<Products>();
            }
            catch (HttpRequestException ex)
            {
                // Ghi log lỗi để dễ dàng chẩn đoán
                Console.WriteLine($"HTTP request error: {ex.Message}");
                return new List<Products>(); // Trả về danh sách rỗng trong trường hợp lỗi
            }
            catch (Exception ex)
            {
                // Bắt mọi lỗi khác
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<Products>(); // Trả về danh sách rỗng
            }
        }


        // Thêm sản phẩm
        public async Task AddProductAsync(Products product)
        {
            var response = await _httpClient.PostAsJsonAsync("", product);
            response.EnsureSuccessStatusCode();
        }

        // Bạn có thể thêm các phương thức khác như cập nhật, xóa ở đây...
    }
}
