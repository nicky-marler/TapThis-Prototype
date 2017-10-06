using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms.Maps;
using MvvmHelpers;

namespace TapThis.Model
{
    public class Suggest_Data : ObservableObject
    {
        //Gotta fix the ID of the damn thing... Might make seperate model. way smaller, ecpeilaly in Item with the that crap

        string address = "";
        string city_state_zip = "";
        string business_name = "";

        bool address_flag = false;
        bool city_flag = false;
        bool business_flag = false;

        public string Business_Name
        {
            get { return business_name; }
            set
            {
                SetProperty(ref business_name, value);

                if (value == "") { business_flag = false; }
                else { business_flag = true; }

                Check_CanExecute(EventArgs.Empty);
            }
        }
        public string Address
        {
            get { return address; }
            set
            {
                SetProperty(ref address, value);
                OnPropertyChanged(nameof(Display_Address));

                if (value == "") { address_flag = false; }
                else { address_flag = true; }

                Check_CanExecute(EventArgs.Empty);
            }
        }
        public string City_State_Zip
        {
            get { return city_state_zip; }
            set
            {
                SetProperty(ref city_state_zip, value);
                OnPropertyChanged(nameof(Display_Address));

                if (value == "") { city_flag = false; }
                else { city_flag = true; }

                Check_CanExecute(EventArgs.Empty);
            }
        }
        public string Country
        {
            get; set;
        }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public ObservableCollection<Suggest_Item> Item_List
        {
            get; set;
        }

        


        public string Display_Address
        {
            get { return $"Address: {Address} {City_State_Zip}"; }
        }
        public bool Requirment_Met
        {
            get { return address_flag && city_flag && business_flag; }
        }

        public event EventHandler Command_CanExecute;


        protected virtual void Check_CanExecute(EventArgs e)
        {
            Command_CanExecute?.Invoke(this, e);
        }

        public void Reset_Data()
        {
            Business_Name = "";
            Address = "";
            City_State_Zip = "";
            Country = "";
            Latitude = null;
            Longitude = null;
            Item_List.Clear();
        }


    }
}
