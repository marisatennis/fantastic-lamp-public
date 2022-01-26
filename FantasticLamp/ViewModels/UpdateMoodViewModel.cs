using FantasticLamp.Models;
using FantasticLamp.Storage;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace FantasticLamp.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class UpdateMoodViewModel : BaseViewModel
    {
        MoodStore MoodStore => DependencyService.Get<MoodStore>();
        private Mood mood;
        private string itemId;
        private string name;
        private string emoji;

        public UpdateMoodViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }
        public string Id { get; set; }

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

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            mood.Name = this.name;
            mood.Emoji = this.emoji;
            await MoodStore.UpdateMoodAsync(mood);
            await Shell.Current.GoToAsync("..");
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(name)
                && !String.IsNullOrWhiteSpace(emoji);
        }
        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await MoodStore.GetMoodAsync(int.Parse(itemId));
                this.mood = item;
                this.Id = item.Id.ToString();
                this.Name = item.Name;
                this.Emoji = item.Emoji;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}