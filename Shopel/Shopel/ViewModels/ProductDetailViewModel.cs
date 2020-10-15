using Shopel.Models;
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
    public class ProductDetailViewModel : BaseViewModel
    {
        private ProductModel _product;
        public ProductModel Product
        {
            get
            {
                return _product;
            }
            set
            {
                _product = value;
                OnPropertyChanged(nameof(Product));
            }
        }
        private Command _productDetailCommand;
        public ICommand ProductDetailCommand
        {
            get => _productDetailCommand ?? (_productDetailCommand = new Command((product) =>
            {
                if(_product != (product as ProductModel))
                {
                    Product = product as ProductModel;
                }
            }));
        }
        public ObservableCollection<ProductModel> SimilarProducts { get; set; } = new ObservableCollection<ProductModel>();
        public ProductDetailViewModel(ProductModel product)
        {
            _product = product;
            ExpandedPrice = _product.Price;
        }
        private Command _loadSimilarProductsCommand;
        public ICommand LoadSimilarProductsCommand
        {
            get => _loadSimilarProductsCommand ?? (_loadSimilarProductsCommand = new Command(async () =>
            {
                await LoadSimilarProducts();
            }));
        }
        private async Task LoadSimilarProducts()
        {
            try
            {
                if (SimilarProducts.Any() == false)
                {
                    var api = ApiHelper();
                    string categoryId = Product.Categories.FirstOrDefault().Id;
                    var similarProducts = (await api.GetProductsByCategoryIdAsync(categoryId)).Content.Data;
                    similarProducts.ForEach(p => SimilarProducts.Add(new ProductModel
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
            }
            catch
            {
                await ExceptionHandler();
            }
        }
    }
}
