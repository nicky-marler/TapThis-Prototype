using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Connectivity;

namespace TapThis
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BarPage : ContentPage
    {
        public BarPage()
        {
            InitializeComponent();
        }

        private async void BarItem_Selected(object sender, SelectedItemChangedEventArgs e)
        {

            //ItemSelected is called on deselection, which results in SelectedItem being set to null
            if (e.SelectedItem == null) { return; }
            Model.Bar_Item Temp_Item = (Model.Bar_Item)e.SelectedItem;
            var Answer = await DisplayAlert("Submit Report", "Would you like to report " + Temp_Item.ItemName + "?", "Yes", "No");
            if (Answer)
            {
                var Answer_Report = await DisplayActionSheet("What is the issue?", "Cancel", null, "Price", "Time", "Everything", "Other");
                if (Answer_Report != "Cancel")
                {
                    Answer = await DisplayAlert("Report " + Answer_Report + "?", null, "Yes", "No");
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        if (Answer)
                        {
                            //Q up manager and submit data
                            // Create object to send.
                            Model.Send_Report Report = new Model.Send_Report
                            {
                                Document_Type = "Report",
                                Bar_Id = ((App)Application.Current).Selected_Bar.Bar_Id,
                                Item_Name = Temp_Item.ItemName,
                                Reason = Answer_Report
                            };

                            await Azure_Resource.Query_Manager.Send_Report(Report);
                            await DisplayAlert("Report Submited", null, "OK");

                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("No Connection", "Could not connect to the internet", "OK");
                    }
                   
                }
            }

            Main_BarList.SelectedItem = null;
        }
    }
}