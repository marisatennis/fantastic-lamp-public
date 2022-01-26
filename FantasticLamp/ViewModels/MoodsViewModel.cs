using FantasticLamp.Models;
using FantasticLamp.Storage;
using FantasticLamp.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FantasticLamp.ViewModels
{
    public class MoodsViewModel : BaseViewModel
    {
        MoodStore MoodStore => DependencyService.Get<MoodStore>();
        MoodLogStore MoodLogStore => DependencyService.Get<MoodLogStore>();

        public ObservableCollection<Mood> Moods { get; }
        public Command LoadMoodsCommand { get; }
        public Command AddMoodCommand { get; }
        public Command<Mood> MoodTapped { get; }
        public Command<Mood> MoodDoubleTapped { get; }

        public MoodsViewModel()
        {
            Title = "Moods";
            Moods = new ObservableCollection<Mood>();
            LoadMoodsCommand = new Command(async () => await ExecuteLoadMoodsCommand());

            MoodTapped = new Command<Mood>(OnMoodSelected);

            MoodDoubleTapped = new Command<Mood>(OnMoodEdited);

            AddMoodCommand = new Command(OnAddMood);
        }

        async Task ExecuteLoadMoodsCommand()
        {
            IsBusy = true;

            try
            {
                Moods.Clear();
                var moods = await MoodStore.GetMoodsAsync();
                foreach (var mood in moods)
                {
                    Moods.Add(mood);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        private async void OnAddMood(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewMoodPage));
        }

        async void OnMoodSelected(Mood mood)
        {
            await MoodLogStore.LogMoodAsync(mood);
        }
        async void OnMoodEdited(Mood mood)
        {
            await Shell.Current.GoToAsync($"{nameof(UpdateMoodPage)}?{nameof(UpdateMoodViewModel.ItemId)}={mood.Id}");
        }
    }
}