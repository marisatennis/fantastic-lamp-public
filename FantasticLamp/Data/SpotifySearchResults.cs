using System.Collections.Generic;

namespace FantasticLamp.Data
{
    class SpotifySearchResults
    {
        public SpotifyItems artists { get; set; }
        public SpotifyItems albums { get; set; }
        public SpotifyItems tracks { get; set; }
    }

    class SpotifyItems
    {
        public IEnumerable<SpotifyItem> items { get; set; }
    }

    class SpotifyItem
    {
        public SpotifyItem album { get; set; }
        public IEnumerable<SpotifyImage> images { get; set; }
    }

    class SpotifyImage
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}