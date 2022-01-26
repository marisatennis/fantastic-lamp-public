using SQLite;
using System;

namespace FantasticLamp.Models
{
    public class MoodLog
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int MoodId { get; set; }
        public DateTime DateTime { get; set; }
    }
}