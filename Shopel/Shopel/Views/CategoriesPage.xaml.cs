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
    public partial class CategoriesPage : ContentPage
    {
        private CategoryViewModel _cvm;
        public CategoriesPage()
        {
            InitializeComponent();
            BindingContext = _cvm = new CategoryViewModel();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _cvm.TotalCartItems = await _cvm.GetCartItemsCount();
        }
    }
}