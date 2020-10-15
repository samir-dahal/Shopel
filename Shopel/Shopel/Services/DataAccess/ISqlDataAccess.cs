using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shopel.DataAccess
{
    public interface ISqlDataAccess
    {
        Task<int> InsertCartItemAsync(ProductDb product);
        Task<ProductDb> GetCartItemById(int id);
        Task<ProductDb> DeleteCartItem(ProductDb product);
        Task<List<ProductDb>> GetCartItemsAsync();
    }
}
