using FantasticLamp.Models;
using FantasticLamp.Services;
using FantasticLamp.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FantasticLamp.ViewModels
{
    public class TopViewModel : BaseViewModel
    {
        PlayGrouper PlayGrouper => DependencyService.Get<PlayGrouper>();
        public ObservableCollection<PlayGroup> PlayGroups { get; }
        public PlayGroupSelection PlayGroupSelection { get; }
        public string State { get; set; }
        ImageStore ImageStore => DependencyService.Get<ImageStore>();
        public Command LoadTopArtistsCommand { get; }
        public Command LoadTopSongsCommand { get; }
        public Command LoadTopAlbumsCommand { get; }
        public Command LoadTopCommand { get; }

        public TopViewModel()
        {
            Title = "Top";
            PlayGroups = new ObservableCollection<PlayGroup>();
            PlayGroupSelection = new PlayGroupSelection();
            LoadTopArtistsCommand = new Command(() => ExecuteLoadTopArtistsCommandAsync());
            LoadTopSongsCommand = new Command(() => ExecuteLoadTopSongsCommandAsync());
            LoadTopAlbumsCommand = new Command(() => ExecuteLoadTopAlbumsCommandAsync());
            LoadTopCommand = new Command(() => ExecuteLoadPageCommandAsync());
        }
        async void ExecuteLoadTopArtistsCommandAsync()
        {
            IsBusy = true;

            try
            {
                PlayGroups.Clear();
                var artistPlays = await PlayGrouper.getPlaysByArtist(PlayGroupSelection);
                foreach (var artist in artistPlays.OrderByDescending(row=>row.Plays))
                {   
                    PlayGroups.Add(artist);
                }
                State = "Artists";
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
        async void ExecuteLoadTopSongsCommandAsync()
        {
            IsBusy = true;

            try
            {
                PlayGroups.Clear();
                var songPlays = await PlayGrouper.getPlaysBySong(PlayGroupSelection);
                foreach (var song in songPlays.OrderByDescending(row => row.Plays))
                {
                    PlayGroups.Add(song);
                }
                State = "Songs";
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
        async void ExecuteLoadTopAlbumsCommandAsync()
        {
            IsBusy = true;

            try
            {
                PlayGroups.Clear();
                var albumPlays = await PlayGrouper.getPlaysByAlbum(PlayGroupSelection);
                foreach (var album in albumPlays.OrderByDescending(row => row.Plays))
                {
                    PlayGroups.Add(album);
                }
                State = "Albums";
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
            ExecuteLoadTopArtistsCommandAsync();
        }
        async void ExecuteLoadPageCommandAsync()
        {
            if (State == "Artists")
            {
                ExecuteLoadTopArtistsCommandAsync();
                return;
            }
            if (State == "Albums")
            {
                ExecuteLoadTopAlbumsCommandAsync();
                return;
            }
            if (State == "Songs")
            {
                ExecuteLoadTopSongsCommandAsync();
                return;
            }

            throw new Exception("Invalid top group state: " + State);
        }
    }
}