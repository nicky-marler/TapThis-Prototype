﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TapThis.View.Contact
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Contact_Us : ContentPage
	{
		public Contact_Us ()
		{
			InitializeComponent ();
            BindingContext = new ViewModel.Contact_ViewModel();

        }


    }
}