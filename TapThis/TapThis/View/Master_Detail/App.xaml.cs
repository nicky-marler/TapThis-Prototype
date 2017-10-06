using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Connectivity;


namespace TapThis
{
    public partial class App : Application
    {

        public ViewModel.Filter_ViewModel Filter { get; private set; }
        public ViewModel.BarList_ViewModel BarList { get; set; }
        public ViewModel.Suggest_ViewModel Suggest { get; set; }
        public Model.Bar_Pins Bar_List_Pins { get; set; }
        public Model.Bar Selected_Bar { get; set; }
        public static Model.Geo_Location User_Location { get; set; }
        //public static bool Current_Connection { get; set; }

        public App ()
		{
			InitializeComponent();
            Filter = new ViewModel.Filter_ViewModel();
            Selected_Bar = new Model.Bar();
            Bar_List_Pins = new Model.Bar_Pins();
            User_Location = new Model.Geo_Location();
            BarList = new ViewModel.BarList_ViewModel();
            MainPage = new TapThis.Nav_Control();
            Suggest = new ViewModel.Suggest_ViewModel();


            CrossConnectivity.Current.ConnectivityChanged += Raise_Connection_Change;
            // TODO: Figure out why my emulator crshes on start up.


        }

        private void Raise_Connection_Change(object sender, EventArgs e)
        {
          
        }


        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
            // Handle when your app sleeps
        }

        protected override void OnResume ()
		{

        }
    }
}