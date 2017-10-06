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
	public partial class SuggestPage : ContentPage
	{
        // Saved commands from "AddItem page" How i submitted data to the database
        // Uri collectionLink = UriFactory.CreateDocumentCollectionUri( Azure_Resource.User_InputDB.Database, Azure_Resource.User_InputDB.ItemCollection);    
        // DocumentClient client = new DocumentClient(new Uri(Azure_Resource.User_InputDB.EndpointUri), Azure_Resource.User_InputDB.ReadKey);
        // await client.CreateDocumentAsync(collectionLink, BarItem);
        

        public SuggestPage ()
		{
			InitializeComponent ();
            List_Count.BindingContext = ((App)Application.Current).Suggest.Data.Item_List;
        }

        private async void Get_Location(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Get_Location_Page());
        }

        private async void Get_Items(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Get_Items_Page());
        }

    }
}