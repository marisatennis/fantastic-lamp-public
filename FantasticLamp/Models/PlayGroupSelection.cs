using System;
using System.Collections.Generic;
using System.Linq;

namespace FantasticLamp.Models
{
    public class PlayGroupSelection
    {

        public List<String> Years { get; set; }
        public List<String> Months { get; set; }
        public List<String> WeekDays { get; set; }
        public List<String> Hours { get; set; }
        public List<Location> Locations { get; set; }
        public List<Mood> Moods { get; set; }
        public List<string> Artists { get; set; }
        public List<string> Albums { get; set; }
        public List<string> Songs { get; set; }

        public PlayGroupSelection()
        {
            Locations = new List<Location>();
            Moods = new List<Mood>();
            Artists = new List<string>();
            Albums = new List<string>();
            Songs = new List<string>();
            Years = new List<string>();
            Months = new List<string>();
            WeekDays = new List<string>();
            Hours = new List<string>();

        }

        public override string ToString()
        {
            string where = "WHERE ";
            if (this.Years.Count > 0)
            {
                where += " strftime('%Y',date(DateTime)) IN ('" + String.Join("', '", Years) + "') AND";
            }
            if (this.Months.Count > 0)
            {
                where += " strftime('%m',date(DateTime)) IN ('" + String.Join("', '", Months) + "') AND";
            }
            if (this.WeekDays.Count > 0)
            {
                var weekDays = new List<string>();
                foreach (var weekDay in WeekDays)
                {
                    weekDays.Add(weekDay);
                }
                where += " strftime('%w',date(DateTime)) IN ('" + String.Join("', '", weekDays) + "') AND";
            }
            if (this.Hours.Count > 0)
            {
                where += " strftime('%Y',date(DateTime)) IN ('" + String.Join("', '", Hours) + "') AND";
            }
            if (this.Locations.Count > 0)
            {
                var locations = new List<string>();
                foreach (var location in Locations)
                {
                    locations.Add(location.Id.ToString());
                }
                where += " Location IN (" + String.Join(", ", locations) + ") AND";
            }
            if (this.Moods.Count > 0)
            {
                var moods = new List<string>();
                foreach (var mood in Moods)
                {
                    moods.Add(mood.Id.ToString());
                }
                where += " Mood IN (" + String.Join(", ", moods) + ") AND";
            }
            if (this.Artists.Count > 0)
            {
                where += " Artist IN ('" + String.Join("', '", Artists) + "') AND";
            }
            if (this.Albums.Count > 0)
            {
                where += " Album IN ('" + String.Join("', '", Albums) + "') AND";
            }
            if (this.Songs.Count > 0)
            {
                where += " Song IN ('" + String.Join("', '", Songs) + "') AND";
            }
            if (where.EndsWith("AND"))
            {
                where = where.Substring(0, where.Length - 3);
            }
            if (where == "WHERE ")
            {
                return "";
            }
            Console.WriteLine("MIB: Where clause string " + where);
            return where;
        }

        public string convertFromWeekDayNumberToName(int value)
        {
            if (value >= 0 & value < 7)
            {
                return Enum.GetName(typeof(DayOfWeek), value);
            }
            else 
            {
                throw new ArgumentException("please provide a number value from 0 to 6");
            }
        }
        public string convertFromAndToStringNumber(string value)
        {
            if (value.Length > 1 & value.StartsWith("0"))
            {
                return value.Substring(1);
            }
            else if (value.Length == 1)
            {
                return "0" + value;
            }
            else
            {
                return value;
            }
        }
    }
}
