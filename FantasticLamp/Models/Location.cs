using SQLite;
using Xamarin.Forms.Maps;

namespace FantasticLamp.Models
{
    public class Location
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Emoji { get; set; }
        public double MaxLat { get; set; }
        public double MinLat { get; set; }
        public double MaxLong { get; set; }
        public double MinLong { get; set; }
        [Ignore]
        public Distance Radius { get { return Distance.BetweenPositions(new Position(MaxLat + 0.0001 ,MaxLong + 0.0001 ), new Position(MinLat - 0.0001, MinLong - 0.0001)); } }
        [Ignore]
        public Position Pin { get { return new Position((MaxLat+MinLat)/2, (MaxLong+MinLong)/2); } }
    }
}