using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using FantasticLamp.Services.Filters;
using FantasticLamp.ViewModels;
using Xamarin.Forms;

namespace FantasticLamp.Views
{
    public partial class TopPage : ContentPage
    {

        TopViewModel _viewModel;
        Button Button { get; set; }

        List<FilterOption> SearchName { get; set; }

        Collection<ICategoryFilter> categories { get; set; }

        ObservableCollection<FilterOption> selectedOptions;

        ICategoryFilter currentCategoryValue;
        ICategoryFilter currentCategory
        {
            get
            {
                return this.currentCategoryValue;
            }
            set
            {
                this.currentCategoryValue = value;
                AddFilterCategoriesButtons();
                this.CategoryFilterOptionsChanged(null, null);
            }
        }

        public TopPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new TopViewModel();

            //Select Artist Category
            Button = categoriesStacklayout.Children.OfType<Button>().Where(button => button.Text == "Artists").FirstOrDefault();
            VisualStateManager.GoToState(Button, "Selected");


            categories = new Collection<ICategoryFilter>();
            this.categories.Add(new LocationCategory());
            this.categories.Add(new MoodCategory());
            this.categories.Add(new ArtistCategory());
            this.categories.Add(new AlbumCategory());
            this.categories.Add(new SongCategory());

            this.selectedOptions = new ObservableCollection<FilterOption>();
            this.selectedOptions.CollectionChanged += this.FilterOptionChanged;
            this.selectedOptions.CollectionChanged += this.CategoryFilterOptionsChanged;

            currentCategory = this.categories[0];
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void Category_Button_Clicked(object sender, EventArgs e)
        {
            if (Button != null)
            {
                VisualStateManager.GoToState(Button, "Normal");
            }
            Button = (sender as Button);
            VisualStateManager.GoToState(Button, "Selected");
        }
        void AddFilterCategoriesButtons()
        {
            filterCategoriesStackLayout.Children.Clear();

            foreach (ICategoryFilter category in this.categories)
            {
                var filterButton = new Button();
                filterButton.Text = category.GetName();
                filterButton.Clicked += Filter_Category_Button_ClickedAsync;
                filterButton.Style = (Style)Application.Current.Resources["FilterOptionButton"];
                if (category == currentCategory)
                {
                    VisualStateManager.GoToState(filterButton, "Normal");
                }
                else
                {
                    VisualStateManager.GoToState(filterButton, "UnSelectedCategory");
                }
                filterCategoriesStackLayout.Children.Add(filterButton);
            }
        }

        async void Filter_Category_Button_ClickedAsync(object sender, EventArgs e)
        {
            var button = (sender as Button);
            this.currentCategory = GetCategoryFilter(button.Text);
        }

        private ICategoryFilter GetCategoryFilter(string name)
        {
            foreach (ICategoryFilter category in this.categories)
            {
                if (category.GetName() == name)
                {
                    return category;
                }
            }

            throw new Exception("Unable to find category filter for '" + name + "'");
        }

  
        private void FilterOptionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            selectedfiltersStackLayout.Children.Clear();

            var clearButton = new Button();
            clearButton.Text = "Clear";
            clearButton.Style = (Style)Application.Current.Resources["FilterButton"];
            clearButton.Clicked += Clear_Button_Clicked;
            selectedfiltersStackLayout.Children.Add(clearButton);

            foreach (FilterOption categoryFilterOption in this.selectedOptions)
            {
                Button button = categoryFilterOption.getButton();
                button.Style = (Style)Application.Current.Resources["FilterButton"];
                button.Clicked += Remove_Filter_button_Clicked;
                selectedfiltersStackLayout.Children.Add(button);
            }

            selectedfiltersScrollView.IsVisible = (this.selectedOptions.Count > 0);
        }

        private void Filter_Button_Clicked(object sender, EventArgs e)
        {
            var button = (sender as Button);
            var option = FilterOption.fromButton(button);
            this.selectedOptions.Add(option);

            ICategoryFilter category = this.GetCategoryFilter(option.Category);
            category.SelectOption(_viewModel.PlayGroupSelection, option);
            _viewModel.LoadTopCommand.Execute(null);
        }

        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return; // has been set to null, do not 'process' tapped event
            var option = (FilterOption)e.SelectedItem;
            this.selectedOptions.Add(option);

            ICategoryFilter category = this.GetCategoryFilter(option.Category);
            category.SelectOption(_viewModel.PlayGroupSelection, option);
            _viewModel.LoadTopCommand.Execute(null);
        }

        private async void CategoryFilterOptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var button in filterOptionsStackLayout.Children.OfType<Button>().ToList())
            {
                filterOptionsStackLayout.Children.Remove(button);
            }

            var options = await this.currentCategory.getOptions();

            var counter = 0;
            foreach (FilterOption categoryFilterOption in options)
            {
                counter += 1;
                if (counter > 10)
                {
                    break;
                }
            }

            if (counter < 10)
            {
                searchStacklayout.IsVisible = false;

                foreach (FilterOption categoryFilterOption in options)
                {
                    Button button = categoryFilterOption.getButton();
                    button.Style = (Style)Application.Current.Resources["FilterOptionButton"];

                    // Don't offer options that have already been filtered
                    if (this.selectedOptions.Contains(categoryFilterOption))
                    {
                        VisualStateManager.GoToState(button, "SelectedFilter");
                    }
                    else
                    {
                        VisualStateManager.GoToState(button, "Normal");
                        button.Clicked += Filter_Button_Clicked;
                    }

                    filterOptionsStackLayout.Children.Add(button);
                }
            }
            else
            {
                searchStacklayout.IsVisible = true;
                SearchName = new List<FilterOption>();

                foreach (FilterOption categoryFilterOption in options)
                {
                    if (selectedOptions.Contains(categoryFilterOption)) continue;
                    SearchName.Add(categoryFilterOption);

                }
                searchResults.ItemsSource = SearchName;
            }
        }
        void OnSearchButtonPressed(object sender, EventArgs e)
        {
            var item = searchBar.Text.ToLower();
            searchResults.ItemsSource = SearchName.Where(p => p.Name.ToLower().Contains(item));
        }

        private void Remove_Filter_button_Clicked(object sender, EventArgs e)
        {
            var button = (sender as Button);
            var option = FilterOption.fromButton(button);
            this.selectedOptions.Remove(option);

            ICategoryFilter category = this.GetCategoryFilter(button.ClassId);
            category.RemoveOption(_viewModel.PlayGroupSelection, option);
            _viewModel.LoadTopCommand.Execute(null);
        }

        private bool ClearAllFilters()
        {
            selectedOptions.Clear();
            foreach (ICategoryFilter category in this.categories)
            {
                category.ClearSelection(_viewModel.PlayGroupSelection);
            }
            _viewModel.LoadTopCommand.Execute(null);
            return true;
        }
        private void Clear_Button_Clicked(object sender, EventArgs e)
        {
            ClearAllFilters();
        }

        private void ShowFilters(object sender, EventArgs e)
        {
            filterCategoriesScrollView.IsVisible = !filterCategoriesScrollView.IsVisible;
            filterOptionsScrollView.IsVisible = !filterOptionsScrollView.IsVisible;
        }
    }
}