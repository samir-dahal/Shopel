using Shopel.DataAccess;
using Shopel.Droid.DependencyServices;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
[assembly: Dependency(typeof(SqlDataAccess))]
namespace Shopel.Droid.DependencyServices
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private SQLiteAsyncConnection _db;
        public SqlDataAccess()
        {
            _db = new SQLiteAsyncConnection(App.FilePath);
            _db.CreateTableAsync<ProductDb>().Wait();
        }
        public async Task<ProductDb> DeleteCartItem(ProductDb product)
        {
            await _db.DeleteAsync(product);
            return product;
        }

        public async Task<ProductDb> GetCartItemById(int id)
        {
            var product = await _db.Table<ProductDb>().Where(p => p.Id == id).FirstOrDefaultAsync();
            return product;
        }

        public async Task<List<ProductDb>> GetCartItemsAsync()
        {
            var cartItems = await _db.Table<ProductDb>().ToListAsync();
            return cartItems;
        }

        public async Task<int> InsertCartItemAsync(ProductDb product)
        {
            var existingProduct = await GetCartItemById(product.Id);
            if(existingProduct != null)
            {
                existingProduct.Price = product.Price;
                existingProduct.Quantity = product.Quantity;
                await _db.UpdateAsync(existingProduct);
                return 0;
            }
            else
            {
                return await _db.InsertAsync(product);
            }
        }
    }
}
