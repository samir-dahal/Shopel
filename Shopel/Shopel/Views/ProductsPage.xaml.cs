using Newtonsoft.Json;
using Refit;
using Shopel.ApiHelper.Interfaces;
using Shopel.DataAccess;
using Shopel.Models;
using Shopel.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shopel.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsPage : ContentPage
    {
        private ProductViewModel _pvm;
        public ProductsPage(CategoryModel category)
        {
            InitializeComponent();
            BindingContext = _pvm = new ProductViewModel(category);
        }
        public ProductsPage() 
        {
            InitializeComponent();
            BindingContext = _pvm = new ProductViewModel(); 
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _pvm.TotalCartItems = await _pvm.GetCartItemsCount();
        }
    }
}