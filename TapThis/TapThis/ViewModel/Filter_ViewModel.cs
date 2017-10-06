using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TapThis.ViewModel
{
    public class Filter_ViewModel : BaseViewModel
    {
        public Model.Filter Filter_Settings { get; set; }
        public Command Reset_FilterSettings_Command { get; }
        public List<string> Days_of_Week { get; set; }


        public Filter_ViewModel()
        {
            Title = "Filter";
            Filter_Settings = new Model.Filter();
            Reset_FilterSettings_Command = new Command(Reset_FilterSettings);

            Days_of_Week = new List<string>
            {
                {"Sunday"},{"Monday"},{"Tuesday"},{"Wednesday"},{"Thursday"},{"Friday"},{"Saturday"}
            };
        }

        void Reset_FilterSettings()
        {
            Filter_Settings.Day = DateTime.Today.DayOfWeek.ToString();
            //Filter_Settings.Time = DateTime.Now.Hour;
            //   Filter_Settings.Type = "All"; //Need to change to in or to nested if for drink, food, all
            Filter_Settings.All_Day = false;
            Filter_Settings.Special = false;
        }


    }
}
