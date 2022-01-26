
using System.Collections.Generic;

namespace FantasticLamp.Models
{
    public class ChartColour
    {
        public int Id { get; set; }
        public string Colour { get; set; }

        public static List<ChartColour> chartColours = new List<ChartColour>()
            {
                { new ChartColour { Id = 1, Colour = "#fafafa"} },
                { new ChartColour { Id = 2, Colour = "#c83264"} },
                { new ChartColour { Id = 3, Colour = "#96c8c8"} },
                { new ChartColour { Id = 4, Colour = "#h64c896"} },
                { new ChartColour { Id = 5, Colour = "#fac864"} }
            };
    }
    
}