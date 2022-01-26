using SQLite;
using System;
using Xamarin.Forms.Maps;

namespace FantasticLamp.Models
{
    public class RawPlay
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Song { get; set; }
        public DateTime DateTime{ get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public int Position { get; set; }
        public int Play { get; set; }
    }
}