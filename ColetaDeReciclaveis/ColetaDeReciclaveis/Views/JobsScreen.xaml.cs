using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColetaDeReciclaveisClassLibrary.Controllers;
using ColetaDeReciclaveisClassLibrary.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ColetaDeReciclaveis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JobsScreen : ContentPage
    {

		public List<User> Users = new List<User>();
		public List<Job> Jobs = new List<Job>();

        public JobsScreen()
        {
            InitializeComponent();
			ListAll();
        }

		private async void ListAll(){

			try {
				Users = await UserController.GetAllAPI();

				foreach (User user in Users) {
					foreach (Job job in user.Jobs) {
						try { job.Phone = user.Phones.FirstOrDefault().Number; } catch { }
						job.GetImage();
						Jobs.Add(job);
					}
				}
				anuncios.ItemsSource = Jobs;
			}
			catch{ await DisplayAlert("Ocorreu um erro ao tentar recuperar os serviços!", "Tente novamente mais tarde.", "Ok"); }
		}
    }
}