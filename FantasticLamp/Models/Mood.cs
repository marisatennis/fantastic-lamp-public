using SQLite;

namespace FantasticLamp.Models
{
    public class Mood
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Emoji { get; set; }
    }
}