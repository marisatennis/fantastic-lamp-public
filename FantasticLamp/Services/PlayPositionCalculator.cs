using System;
using System.Collections.Generic;
using System.Linq;
using FantasticLamp.Models;
using Xamarin.Forms.Maps;

namespace FantasticLamp.Services
{
    public class PlayPositionCalculator
    {
        public Position? GetMedianPositionFromRawPlays(IEnumerable<RawPlay> rawPlays)
        {
            double GetMedian(List<double> list )
            {
                decimal n = list.Count/2;
                int index = (int)(Math.Ceiling(n));
                list.Sort();
                double value = list[index];
                return value;
            }
            List<List<double>> LatsLongs = new List<List<double>>();
            LatsLongs.Add((from rawplay in rawPlays select rawplay.Lat).ToList());
            LatsLongs.Add((from rawplay in rawPlays select rawplay.Long).ToList());

            //if the raw plays are too physically far apart then dont map to a lat and long
            if (LatsLongs[0].Max() > (LatsLongs[0].Min() + 0.0002) || LatsLongs[1].Max() > (LatsLongs[1].Min() + 0.0002)) 
            {
                return null;
            }
            Position position = new Position(GetMedian(LatsLongs[0]), GetMedian(LatsLongs[1]));
            return position;
        }
    }
}
