using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Connectivity;
using MvvmHelpers;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TapThis.ViewModel
{
    class Contact_ViewModel : BaseViewModel
    {
        public Command Send_Contact_Command { get; }
        public Model.Contact.Contact Contact { get; set; }

        public Contact_ViewModel()
        {
            Title = "Contact Us";
            Contact = new Model.Contact.Contact();
            Send_Contact_Command = new Command(async () => await Send_Contact(), () => Contact.Requirment_Met);
            //Add event
            Contact.Command_CanExecute += Refresh_CanExecute;
        }



        public async Task Send_Contact()
        {
            IsBusy = true;
            if (CrossConnectivity.Current.IsConnected)
            {
                var Answer = await App.Current.MainPage.DisplayAlert("Submit Message?", "Are you sure?", "Yes", "No");
                if (Answer)
                {
                    //Checking for Required info
                    try
                    {
                        //Send and shorten Data
                        await Azure_Resource.Query_Manager.Send_Contact(new Model.WriteDB.Contact
                        {
                            First = Contact.First,
                            Last = Contact.Last,
                            Company = Contact.Company,
                            Email = Contact.Email,
                            Message = Contact.Message,
                            Document_Type = "Contact"
                        });


                        //reset data after submission
                        Contact.Reset_Data();
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
            Send_Contact_Command.ChangeCanExecute();
        }
    }
}
