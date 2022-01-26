using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using FantasticLamp.Services;
using FantasticLamp.Storage;
using System;

namespace FantasticLamp.ViewModels
{
    public class SearchBarViewModel : INotifyPropertyChanged
    {
        PlayStore PlayStore => DependencyService.Get<PlayStore>();

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ICommand PerformSearch => new Command<string>(async (string query) =>
        {
            SearchResults = new List<string>();
            SearchResults = await PlayStore.GetArtistsSearch(query);
            NotifyPropertyChanged();
        });
        public List<string> SearchResults { get; set; }
    }
}
