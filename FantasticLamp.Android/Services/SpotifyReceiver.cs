using System;
using Xamarin.Essentials;
using Android.App;
using Android.Content;
using FantasticLamp.Models;
using FantasticLamp.Storage;
using FantasticLamp.Services;
using Xamarin.Forms;

namespace FantasticLamp.Droid.Services
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { "com.spotify.music.metadatachanged" })]
    public class SpotifyReceiver : BroadcastReceiver
    {
        RawPlayStore RawPlayStore => DependencyService.Get<RawPlayStore>();
        PlayLogger playLogger = DependencyService.Get<PlayLogger>();

        public override async void OnReceive(Context context, Intent intent)
        {
            long timeSentInMs = intent.GetLongExtra("timeSent", 0L);

            if (intent.Action == "com.spotify.music.metadatachanged")
            {
                var location = await Geolocation.GetLocationAsync();

                var rawPlay = new RawPlay();

                if (location != null)
                {
                    rawPlay.Lat = Math.Round(location.Latitude,7);
                    rawPlay.Long = Math.Round(location.Longitude,7);
                }

                rawPlay.Artist = intent.GetStringExtra("artist");
                rawPlay.Album = intent.GetStringExtra("album");
                rawPlay.Song = intent.GetStringExtra("track");
                rawPlay.Position = intent.GetIntExtra("length", 0);
                rawPlay.DateTime = DateTimeOffset.FromUnixTimeMilliseconds(timeSentInMs).DateTime;
                playLogger.Log(rawPlay);
            }
        }
    }
}