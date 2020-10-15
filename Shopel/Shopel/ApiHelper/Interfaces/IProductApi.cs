using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Refit;
using System.Threading;
using Shopel.Domains;

namespace Shopel.ApiHelper.Interfaces
{
    public interface IProductApi
    {
        [Get("/products?$skip={skip}")]
        Task<ApiResponse<Response<Product>>> GetProductsAsync(int skip = 0);
        [Get("/categories?$skip={skip}")]
        Task<ApiResponse<Response<Category>>> GetCategoriesAsync(int skip = 0);
        [Get("/categories/{id}")]
        Task<ApiResponse<Response<Category>>> GetCategoryByIdAsync(string id);
        [Get("/products/{id}")]
        Task<ApiResponse<Response<Product>>> GetProductByIdAsync(string id);
        [Get("/products?category.id={id}&$skip={skip}")]
        Task<ApiResponse<Response<Product>>> GetProductsByCategoryIdAsync(string id, int skip = 0);
        [Get("/products?name[$like]=*{searchTerm}*")]
        Task<ApiResponse<Response<Product>>> GetProductsBySearchTerm(string searchTerm);
        [Get("/categories?name[$like]=*{searchTerm}*")]
        Task<ApiResponse<Response<Category>>> GetCategoriesBySearchTerm(string searchTerm);
    }
}
