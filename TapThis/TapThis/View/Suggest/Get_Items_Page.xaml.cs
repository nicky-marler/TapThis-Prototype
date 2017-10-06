using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TapThis
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Get_Items_Page : ContentPage
	{
		public Get_Items_Page ()
		{
            InitializeComponent ();
            BindingContext = ((App)Application.Current).Suggest;
            ((App)Application.Current).Suggest.Page_Navigation = Navigation;

        }

        private async void Add_Item_Page_Old(object sender, SelectedItemChangedEventArgs e)
        {
            //ItemSelected is called on deselection, which results in SelectedItem being set to null
            if (e.SelectedItem == null) { return; }
            //I am finding the integer index of the selected Item within the list it's clicked on. THen passing it to the item page
            await Navigation.PushAsync(new View.Suggest.Add_Item_Page(false, ((App)Application.Current).Suggest.Data.Item_List.IndexOf((Model.Suggest_Item)e.SelectedItem)));
            Added_Items_List.SelectedItem = null;

        }

    }
}