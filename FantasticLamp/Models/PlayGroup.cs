namespace FantasticLamp.Models
{
    public class PlayGroup
    {
        public string Name { get; set; }
        public int Plays { get; set; }
        public Image Image { get; set; }
        public string ImageURL
        {
            get => Image.Url;
        }
    }
}
