using AkilliAlisverisApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AkilliAlisverisApp.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<bool> DeleteProductAsync(int id);
        Task<Product?> AddProductAsync(Product product);
    }
}
