using Shopel.DataAccess;
using Shopel.Models;
using Shopel.ToastMessaging;
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
    public class CartViewModel : BaseViewModel
    {
        public ObservableCollection<ProductModel> CartItems { get; set; } = new ObservableCollection<ProductModel>();
        private bool _cartHasItem;
        public bool CartHasItem
        {
            get
            {
                return _cartHasItem;
            }
            set
            {
                _cartHasItem = value;
                OnPropertyChanged(nameof(CartHasItem));
            }
        }
        private Command _removeFromCartCommand;
        public ICommand RemoveFromCartCommand
        {
            get => _removeFromCartCommand ?? (_removeFromCartCommand = new Command(async (product) =>
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
                    Price = p.Price,
                    Shipping = p.Shipping,
                    Type = p.Type,
                    Upc = p.Upc
                };
                await db.DeleteCartItem(cartItem);
                CartItems.Remove(p);
                TotalCartItems--;
                CartHasItem = (CartItems.Any() == false) ? false : true;
                toast.ShortAlert("item removed from cart");
            }));
        }
        private Command _checkOutCommand;
        public ICommand CheckOutCommand
        {
            get => _checkOutCommand ?? (_checkOutCommand = new Command(async () =>
            {
                double total = CartItems.Select(item => item.Price).Sum();
                await AlertAsync($"Your total is {total}", "Thank you for shopping with us :)");
            }));
        }
        private Command _loadCartItemsCommand;
        public ICommand LoadCartItemsCommand
        {
            get => _loadCartItemsCommand ?? (_loadCartItemsCommand = new Command(async () =>
            {
                IsBusy = true;
                await LoadCartItems();
                IsBusy = false;
            }));
        }
        private async Task LoadCartItems()
        {
            try
            {
                if (CartItems.Any() == false)
                {
                    TotalCartItems = await GetCartItemsCount();
                    var db = DependencyService.Get<ISqlDataAccess>();
                    var products = await db.GetCartItemsAsync();
                    if (products.Any() == false)
                    {
                        CartHasItem = false;
                    }
                    else
                    {
                        products.ForEach(p => CartItems.Add(new ProductModel
                        {
                            Id = p.Id,
                            Description = p.Description,
                            Image = p.Image,
                            Model = p.Model,
                            Name = p.Name,
                            Price = p.Price,
                            Quantity = p.Quantity,
                            Shipping = p.Shipping,
                            Type = p.Type,
                            Upc = p.Upc
                        }));
                        CartHasItem = true;
                    }
                }
            }
            catch
            {
                await ExceptionHandler();
            }
        }
    }
}
