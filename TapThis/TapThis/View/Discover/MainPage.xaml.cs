using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Xamarin.Forms.Maps;


namespace TapThis
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {

        
        public MainPage()
        {
            App.User_Location.User_Location += On_User_Location;
            ((App)Application.Current).Bar_List_Pins.Getting_Pins += Update_Map_Pins;
            

            InitializeComponent();

            User_Map.BindingContext = App.User_Location;
            ((App)Application.Current).BarList.Navigation = Navigation;

            BindingContext = ((App)Application.Current).BarList;
            
            
            //Maps
            if (!TapThis.App.User_Location.Is_Enabled)
            {
                TapThis.App.User_Location.Latitude = 30.267153;
                TapThis.App.User_Location.Longitude = -97.743061;
                Set_Map_Default();
            }
            else
            {
                Set_Map_Connected();
            }
            //Handle keeping exsisting pins on the map
            if (((App)Application.Current).Bar_List_Pins.Count > 0)
            {
                ((App)Application.Current).Bar_List_Pins.Finshed_Getting_Pins();
            }

            //User_Map.BindingContext = App.User_Location;


        }
        private void On_User_Location(object sender, EventArgs e)
        {
            Set_Map_Connected();
        }

        private async void BarSelected_OpenBarPage(object sender, SelectedItemChangedEventArgs e)
        {
            //ItemSelected is called on deselection, which results in SelectedItem being set to null
            if (e.SelectedItem == null) { return; }

            ((App)Application.Current).Selected_Bar = (Model.Bar)e.SelectedItem;
            await Navigation.PushAsync(new BarPage());
            Main_BarList.SelectedItem = null;

        }

        public void Set_Map_Default()
        {
            Distance Standard_User_Zoom = Distance.FromKilometers(1000);
            MapSpan Goto_User = MapSpan.FromCenterAndRadius(App.User_Location.User_Position, Standard_User_Zoom);
            User_Map.MoveToRegion(Goto_User);
        }
        public void Set_Map_Connected()
        {
            Distance Standard_User_Zoom = Distance.FromKilometers(2);
            MapSpan Goto_User = MapSpan.FromCenterAndRadius(App.User_Location.User_Position, Standard_User_Zoom);
            User_Map.MoveToRegion(Goto_User);

            //User_Map.IsShowingUser = true;
            
        }

        private async void ShowHide_List(object sender, EventArgs e)
        {
            if(((Button)sender).Text == "Show List")
            {
                ((Button)sender).Text = "Hide List";
                The_List.IsVisible = true;
                await The_List.FadeTo(1,250);
            }
            else
            {
                ((Button)sender).Text = "Show List";
                await The_List.FadeTo(0, 250);
                The_List.IsVisible = false;
            }
        }


        private void Update_Map_Pins(object sender, EventArgs e)
        {
            User_Map.Pins.Clear();

           foreach(Pin Bar_Pin in ((App)Application.Current).Bar_List_Pins)
            {
                Bar_Pin.Clicked += BarPinSelected_OpenBarPage;
                User_Map.Pins.Add(Bar_Pin);
                
            }
        }

        private async void BarPinSelected_OpenBarPage(object sender, EventArgs args)
        {
            
            foreach (Model.Bar Bar in ((App)Application.Current).BarList.Bar_List)
            {
                if (Bar.Address1 == ((Pin)sender).Address )
                {
                    ((App)Application.Current).Selected_Bar = Bar;
                    await Navigation.PushAsync(new BarPage());
                    break;
                }
            }
        }


        
    }
}
