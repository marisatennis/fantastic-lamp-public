using Xamarin.Forms;

namespace FantasticLamp.Services.Filters
{
    public class FilterOption
    {
        public string Category { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        static public FilterOption fromButton(Button button)
        {
            FilterOption option = new FilterOption();
            option.Category = button.ClassId;
            option.Id = int.Parse(button.StyleId);
            option.Name = button.Text;
            return option;
        }

        public Button getButton()
        {
            Button button = new Button();
            button.ClassId = this.Category;
            button.StyleId = this.Id.ToString();
            button.Text = this.Name;
            return button;
        }

        public override bool Equals(object obj)
        {
            return obj is FilterOption option &&
                   Category == option.Category &&
                   Id == option.Id;
        }
    }
}
