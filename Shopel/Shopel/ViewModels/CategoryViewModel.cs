using Refit;
using Shopel.ApiHelper;
using Shopel.ApiHelper.Interfaces;
using Shopel.Domains;
using Shopel.Models;
using Shopel.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Shopel.ViewModels
{
    public class CategoryViewModel : BaseViewModel
    {
        private Command _loadMoreCategoriesCommand;
        public ICommand LoadMoreCategoriesCommand
        {
            get => _loadMoreCategoriesCommand ?? (_loadMoreCategoriesCommand = new Command(async () =>
            {
                IsBusy = true;
                await LoadMoreCategories();
                IsBusy = false;
                Skip++;
            }, () => EndReached == false));
        }
        private Command _productsPageCommand;
        public ICommand ProductsPageCommand
        {
            get => _productsPageCommand ?? (_productsPageCommand = new Command(async (category) =>
            {
                var c = category as CategoryModel;
                await PushAsync(new ProductsPage((category as CategoryModel)));
            }));
        }
        private Command _refreshCommand;
        public ICommand RefreshCommand
        {
            get => _refreshCommand ?? (_refreshCommand = new Command(async () =>
            {
                EndReached = false;
                IsRefreshing = true;
                await LoadMoreCategories();
                IsRefreshing = false;
            }));
        }
        private Command _searchCategoryCommand;
        public ICommand SearchCategoryCommand
        {
            get => _searchCategoryCommand ?? (_searchCategoryCommand = new Command(() =>
            {
                Categories.Clear();
                if (EndReached)
                {
                    EndReached = false;
                    LoadMoreCategoriesCommand.Execute(null);
                }
            }));
        }
        public ObservableCollection<CategoryModel> Categories { get; set; } = new ObservableCollection<CategoryModel>();
        public async Task LoadMoreCategories()
        {
            if (IsRefreshing)
            {
                EndReached = false;
                IsBusy = false;
                Categories.Clear();
                Skip = RandomSkip(1, 500);
            }
            if (string.IsNullOrWhiteSpace(SearchTerm) == false)
            {
                await LoadCategoriesBySearchTerm();
            }
            else
            {
                await LoadCategories();
            }
        }
        public async Task LoadCategories()
        {
            try
            {
                var api = ApiHelper();
                var content = (await api.GetCategoriesAsync(Skip)).Content;
                MapToCategoryModel(content);
            }
            catch
            {
                await ExceptionHandler();
            }
        }
        public async Task LoadCategoriesBySearchTerm()
        {
            try
            {
                var api = ApiHelper();
                var content = (await api.GetCategoriesBySearchTerm(SearchTerm)).Content;
                MapToCategoryModel(content);
            }
            catch
            {
                await ExceptionHandler();
            }
        }
        private void MapToCategoryModel(Response<Category> content)
        {
            if (content.Total > 0)
            {
                var categories = content.Data;
                categories.ForEach(c => Categories.Add(new CategoryModel
                {
                    Id = c.Id,
                    Name = c.Name
                }));
            }
            else
            {
                EndReached = true;
            }
        }
    }
}
