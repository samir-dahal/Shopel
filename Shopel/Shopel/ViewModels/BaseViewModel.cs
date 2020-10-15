using Refit;
using Shopel.ApiHelper;
using Shopel.ApiHelper.Interfaces;
using Shopel.DataAccess;
using Shopel.Models;
using Shopel.Services;
using Shopel.Services.DataAccess;
using Shopel.ToastMessaging;
using Shopel.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Shopel.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private Page _main = App.Current.MainPage;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public async Task AlertAsync(string title, string message, string cancel = "OK")
        {
            await _main.DisplayAlert(title, message, cancel);
        }
        public async Task PushAsync(Page page)
        {
            await _main.Navigation.PushAsync(page);
        }
        public async Task PopAsync()
        {
            await _main.Navigation.PopAsync();
        }
        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        private double _expandedPrice;
        public double ExpandedPrice
        {
            get
            {
                return _expandedPrice;
            }
            set
            {
                _expandedPrice = value;
                OnPropertyChanged(nameof(ExpandedPrice));
            }
        }
        private int _quantity = 1;
        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
        private Command _addToCartCommand;
        public ICommand AddToCartCommand
        {
            get => _addToCartCommand ?? (_addToCartCommand = new Command(async (product) =>
            {
                var db = DependencyService.Get<ISqlDataAccess>();
                var toast = DependencyService.Get<IToastMessage>();
                var p = product as ProductModel;
                var cartItem = new ProductDb
                {
                    Id = p.Id,
                    Description = p.Description,
                    Image = p.Image,
                    Model = p.Model,
                    Name = p.Name,
                    Price = ExpandedPrice,
                    Shipping = p.Shipping,
                    Quantity = Quantity,
                    Type = p.Type,
                    Upc = p.Upc
                };
                int row = await db.InsertCartItemAsync(cartItem);
                if (row > 0)
                {

                    toast.ShortAlert("Item added to cart");
                    TotalCartItems++;
                }
                else
                {
                    toast.ShortAlert("Item updated in cart");
                }
            }));
        }
        private int _totalCartItems;
        public int TotalCartItems
        {
            get
            {
                return _totalCartItems;
            }
            set
            {
                _totalCartItems = value;
                OnPropertyChanged(nameof(TotalCartItems));
            }
        }
        private bool _endReached;
        public bool EndReached
        {
            get
            {
                return _endReached;
            }
            set
            {
                _endReached = value;
                OnPropertyChanged(nameof(EndReached));
            }
        }
        private Command _productDetailPageCommand;
        public ICommand ProductDetailPageCommand
        {
            get => _productDetailPageCommand ?? (_productDetailPageCommand = new Command(async (product) =>
            {
                await PushAsync(new ProductDetailPage
                {
                    BindingContext = new ProductDetailViewModel((product as ProductModel))
                });
            }));
        }
        private Command _cartPageCommand;
        public ICommand CartPageCommand
        {
            get => _cartPageCommand ?? (_cartPageCommand = new Command(async () =>
            {
                await PushAsync(new CartPage
                {
                    BindingContext = new CartViewModel()
                });
            }));
        }
        public async Task<int> GetCartItemsCount()
        {
            return (await DependencyService.Get<ISqlDataAccess>().GetCartItemsAsync()).Count;
        }
        public int RandomSkip(int min, int max)
        {
            return new Random().Next(min, max);
        }
        public bool HasInternet()
        {
            var current = Connectivity.NetworkAccess;
            return current == NetworkAccess.Internet;
        }
        public async Task ExceptionHandler(string message = "unable to connect to the server")
        {
            IsBusy = IsRefreshing = false;
            await AlertAsync("Error occured", message);
        }
        public int Skip { get; set; } = new Random().Next(1, 500);
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        private string _searchTerm;
        public string SearchTerm
        {
            get
            {
                return _searchTerm;
            }
            set
            {
                _searchTerm = value;
                OnPropertyChanged(nameof(SearchTerm));
            }
        }
        private bool _isSearchBarVisisble = true;
        public bool IsSearchBarVisible
        {
            get
            {
                return _isSearchBarVisisble;
            }
            set
            {
                _isSearchBarVisisble = value;
                OnPropertyChanged(nameof(IsSearchBarVisible));
            }
        }
        public IProductApi ApiHelper()
        {
            return RestService.For<IProductApi>(App.ApiUrl);
        }
        private bool _showCarousel = true;
        public bool ShowCarousel
        {
            get
            {
                return _showCarousel;
            }
            set
            {
                _showCarousel = value;
                OnPropertyChanged(nameof(ShowCarousel));
            }
        }
    }
}
