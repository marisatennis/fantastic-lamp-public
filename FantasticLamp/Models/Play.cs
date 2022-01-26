using SQLite;
using System;
using Xamarin.Forms.Maps;

namespace FantasticLamp.Models
{
    public class Play
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Song { get; set; }
        public DateTime DateTime{ get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public int Location { get; set; }
        public int Mood { get; set; }
        [Ignore]
        public Position Pin { get { return new Position(Lat, Long); } }

        static public Play fromRawPlay(RawPlay rawPlay)
        {
            var play = new Play();
            play.Artist = rawPlay.Artist;
            play.Album = rawPlay.Album;
            play.Song = rawPlay.Song;
            play.DateTime = rawPlay.DateTime;
            return play;
        }

    }

}