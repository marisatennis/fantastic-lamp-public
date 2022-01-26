using System;
using Xamarin.Forms;
using FantasticLamp.ViewModels;
using Microcharts;
using SkiaSharp;
using FantasticLamp.Services.Filters;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;

namespace FantasticLamp.Views
{
    public partial class ReportsPage : ContentPage
    {
        ReportsViewModel _viewModel;
        Button categoryButton { get; set; }
        Button timeButton { get; set; }
        Button selectedButton { get; set; }
        FilterOption currentYear { get; set; }

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

        public ReportsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ReportsViewModel();

            categories = new Collection<ICategoryFilter>();
            this.categories.Add(new YearCategory());
            this.categories.Add(new WeekDayCategory());
            this.categories.Add(new LocationCategory());
            this.categories.Add(new MoodCategory());
            this.categories.Add(new ArtistCategory());
            this.categories.Add(new AlbumCategory());
            this.categories.Add(new SongCategory());

            this.selectedOptions = new ObservableCollection<FilterOption>();
            this.selectedOptions.CollectionChanged += this.FilterOptionChanged;
            this.selectedOptions.CollectionChanged += this.CategoryFilterOptionsChanged;
            this.selectedOptions.CollectionChanged += this.GetChart;
            ShowEmptyView();

            currentCategory = this.categories[0];

            //Select Month Category
            categoryButton = categoriesStacklayout.Children.OfType<Button>().Where(button => button.Text == "Time").FirstOrDefault();
            VisualStateManager.GoToState(categoryButton, "Selected");
            timeButton = timeStackLayout.Children.OfType<Button>().Where(button => button.Text == "Months").FirstOrDefault();
            VisualStateManager.GoToState(timeButton, "SelectedFilter");
            timeStackLayout.IsVisible = true;
            selectedButton = timeButton;

            //Auto Filter for this year
            currentYear = new FilterOption();
            currentYear.Id = DateTime.Now.Year;
            currentYear.Name = DateTime.Now.Year.ToString();
            currentYear.Category = "Years";
            selectedOptions.Add(currentYear);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
        private void Category_Button_Clicked(object sender, EventArgs e)
        {
            if (categoryButton != null)
            {
                VisualStateManager.GoToState(categoryButton, "Normal");
            }
            categoryButton = (sender as Button);
            VisualStateManager.GoToState(categoryButton, "Selected");
            if (categoryButton.Text == "Time")
            {
                selectedButton = timeButton;
                timeStackLayout.IsVisible = true;
                _viewModel.State = selectedButton.Text;
                _viewModel.LoadReportsCommand.Execute(null);
                ShowEmptyView();
            }
            else
            {
                selectedButton = categoryButton;
                timeStackLayout.IsVisible = false;
            }

            GetChart(null,null);
        }
        private void Time_Button_Clicked(object sender, EventArgs e)
        {
            if (timeButton != null)
            {
                VisualStateManager.GoToState(timeButton, "Normal");
            }
            timeButton = (sender as Button);
            VisualStateManager.GoToState(timeButton, "SelectedFilter");
            selectedButton = timeButton;

            GetChart(null, null);
        }

        private void GetChart(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (selectedButton.Text == "Hours")
            {
                var chart = new RadarChart() { Entries = _viewModel.Entries1 };
                chartView.Chart = chart;
            }
            else
            {
                var chart = new BarChart() { Entries = _viewModel.Entries1 };
                chartView.Chart = chart;
            }
            chartView.Chart.BackgroundColor = SKColor.Parse("#052b42");
            chartView.Chart.LabelTextSize = 30;
            chartView.Chart.LabelColor = SKColor.Parse("#fffc80");
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
            _viewModel.LoadReportsCommand.Execute(null);
            ShowEmptyView();
        }

        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return; // has been set to null, do not 'process' tapped event
            var option = (FilterOption)e.SelectedItem;
            this.selectedOptions.Add(option);

            ICategoryFilter category = this.GetCategoryFilter(option.Category);
            category.SelectOption(_viewModel.PlayGroupSelection, option);
            _viewModel.LoadReportsCommand.Execute(null);
            ShowEmptyView();
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
            _viewModel.LoadReportsCommand.Execute(null);
            ShowEmptyView();
        }

        private bool ClearAllFilters()
        {
            selectedOptions.Clear();
            foreach (ICategoryFilter category in this.categories)
            {
                category.ClearSelection(_viewModel.PlayGroupSelection);
            }
            _viewModel.LoadReportsCommand.Execute(null);
            ShowEmptyView();
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
        private void ShowEmptyView()
        {
            if (_viewModel.Entries1.Count() > 0)
            {
                emptyViewStacklayout.IsVisible = false;
                chartView.IsVisible = true;
            }
            else
            {
                emptyViewStacklayout.IsVisible = true;
                chartView.IsVisible = false;
            }
        }
    }
}
