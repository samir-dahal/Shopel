using Refit;
using Shopel.ApiHelper.Interfaces;
using Shopel.Models;
using Shopel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shopel.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetailPage : ContentPage
    {
        private ProductDetailViewModel _pdvm;
        private bool _initialized;
        public ProductDetailPage()
        {
            InitializeComponent();
            _initialized = true;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _pdvm = BindingContext as ProductDetailViewModel;
            _pdvm.TotalCartItems = await _pdvm.GetCartItemsCount();
            _pdvm.LoadSimilarProductsCommand.Execute(null);
        }

        private void _stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (_initialized)
            {
                _pdvm.Quantity = (int)e.NewValue;
                _pdvm.ExpandedPrice = _pdvm.Quantity * _pdvm.Product.Price;
            }
        }
    }
}