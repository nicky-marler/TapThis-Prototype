using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using MvvmHelpers;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents.Spatial;
using System.Linq;
using System.Net;
using Xamarin.Forms.Maps;
using Plugin.Connectivity;

namespace TapThis.ViewModel
{
    public class BarList_ViewModel : BaseViewModel
    {
        // The internal Data Structure
        private bool Bar_In_List;
        private bool Group_In_List;
        public ObservableCollection<Model.Bar> Bar_List { get; set; }

        // Commands to be used on this view model
        public Command GetBarItemsCommand { get; }
        public Command UpdateFilterPageCommand { get; }
        public Command SelectedBarPage_Command { get; }

        // For pushing and popping this page from a stack
        public INavigation Navigation;


        public BarList_ViewModel()
        {

            Bar_List = new ObservableCollection<Model.Bar>();

            GetBarItemsCommand = new Command<MapSpan>(async (MapSpan) => await GetBarItems(MapSpan), (MapSpan) => !IsBusy);
            UpdateFilterPageCommand = new Command(async () => await UpdateFilterPage(), () => !IsBusy);
            SelectedBarPage_Command = new Command(async () => await UpdateFilterPage(), () => !IsBusy);

        }


        /// <summary>
        /// GetBarItems is the task that handles the fetch query, handling the data structure, and their Algorithms. 
        /// </summary>
        private async Task GetBarItems(MapSpan Map_View)
        {
            IsBusy = true;
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    Bar_List.Clear();
                    ((App)Application.Current).Bar_List_Pins.Clear();
                    // Making my polygon to give to the query
                    Polygon Map_Corners = new Polygon(new[]
                        {new LinearRing(new [] {
                        new Microsoft.Azure.Documents.Spatial.Position((Map_View.Center.Longitude - Map_View.LongitudeDegrees), (Map_View.Center.Latitude + Map_View.LatitudeDegrees)),
                        new Microsoft.Azure.Documents.Spatial.Position((Map_View.Center.Longitude - Map_View.LongitudeDegrees), (Map_View.Center.Latitude - Map_View.LatitudeDegrees)),
                        new Microsoft.Azure.Documents.Spatial.Position((Map_View.Center.Longitude + Map_View.LongitudeDegrees), (Map_View.Center.Latitude - Map_View.LatitudeDegrees)),
                        new Microsoft.Azure.Documents.Spatial.Position((Map_View.Center.Longitude + Map_View.LongitudeDegrees),(Map_View.Center.Latitude + Map_View.LatitudeDegrees)),
                        new Microsoft.Azure.Documents.Spatial.Position((Map_View.Center.Longitude - Map_View.LongitudeDegrees), (Map_View.Center.Latitude + Map_View.LatitudeDegrees))

                        })
                    });
                    Microsoft.Azure.Documents.Spatial.Point Map_Center = new Microsoft.Azure.Documents.Spatial.Point(
                        Map_View.Center.Longitude, Map_View.Center.Latitude
                        );
                    //Gets the correct filtered Query

                    var Query = Azure_Resource.Query_Manager.Set_Query(Map_Corners);
                    int Temp_Time = DateTime.Now.Hour;
                    string Temp_Day = DateTime.Today.DayOfWeek.ToString();

                    //Invariant: Finite number of Items. Counting down to 0. Only relevent if i were doing pages via feed option MaxCount
                    while (Query.HasMoreResults)
                    {
                        foreach (Model.Document DB_Document in await Query.ExecuteNextAsync<Model.Document>())
                        {
                            Bar_In_List = false;
                            // Checks to see if the Bar for the document already exsists. If it does, add the Item to the correct bar and exit the loop.
                            foreach (Model.Bar BarList_Element in Bar_List)
                            {
                                if (BarList_Element.Bar_Id == DB_Document.Bar_Id)
                                {
                                    Group_In_List = false;
                                    Bar_In_List = true;

                                    Model.Bar_Item Temp_Item = new Model.Bar_Item
                                    {
                                        ItemName = DB_Document.Name,
                                        Price = DB_Document.Price,
                                        Hours = DB_Document.BleedDay == null ? SetAMandPM(DB_Document.Start) + " - " + SetAMandPM(DB_Document.End) :
                                            SetAMandPM(DB_Document.Start) + " - " + SetAMandPM(DB_Document.BleedEnd),
                                        Active = DB_Document.Start <= Temp_Time && Temp_Time < DB_Document.End && Temp_Day == DB_Document.Day ||
                                        (Temp_Day == DB_Document.BleedDay && Temp_Time < DB_Document.BleedEnd)

                                    };
                                    //Add item to list in group if list exsists
                                    foreach (Model.Grouped_List Group_Element in BarList_Element.Item_Group)
                                    {
                                        if (Group_Element.Title == DB_Document.Type)
                                        {
                                            Group_Element.Add(Temp_Item);
                                            Group_In_List = true;
                                        }
                                    }
                                    //Make new group list and add item to list
                                    if (!Group_In_List)
                                    {
                                        Model.Grouped_List Temp_List = new Model.Grouped_List
                                        {
                                            Title = DB_Document.Type,
                                            ShortName = DB_Document.Type[0].ToString(),
                                        };
                                        Temp_List.Add(Temp_Item);
                                        BarList_Element.Item_Group.Add(Temp_List);
                                    }
                                    //BarList_Element.Item_List.Add(Temp_Item);
                                    //should remove later
                                    break;
                                }
                            }
                            //If the Bar for the document does not exsist. Create new Bar and add it to the Bar_List. Then add the new Item.
                            //Also add pin to pin list
                            if (!Bar_In_List)
                            {
                                Model.Bar_Item Temp_Item = new Model.Bar_Item
                                {
                                    ItemName = DB_Document.Name,
                                    Price = DB_Document.Price,
                                    Hours = DB_Document.BleedDay == null ? SetAMandPM(DB_Document.Start) + " - " + SetAMandPM(DB_Document.End) :
                                            SetAMandPM(DB_Document.Start) + " - " + SetAMandPM(DB_Document.BleedEnd),
                                    Active = DB_Document.Start <= Temp_Time && Temp_Time < DB_Document.End && Temp_Day == DB_Document.Day ||
                                        (Temp_Day == DB_Document.BleedDay && Temp_Time < DB_Document.BleedEnd)
                                };
                                Model.Grouped_List Temp_List = new Model.Grouped_List
                                {
                                    Title = DB_Document.Type,
                                    ShortName = DB_Document.Type[0].ToString()
                                };
                                Temp_List.Add(Temp_Item);

                                Model.Bar Temp_Bar = new Model.Bar
                                {
                                    Bar_Id = DB_Document.Bar_Id,
                                    BarName = DB_Document.BarName,
                                    Address1 = DB_Document.Address1,
                                    Address2 = DB_Document.Address2,
                                    Location = DB_Document.Location,
                                    Phone = DB_Document.Phone,
                                    Kitchen_Hours = DB_Document.Kitchen_Hours,
                                    Open_Hours = DB_Document.Open_Hours,
                                    Item_Group = new List<Model.Grouped_List>()

                                };
                                Temp_Bar.Item_Group.Add(Temp_List);

                                //Sorting Each business by near to far
                                //Handles the intial empty list
                                if (Bar_List.Count == 0)
                                {
                                    Bar_List.Add(Temp_Bar);
                                }
                                else
                                {
                                    int Index_Counter = 0;
                                    foreach (Model.Bar Sort_Bar in Bar_List)
                                    {
                                        //(Center,New,Old) -> bool
                                        if (Is_New_Closer_To_Center(
                                           Map_View.Center.Longitude, Map_View.Center.Latitude,
                                           Temp_Bar.Location.Position.Longitude, Temp_Bar.Location.Position.Latitude,
                                           Sort_Bar.Location.Position.Longitude, Sort_Bar.Location.Position.Latitude))
                                        {
                                            Bar_List.Insert(Index_Counter, Temp_Bar);
                                            Bar_In_List = true;
                                            break;
                                        }
                                        else
                                        {
                                            Index_Counter++;
                                        }

                                    }
                                    //Adds the bar if it is the furthest bar in the list
                                    if (!Bar_In_List)
                                    {
                                        Bar_List.Add(Temp_Bar);
                                    }
                                }


                                // Adding pins
                                ((App)Application.Current).Bar_List_Pins.Add(new Pin
                                {
                                    Label = Temp_Bar.BarName,
                                    Address = Temp_Bar.Address1,
                                    Position = new Xamarin.Forms.Maps.Position(DB_Document.Location.Position.Latitude, DB_Document.Location.Position.Longitude)
                                }
                                );
                            }
                        }
                    }
                //Fire event to tell maps to add pins
                ((App)Application.Current).Bar_List_Pins.Finshed_Getting_Pins();

                }
                catch (DocumentClientException de)
                {
                    Exception baseException = de.GetBaseException();
                    Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
                }
                catch (Exception e)
                {
                    Exception baseException = e.GetBaseException();
                    Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
                }
                finally
                {
                    IsBusy = false;
                    if (Bar_List.Count == 0)
                    {
                        await App.Current.MainPage.DisplayAlert("No results", null, "OK");
                    }
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("No Connection", "Could not connect to the internet", "OK");       
            }

            IsBusy = false;
            
        }

        public async Task UpdateFilterPage()
        {
            await Navigation.PushAsync(new FilterPage());
        }

        public async Task SelectedBarPage()
        {
            await Navigation.PushAsync(new BarPage());
        }

        //Needing to do boths Math Truncate sides to get hours and minutes
        public string SetAMandPM(double x)
        {
            var hour = Math.Truncate(x);
            var min = x - Math.Truncate(x);
            string min_string = "";

            //Need to handle minutes under 10. will return single digit.
            if (min < 10)
            {
                min_string = $"0{min}";
            }
            else
            {
                min_string = min.ToString();
            }
            // Handles Midnight. Clock does 00 to 23. I write 24 for shorthand
            if (hour == 24 && min == 0) { return "Midnight"; }

            if (0 <= hour && hour < 12)
            {
                if (hour == 0)
                {
                    hour = 12;
                }
                return $"{hour}:" + min_string + " am";
                //return hour.ToString() +":"+ min.ToString() + " am";
            }
            //Fixes military to normal. Keeps noon in mind
            if (hour != 12)
            {
                hour = hour - 12;
            }
            return $"{hour}:" + min_string + " pm";
            //return x.ToString() + ":00 pm";
        }

        public bool Is_New_Closer_To_Center(double Center_x, double Center_y, double New_x, double New_y, double Old_x, double Old_y)
        {
            return Math.Sqrt((Center_x - New_x) * (Center_x - New_x) + (Center_y - New_y) * (Center_y - New_y))
                < Math.Sqrt((Center_x - Old_x) * (Center_x - Old_x) + (Center_y - Old_y) * (Center_y - Old_y));
        }
    }
}
