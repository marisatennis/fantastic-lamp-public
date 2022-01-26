using SQLite;
using FantasticLamp.Models;
using System.Threading.Tasks;
using Xamarin.Forms;
using Image = FantasticLamp.Models.Image;
using System.Net.Http;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using FantasticLamp.Data;
using System.Text.Json;
using System.Web;

namespace FantasticLamp.Storage
{
    class ImageStore
    {
        private const string CLIENT_ID = "dff41e3c09484148b1b28bbf4d317df1";
        private const string CLIENT_SECRET = "1fe71e1ce35d45ceba8afbf1a1d71830";

        static readonly HttpClient client = new HttpClient();
        static bool authenticated = false;

        SQLiteAsyncConnection Database => DependencyService.Get<DatabaseFactory>().GetDatabase();

        private async Task<Image> GetImage(string type, PlayGroupByArtist playGroup)
        {
            var image = await Database.Table<Image>().Where(row => row.Name == playGroup.Name && row.Type == type).FirstOrDefaultAsync();
            if (image == null)
            {
                string url = await this.downloadImage(type, playGroup);
                image = new Image();
                image.Type = type;
                image.Name = playGroup.Name;
                image.Url = url;
                await Database.InsertAsync(image);
            }
            return image;
        }

        public async Task<Image> GetArtistImage(PlayGroupByArtist playGroup)
        {
            return await this.GetImage("artist", playGroup);
        }
        public async Task<Image> GetAlbumImage(PlayGroupByArtist playGroup)
        {
            return await this.GetImage("album", playGroup);
        }
        public async Task<Image> GetSongImage(PlayGroupByArtist playGroup)
        {
            return await this.GetImage("track", playGroup);
        }
        async Task<string> downloadImage(string type, PlayGroupByArtist playGroup)
        {
            if (!authenticated)
            {
                await this.authenticate();
            }

            string url;
            if (type == "artist")
            {
                url = "https://api.spotify.com/v1/search?type=" + type + "&q=" + HttpUtility.UrlEncode(playGroup.Name);
            }
            else
            {
                url = "https://api.spotify.com/v1/search?type=" + type + "&q=artist:" + HttpUtility.UrlEncode(playGroup.Artist) + HttpUtility.UrlEncode(" ") + type + ":" + HttpUtility.UrlEncode(playGroup.Name);
            }

            string responseBody = await ImageStore.client.GetStringAsync(url);
            SpotifySearchResults results = JsonSerializer.Deserialize<SpotifySearchResults>(responseBody);

            SpotifyItems items;
            if (type == "artist")
            {
                items = results.artists;
            }
            else if (type == "album")
            {
                items = results.albums;
            }
            else if (type == "track")
            {
                foreach (SpotifyItem item in results.tracks.items)
                {
                    foreach (SpotifyImage image in item.album.images)
                    {
                        return image.url;
                    }
                }
                return "";
            }
            else
            {
                throw new Exception("Unexpected image type: " + type);
            }

            foreach (SpotifyItem item in items.items)
            {
                foreach (SpotifyImage image in item.images)
                {
                    return image.url;
                }
            }

            return "";
        }

        async Task<bool> authenticate()
        {
            string secret = ImageStore.CLIENT_ID + ":" + ImageStore.CLIENT_SECRET;
            string base64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(secret));
            ImageStore.client.DefaultRequestHeaders.Add("Authorization", "Basic " + base64);

            var body = new Collection<KeyValuePair<string, string>>();
            body.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            HttpContent content = new FormUrlEncodedContent(body);
            HttpResponseMessage response = await ImageStore.client.PostAsync("https://accounts.spotify.com/api/token", content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            SpotifyAuthentication authentication = JsonSerializer.Deserialize<SpotifyAuthentication>(responseBody);

            ImageStore.client.DefaultRequestHeaders.Clear();
            ImageStore.client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authentication.access_token);
            authenticated = true;
            return true;
        }
    }
}
