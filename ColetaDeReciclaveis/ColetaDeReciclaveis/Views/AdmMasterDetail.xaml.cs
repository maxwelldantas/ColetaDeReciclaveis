using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ColetaDeReciclaveis.Views;
using ColetaDeReciclaveisClassLibrary.Models;
using ColetaDeReciclaveisClassLibrary.Controllers;
using Xamarin.Forms.MultiSelectListView;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ColetaDeReciclaveis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdmMasterDetail : MasterDetailPage
    {
		private ObservableCollection<ItemsMaterPage> ListUser = new ObservableCollection<ItemsMaterPage>();

		private ObservableCollection<ItemsMaterPage> ListAdmin = new ObservableCollection<ItemsMaterPage>();

		public AdmMasterDetail(User user)
        { 
            InitializeComponent();
			
			UserController.CurrentUser = user;

			GetMenuItems(user);
			GetAdminMenuItems(user);

			navigationApp.ItemsSource = ListUser;
			navigationList.ItemsSource = ListAdmin;

            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MapScreen)));
        }

        private void MenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ItemsMaterPage item = (ItemsMaterPage)e.SelectedItem;

            Type pagina = item.TargetType;

            Detail = new NavigationPage((Page)Activator.CreateInstance(pagina));

            IsPresented = false;
        }

		private void Logout(object sender, EventArgs e){
			Application.Current.MainPage = new Login();
		}

		private void CallAboutPage(object sender, EventArgs e) {
			Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(AboutScreen)));
		}

		public void GetMenuItems(User user) {

			ListUser.Clear();

			ListUser.Add(new ItemsMaterPage("Mapa de EcoPontos", "Location48.png", typeof(MapScreen)));

			if (user.IsAppAdmin || user.IsDbaAdmin) {

			}

			if (user.IsDonator) {
				//ListUser.Add(new ItemsMaterPage("Meus Serviços", "Gear48.png", typeof(JobsScreen)));
			}

			if (user.IsColector) {
				ListUser.Add(new ItemsMaterPage("Serviços", "Gear48.png", typeof(JobsScreen)));
			}
		}

		public void GetAdminMenuItems(User user) {
			ListAdmin.Clear();

			if (user.IsAppAdmin || user.IsDbaAdmin) {
			ListAdmin.Add(new ItemsMaterPage("Novo EcoPonto", "Office48.png", typeof(EcoPointScreen)));
				ListAdmin.Add(new ItemsMaterPage("Novo Material", "Box48.png", typeof(MaterialScreen)));
				ListAdmin.Add(new ItemsMaterPage("Novo Usuário", "User48.png", typeof(UserScreen)));
			}

			if (user.IsDonator) {
				ListAdmin.Add(new ItemsMaterPage("Novo Serviço", "Gear48.png", typeof(JobScreen)));
			}

			if (user.IsColector) {

			}
			if(ListAdmin.Count <= 0){ navigationList.IsVisible = false; }
		}

	}
}