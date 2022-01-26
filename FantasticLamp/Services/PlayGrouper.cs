using FantasticLamp.Models;
using FantasticLamp.Storage;
using SQLite;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace FantasticLamp.Services
{
    public class PlayGrouper
    {
        SQLiteAsyncConnection Database => DependencyService.Get<DatabaseFactory>().GetDatabase();

        ImageStore ImageStore => DependencyService.Get<ImageStore>();

        public async Task<IEnumerable<PlayGroup>> getPlaysByArtist(PlayGroupSelection selection)
        {
            string query = "SELECT Artist AS Name, COUNT(*) AS Plays, Artist FROM Play " + selection.ToString() + " GROUP BY Artist";

            var playGroupByArtists = await Database.QueryAsync<PlayGroupByArtist>(query);
            List<PlayGroup> playGroups = new List<PlayGroup>();
            foreach (var playGroupByArtist in playGroupByArtists)
            {
                var playGroup = new PlayGroup();
                playGroup.Name = playGroupByArtist.Name;
                playGroup.Plays = playGroupByArtist.Plays;
                playGroup.Image = await ImageStore.GetArtistImage(playGroupByArtist);
                playGroups.Add(playGroup);
            }
            return playGroups;
        }

        public async Task<IEnumerable<PlayGroup>> getPlaysBySong(PlayGroupSelection selection)
        {
            string query = "SELECT Song AS Name, COUNT(*) AS Plays, Artist FROM Play " + selection.ToString() + " GROUP BY Song, Artist";

            var playGroupByArtists = await Database.QueryAsync<PlayGroupByArtist>(query);
            List<PlayGroup> playGroups = new List<PlayGroup>();
            foreach (var playGroupByArtist in playGroupByArtists)
            {
                var playGroup = new PlayGroup();
                playGroup.Name = playGroupByArtist.Name;
                playGroup.Plays = playGroupByArtist.Plays;
                playGroup.Image = await ImageStore.GetSongImage(playGroupByArtist);
                playGroups.Add(playGroup);
            }
            return playGroups;
        }
        public async Task<IEnumerable<PlayGroup>> getPlaysByAlbum(PlayGroupSelection selection)
        {
            string query = "SELECT Album AS Name, COUNT(*) AS Plays, Artist FROM Play " + selection.ToString() + " GROUP BY Album, Artist";
            var playGroupByArtists = await Database.QueryAsync<PlayGroupByArtist>(query);
            List<PlayGroup> playGroups = new List<PlayGroup>();
            foreach (var playGroupByArtist in playGroupByArtists)
            {
                var playGroup = new PlayGroup();
                playGroup.Name = playGroupByArtist.Name;
                playGroup.Plays = playGroupByArtist.Plays;
                playGroup.Image = await ImageStore.GetAlbumImage(playGroupByArtist);
                playGroups.Add(playGroup);
            }
            return playGroups;
        }

        public async Task<IEnumerable<PlayGroup>> getPlaysByLocation(PlayGroupSelection selection)
        {
            string query = "SELECT b.Name, COUNT(*) AS Plays FROM Play a JOIN Location b ON a.Location = b.Id " + selection.ToString() + " GROUP BY b.Name";
            return await Database.QueryAsync<PlayGroup>(query);
        }
        public async Task<IEnumerable<PlayGroup>> getPlaysByMood(PlayGroupSelection selection)
        {
            string query = "SELECT b.Name, COUNT(*) AS Plays FROM Play a JOIN Mood b ON a.Mood = b.Id " + selection.ToString() + " GROUP BY b.Name";
            return await Database.QueryAsync<PlayGroup>(query);
        }

        public async Task<IEnumerable<PlayGroup>> getPlaysByYear(PlayGroupSelection selection)
        {
            string query = "SELECT strftime('%Y',date(DateTime)) AS Name, COUNT(*) AS Plays FROM Play " + selection.ToString() + " GROUP BY strftime('%Y',date(DateTime))";
            return await Database.QueryAsync<PlayGroup>(query);
        }
        public async Task<IEnumerable<PlayGroup>> getPlaysByMonth(PlayGroupSelection selection)
        {
            string query = "SELECT strftime('%m',date(DateTime)) AS Name, COUNT(*) AS Plays FROM Play " + selection.ToString() + " GROUP BY strftime('%m',date(DateTime))";
            return await Database.QueryAsync<PlayGroup>(query);
        }
        public async Task<IEnumerable<PlayGroup>> getPlaysByWeekDay(PlayGroupSelection selection)
        {
            string query = "SELECT strftime('%w',date(DateTime)) AS Name, COUNT(*) AS Plays FROM Play  " + selection.ToString() + " GROUP BY strftime('%w',date(DateTime))";
            return await Database.QueryAsync<PlayGroup>(query);
        }
        public async Task<IEnumerable<PlayGroup>> getPlaysByTime(PlayGroupSelection selection)
        {
            string query = "SELECT strftime('%H',time(DateTime)) AS Name, COUNT(*) AS Plays FROM Play " + selection.ToString() + " GROUP BY strftime('%H',time(DateTime))";
            return await Database.QueryAsync<PlayGroup>(query);
        }
    }
}