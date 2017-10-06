using System;
using System.Collections.Generic;
using System.Text;
using MvvmHelpers;
using Xamarin.Forms;

namespace TapThis.Model.Contact
{
    class Contact : ObservableObject
    {
        string first = "";
        string last = "";
        string email = "";
        string message = "";

        bool first_flag = false;
        bool last_flag = false;
        bool email_flag = false;
        bool message_flag = false;

        public string First
        {
            get { return first; }
            set
            {
                SetProperty(ref first, value);

                if (value == "") { first_flag = false; }
                else { first_flag = true; }

                Check_CanExecute(EventArgs.Empty);
            }
        }

        public string Last
        {
            get { return last; }
            set
            {
                SetProperty(ref last, value);

                if (value == "") { last_flag = false; }
                else { last_flag = true; }

                Check_CanExecute(EventArgs.Empty);
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                SetProperty(ref email, value);

                if (value == "") { email_flag = false; }
                else { email_flag = true; }

                Check_CanExecute(EventArgs.Empty);
            }
        }

        public string Message
        {
            get { return message; }
            set
            {
                SetProperty(ref message, value);

                if (value == "") { message_flag = false; }
                else { message_flag = true; }

                Check_CanExecute(EventArgs.Empty);
            }
        }

        public string Company { get; set; }

        public bool Requirment_Met
        {
            get { return first_flag && last_flag && email_flag && message_flag; }
        }

        public event EventHandler Command_CanExecute;


        protected virtual void Check_CanExecute(EventArgs e)
        {
            Command_CanExecute?.Invoke(this, e);
        }

        public void Reset_Data()
        {
            First = "";
            Last = "";
            Email = "";
            Company = "";
            Message = "";
        }
    }
}
