using Refit;
using Shopel.ApiHelper;
using Shopel.ApiHelper.Interfaces;
using Shopel.DataAccess;
using Shopel.Domains;
using Shopel.Models;
using Shopel.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Shopel.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        private readonly CategoryModel _category;
        public ProductViewModel(CategoryModel category = null)
        {
            _category = category;
            Title = _category?.Name ?? "Shopel";
        }
        private Command _refreshCommand;
        public ICommand RefreshCommand
        {
            get => _refreshCommand ?? (_refreshCommand = new Command(async () =>
            {
                IsRefreshing = true;
                await LoadMoreProducts();
                IsRefreshing = false;
            }));
        }
        private Command _loadMoreProductsCommand;
        public ICommand LoadMoreProductsCommand
        {
            get => _loadMoreProductsCommand ?? (_loadMoreProductsCommand = new Command(async () =>
            {
                IsBusy = true;
                await LoadMoreProducts();
                IsBusy = false;
                Skip++;

            }, () => EndReached == false));
        }
        private Command _searchProductCommand;
        public ICommand SearchProductCommand
        {
            get => _searchProductCommand ?? (_searchProductCommand = new Command(() =>
            {
                Products.Clear();
                if (EndReached)
                {
                    EndReached = false;
                    LoadMoreProductsCommand.Execute(null);
                }
            }));
        }
        public ObservableCollection<ProductModel> Products { get; set; } = new ObservableCollection<ProductModel>();
        public async Task LoadMoreProducts()
        {
            if (IsRefreshing)
            {
                EndReached = false;
                IsBusy = false;
                Products.Clear();
                Skip = RandomSkip(1, 500);
            }
            if (_category != null)
            {
                IsSearchBarVisible = false;
                await LoadMoreProductsByCategory(_category.Id);
            }
            else if (string.IsNullOrWhiteSpace(SearchTerm) == false)
            {
                await LoadProductsBySearchTerm();
            }
            else
            {
                await LoadProducts();
            }
        }
        public async Task LoadMoreProductsByCategory(string id)
        {
            if (IsRefreshing)
            {
                IsBusy = false;
                Skip = 0;
            }
            try
            {
                var api = ApiHelper();
                var content = (await api.GetProductsByCategoryIdAsync(id, Skip)).Content;
                MapToProductModel(content);
            }
            catch
            {
                await ExceptionHandler();
            }
        }
        public async Task LoadProducts()
        {
            try
            {
                Skip = RandomSkip(1, 500);
                var api = ApiHelper();
                var content = (await api.GetProductsAsync(Skip)).Content;
                MapToProductModel(content);
            }
            catch
            {
                await ExceptionHandler();
            }
        }
        public async Task LoadProductsBySearchTerm()
        {
            try
            {
                var api = ApiHelper();
                var content = (await api.GetProductsBySearchTerm(SearchTerm)).Content;
                MapToProductModel(content);
            }
            catch
            {
                await ExceptionHandler();
            }
        }
        private void MapToProductModel(Response<Product> content)
        {
            if (content.Total > 0)
            {
                var products = content.Data;
                products.ForEach(p => Products.Add(new ProductModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Image = p.Image,
                    Model = p.Model,
                    Price = p.Price,
                    Shipping = p.Shipping,
                    Type = p.Type,
                    Upc = p.Upc,
                    Categories = p.Categories.Select(c => new CategoryModel
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList()
                }));
            }
            else
            {
                EndReached = true;
            }
        }
    }
}
