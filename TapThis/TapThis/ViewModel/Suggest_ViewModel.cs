using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Connectivity;

namespace TapThis.ViewModel
{
    public class Suggest_ViewModel : BaseViewModel
    {
        //Model for List/Observable Coleection of items and bar info. List can be empty
        //Command for sending data. Will need to use Azure Resource for DB Key and Query
        //Need to make the pages for Location, Name? Might do entry cell, Adding items

        // public Model.Filter Filter_Settings { get; set; }
        public Command Send_Data_Command { get; }
        public Command Location_Current_Command { get; }
        public Command Location_Mark_Command { get; }
        public Command Update_With_Mark_Command { get; }
        public Command Add_New_Item_Command { get; }
        public Command Send_Suggest_Command { get; }


        public Model.Suggest_Data Data { get; set; }

        public Pin Mark_Pin { get; set; }
        //public string Temp_Address; Get from Pin.... Label is Address. Address is Zip and State

        public string Temp_Country;

        public List<string> Item_Types { get; set; }

        public event EventHandler Marking_Pin;
        public void Finshed_Marking_Pin()
        {
            On_Finshed_Marking_Pin(EventArgs.Empty);
        }
        protected virtual void On_Finshed_Marking_Pin(EventArgs e)
        {
            Marking_Pin?.Invoke(this, e);
        }

        public INavigation Page_Navigation { get; set; }

        public Geocoder Geo_Coder { get; set; }

        public Suggest_ViewModel()
        {
            Title = "Suggest";
            Data = new Model.Suggest_Data();
            Data.Item_List = new System.Collections.ObjectModel.ObservableCollection<Model.Suggest_Item>();

            Location_Current_Command = new Command(async () => await Fill_Location_Current(), () => !IsBusy);
            Location_Mark_Command = new Command<MapSpan>(async (MapSpan) => await Fill_Location_Mark(MapSpan), (MapSpan) => !IsBusy);
            Update_With_Mark_Command = new Command(async () => await Update_With_Mark(), () => !IsBusy);
            Add_New_Item_Command = new Command(async () => await Add_New_Item(), () => !IsBusy);
            Send_Suggest_Command = new Command(async () => await Send_Suggest(), () => Data.Requirment_Met);

            Data.Command_CanExecute += Refresh_CanExecute;

            Geo_Coder = new Geocoder();
            Mark_Pin = new Pin();

            
            

            Item_Types = new List<string>
            {
                {"Drink"},{"Food"},{"Cover"}
            };
        }

        public async Task Fill_Location_Current()
        {
            try
            {
                //Do the geocodeer. Currently crashing
                IsBusy = true;
                IEnumerable<string> Geo_Coder_Addresses = await Geo_Coder.GetAddressesForPositionAsync(App.User_Location.User_Position);

  
                Data.Latitude = App.User_Location.Latitude;
                Data.Longitude = App.User_Location.Longitude;
                //Only way to get the top string from the list. Will ont execute if empty. Checks for a shorten address. will break after one read through
                foreach (string Address in Geo_Coder_Addresses)
                {
                    StringReader Address_Reader = new StringReader(Address);
                    if (Address_Reader != null) { Data.Address = Address_Reader.ReadLine(); }
                    if (Address_Reader != null) { Data.City_State_Zip = Address_Reader.ReadLine(); }
                    if (Address_Reader != null) { Data.Country = Address_Reader.ReadLine(); }
                    break;
                }
                IsBusy = false;
                await Page_Navigation.PopToRootAsync();
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
                await App.Current.MainPage.DisplayAlert("Uh Oh!", "The address reader is having a bad day. Going to use latitude and longitude", "OK");

                Data.Latitude = App.User_Location.Latitude;
                Data.Longitude = App.User_Location.Longitude;
                Data.Address = App.User_Location.Latitude.ToString();
                Data.City_State_Zip = App.User_Location.Longitude.ToString();
                IsBusy = false;
                await Page_Navigation.PopToRootAsync();
            }

        }

        public async Task Fill_Location_Mark(MapSpan Map_View)
        {
            //Do geo coder and center of map BS... Assigning it to the data is another button

            try
            {
                IEnumerable<string> Geo_Coder_Addresses = await Geo_Coder.GetAddressesForPositionAsync(Map_View.Center);


                Data.Latitude = Map_View.Center.Latitude;
                Data.Longitude = Map_View.Center.Longitude;
                //Only way to get the top string from the list. Will ont execute if empty. Checks for a shorten address. will break after one read through
                foreach (string Address in Geo_Coder_Addresses)
                {
                    StringReader Address_Reader = new StringReader(Address);
                    if (Address_Reader != null) { Mark_Pin.Label = Address_Reader.ReadLine(); }
                    if (Address_Reader != null) { Mark_Pin.Address = Address_Reader.ReadLine(); }
                    if (Address_Reader != null) { Temp_Country = Address_Reader.ReadLine(); }
                    break;
                }

                Mark_Pin.Position = Map_View.Center;

                Finshed_Marking_Pin();
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
                await App.Current.MainPage.DisplayAlert("Uh Oh!", "The address reader is having a bad day. Going to use latitude and longitude", "OK");

                Data.Latitude = Map_View.Center.Latitude;
                Data.Longitude = Map_View.Center.Longitude;
                Mark_Pin.Label = Map_View.Center.Latitude.ToString();
                Mark_Pin.Address = Map_View.Center.Longitude.ToString();

                Mark_Pin.Position = Map_View.Center;

                Finshed_Marking_Pin();

            }
        }

        public async Task Update_With_Mark()
        {
            Data.Address = Mark_Pin.Label;
            Data.City_State_Zip = Mark_Pin.Address;
            if (Temp_Country != null)
            {
                Data.Country = Temp_Country;
            }
            await Page_Navigation.PopToRootAsync();
        }

        public async Task Add_New_Item()
        {
            Data.Item_List.Add(new Model.Suggest_Item());
            //Telling the page that it'a new Item
            int Last_Item = Data.Item_List.Count - 1;
            await Page_Navigation.PushAsync(new View.Suggest.Add_Item_Page(true, Last_Item));


        }

        public async Task Send_Suggest()
        {
            IsBusy = true;
            if (CrossConnectivity.Current.IsConnected)
            {
                var Answer = await App.Current.MainPage.DisplayAlert("Submit Suggestion", "Are you sure?", "Yes", "No");
                if (Answer)
                {
                    //Checking for Required info
                    try
                    {

                        //Send Data
                        await Azure_Resource.Query_Manager.Send_Data();
                        //await client.CreateDocumentAsync(collectionLink, item);

                        //reset data after submission
                        Data.Reset_Data();
                        await App.Current.MainPage.DisplayAlert("Successfully sent!", null, "OK");
                    }
                    catch (Exception e)
                    {
                        //Handle for Database fucking up somehow
                        //Might be able to setup a catch for no signal
                        Exception baseException = e.GetBaseException();
                        Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
                        await App.Current.MainPage.DisplayAlert("Uh Oh!", "Could not send suggestion for some reason. Please try again later", "OK");

                    }
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("No Connection", "Could not connect to the internet", "OK");
            }
            IsBusy = false;
        }

        void Refresh_CanExecute(object sender, EventArgs e)
        {
            Send_Suggest_Command.ChangeCanExecute();
        }
    }
}
