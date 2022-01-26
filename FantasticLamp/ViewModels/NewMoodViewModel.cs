using FantasticLamp.Models;
using FantasticLamp.Storage;
using System;
using Xamarin.Forms;

namespace FantasticLamp.ViewModels
{
    public class NewMoodViewModel : BaseViewModel
    {
        MoodStore MoodStore => DependencyService.Get<MoodStore>();
        private string name;
        private string emoji;

        public NewMoodViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(name)
                && !String.IsNullOrWhiteSpace(emoji);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Emoji
        {
            get => emoji;
            set => SetProperty(ref emoji, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Mood newMood = new Mood()
            {
                Name = Name,
                Emoji = Emoji
            };

            await MoodStore.AddMoodAsync(newMood);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
