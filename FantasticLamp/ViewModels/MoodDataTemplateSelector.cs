using FantasticLamp.Models;
using Xamarin.Forms;

namespace FantasticLamp.ViewModels
{
    class MoodDataTemplateSelector: DataTemplateSelector
	{
		private int style = 1;

		public DataTemplate OneDataTemplate { get; set; }

		public DataTemplate TwoDataTemplate { get; set; }

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			DataTemplate template;

			int remainder = this.style % 4;
			if (remainder < 2 || remainder > 3)
			{
				template = OneDataTemplate;
			}
			else
			{
				template = TwoDataTemplate;
			}
			++this.style;

			return template;
		}
	}
}
