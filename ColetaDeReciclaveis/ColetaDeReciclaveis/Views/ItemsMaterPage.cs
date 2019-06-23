using System;

namespace ColetaDeReciclaveis.Views
{
    public class ItemsMaterPage
    {
		public ItemsMaterPage(string title, string icon, Type targetType) {
			Title = title;
			Icon = icon;
			TargetType = targetType;
		}

		public string Title { get; set; }
        public string Icon { get; set; }
        public Type TargetType { get; set; }
    }
}
