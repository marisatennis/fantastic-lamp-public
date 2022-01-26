using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasticLamp.Models;
using FantasticLamp.Storage;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FantasticLamp.Services
{
    public class PlayLogger
    {
        RawPlayStore rawPlayStore => DependencyService.Get<RawPlayStore>();

        PlayStore playStore => DependencyService.Get<PlayStore>();
        LocationStore locationStore => DependencyService.Get<LocationStore>();


        MoodLogStore moodLogStore => DependencyService.Get<MoodLogStore>();

        LocationMapper locationMapper => DependencyService.Get<LocationMapper>();
        PlayPositionCalculator playPositionCalculator => DependencyService.Get<PlayPositionCalculator>();

        public async void Log(RawPlay rawPlay)
        {
            await rawPlayStore.AddRawPlayAsync(rawPlay);
            SumariseNewRawPlays();
        }

        public async void SumariseNewRawPlays()
        {
            var rawPlays = await this.rawPlayStore.GetUnprocessedPlaysAsync();
            var groupedRawPlays = GroupRawPlays(rawPlays);
            foreach (var group in groupedRawPlays)
            {
                var play = await CreatePlayFromRawPlays(group);
                var mood = await moodLogStore.GetMoodAtDateTimeAsync(play.DateTime);
                if (mood != null)
                {
                    play.Mood = mood.Id;
                }
                await this.playStore.AddPlayAsync(play);
                foreach (var rawPlay in group)
                {
                    rawPlay.Play = play.Id;
                    await this.rawPlayStore.UpdatePlayAsync(rawPlay);
                }
            }
        }

        public async Task<Play> CreatePlayFromRawPlays(IEnumerable<RawPlay> rawPlays)
        {
            if (rawPlays.Count() == 0 )
            {

                throw new ArgumentException("No raw plays were provided");
            }
            Play play = Play.fromRawPlay(rawPlays.First());
            //If the latitude and longitude for a given play has not varied then get location from median lat and long
            var getPosition = playPositionCalculator.GetMedianPositionFromRawPlays(rawPlays);
            if (getPosition != null)
            {
                Position position = (Position)getPosition;
                play.Lat = position.Latitude;
                play.Long = position.Longitude;
                var location = await locationMapper.GetLocationFromLatLong(play.Lat, play.Long);
                if (location != null)
                {
                    play.Location = location.Id;
                }
            }// if the latitude and longitude have changed greatly then try guessing what moving activity it is with the speed
            else
            {
                var speed = GetSpeedFromRawPlays(rawPlays);
                var location = await GetLocationFromSpeedAsync(speed);
                if (location != null)
                {
                    play.Location = location.Id;
                }

            }
            return play;
        }


        public bool IsNewPlay(RawPlay rawPlay, RawPlay previousRawPlay)
        {
            if (previousRawPlay.Song != rawPlay.Song)
            {
                return true;
            }
            if (previousRawPlay.Artist != rawPlay.Artist)
            {
                return true;
            }
            if (previousRawPlay.Album != rawPlay.Album)
            {
                return true;
            }
            if (previousRawPlay.Position > rawPlay.Position)
            {
                return true;
            }
            return false;
        }
        public IEnumerable<IEnumerable<RawPlay>> GroupRawPlays(IEnumerable<RawPlay> rawPlays)
        {
            var groups = new List<List<RawPlay>>();
            var group = new List<RawPlay>();

            RawPlay previousRawPlay = null;
            foreach (var rawPlay in rawPlays)
            {
                if (previousRawPlay == null)
                {
                    group.Add(rawPlay);
                    previousRawPlay = rawPlay;
                    continue;
                }
                if (IsNewPlay(rawPlay, previousRawPlay))
                {
                    groups.Add(group);
                    group = new List<RawPlay>();
                }
                group.Add(rawPlay);
                previousRawPlay = rawPlay;
            }

            return groups;
        }
        public double GetSpeedFromRawPlays(IEnumerable<RawPlay> rawPlays)
        {
            Position minPosition = new Position((from rawplay in rawPlays select rawplay.Lat).Min(), (from rawplay in rawPlays select rawplay.Long).Min());
            Position maxPosition = new Position((from rawplay in rawPlays select rawplay.Lat).Max(), (from rawplay in rawPlays select rawplay.Long).Max());

            Distance distance = Distance.BetweenPositions(minPosition, maxPosition);

            DateTime minDateTime = (from rawplay in rawPlays select rawplay.DateTime).Min();
            DateTime maxDateTime = (from rawplay in rawPlays select rawplay.DateTime).Max();

            double time = maxDateTime.Subtract(minDateTime).TotalSeconds;
            double speed = distance.Meters / time;
            return speed;
        }

        public async Task<Location> GetLocationFromSpeedAsync(double speed)
        {
            if (speed <= 1.43 )
            {
                return await locationStore.GetLocationByNameAsync("Walking");
            }
            if (speed <= 4 )
            {
                return await locationStore.GetLocationByNameAsync("Running");
            }
            if (speed <= 8)
            {
                return await locationStore.GetLocationByNameAsync("Cycling");
            }
            if (speed <= 35)
            {
                return await locationStore.GetLocationByNameAsync("Car");
            }
            return null;
        }
    }
}