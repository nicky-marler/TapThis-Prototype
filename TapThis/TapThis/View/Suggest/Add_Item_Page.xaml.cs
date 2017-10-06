using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TapThis.View.Suggest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Add_Item_Page : ContentPage
    {
        public Model.Suggest_Item Item { get; set; }
        public int Public_Item_Position { get; set; }

        public Add_Item_Page(bool New_Item, int Item_Position)
        {
            //     Item = ((App)Application.Current).Suggest.Data.Item_List[Item_Position];
            Public_Item_Position = Item_Position;

            BindingContext = ((App)Application.Current).Suggest.Data.Item_List[Item_Position];
            InitializeComponent();

            TurnOn_Selected_Labels();
           // ((App)Application.Current).Suggest.Page_Navigation = Navigation;



        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (((App)Application.Current).Suggest.Data.Item_List[Public_Item_Position].Item_Name == ""
                || ((App)Application.Current).Suggest.Data.Item_List[Public_Item_Position].Item_Name == null)
            {
                ((App)Application.Current).Suggest.Data.Item_List[Public_Item_Position].Item_Name = "A very mysterious item";
            }
            if (((App)Application.Current).Suggest.Data.Item_List[Public_Item_Position].Price == ""
               || ((App)Application.Current).Suggest.Data.Item_List[Public_Item_Position].Price == null)
            {
                ((App)Application.Current).Suggest.Data.Item_List[Public_Item_Position].Price = "Too expensive";
            }
        }

        private void Add_Day(object sender, EventArgs e)
        {
            if (((Label)sender).TextColor == Color.LightGray)
            {
                ((Label)sender).TextColor = Color.FromHex("03A9F4");
                ((App)Application.Current).Suggest.Data.Item_List[Public_Item_Position].Day.Add(((Label)sender).Text);
            }
            else
            {
                ((Label)sender).TextColor = Color.LightGray;
                ((App)Application.Current).Suggest.Data.Item_List[Public_Item_Position].Day.Remove(((Label)sender).Text);
            }

        }

        private void TurnOn_Selected_Labels()
        {
            foreach (string Day in ((App)Application.Current).Suggest.Data.Item_List[Public_Item_Position].Day)
            {
                switch (Day)
                {
                    case "Sunday":
                        Sunday.TextColor = Color.FromHex("03A9F4");
                        break;
                    case "Monday":
                        Monday.TextColor = Color.FromHex("03A9F4");
                        break;
                    case "Tuesday":
                        Tuesday.TextColor = Color.FromHex("03A9F4");
                        break;
                    case "Wednesday":
                        Wednesday.TextColor = Color.FromHex("03A9F4");
                        break;
                    case "Thursday":
                        Thursday.TextColor = Color.FromHex("03A9F4");
                        break;
                    case "Friday":
                        Friday.TextColor = Color.FromHex("03A9F4");
                        break;
                    case "Saturday":
                        Saturday.TextColor = Color.FromHex("03A9F4");
                        break;
                    default:
                        break;
                }

            }
        }

        private async void Delete_Item(object sender, EventArgs e)
        {
            var Answer = await DisplayAlert("Delete", "Are you sure?", "Yes", "No");
            if (Answer)
            {

                await Navigation.PopAsync();
                ((App)Application.Current).Suggest.Data.Item_List.RemoveAt(Public_Item_Position);

            }


        }
    }
}