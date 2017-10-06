using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TapThis
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NavMaster : ContentPage
	{
        public ListView ListView => ListViewMenuItems;

        public NavMaster ()
		{
			InitializeComponent ();
            BindingContext = new NavMasterViewModel();
        }

        class NavMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<NavMenuItem> MenuItems { get; }
            public NavMasterViewModel()
            {
                MenuItems = new ObservableCollection<NavMenuItem>(new[]
                {
                    new NavMenuItem { Id = 0, Title = "Discover", TargetType = typeof(MainPage) },
                    new NavMenuItem { Id = 1, Title = "Suggest", TargetType = typeof(SuggestPage) },
                    new NavMenuItem { Id = 1, Title = "About Us", TargetType = typeof(View.About.About_Us) },
                    new NavMenuItem { Id = 1, Title = "Contact", TargetType = typeof(View.Contact.Contact_Us) }
                });
            }
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        }

    }
}