using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ColetaDeReciclaveisClassLibrary.Controllers;
using ColetaDeReciclaveisClassLibrary.Models;
using ColetaDeReciclaveisClassLibrary.Utils;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ColetaDeReciclaveis.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
		public Login()
		{
			InitializeComponent();
		}

		private async void ButtonClicked_Login(object sender, EventArgs e) {
			Content.InputTransparent = true;
			Content.BackgroundColor = Color.Gray;

			if (Connectivity.NetworkAccess != NetworkAccess.Internet) {
				await DisplayAlert($"Sem conexão com a internet!","Tente novamente mais tarde." , "OK");
				return;
			}
			try {

				LoginController loginController = new LoginController();
				CallbackStatus status = new CallbackStatus();
				await loginController.Login(EntryLogin.Text, EntryPassword.Text);

				status = loginController.status;

				User user = new User();
				Content.InputTransparent = false;
				Content.BackgroundColor = Color.White;
				if (loginController.Id > 0) {
					user = await UserController.GetAPI((int)loginController.Id);
					Application.Current.MainPage = new AdmMasterDetail(user);
				} else {
					await DisplayAlert($"{ status.CurrentStatus.CallbackStatusToText()}", status.CallbackMessage, "OK");
				}
			}catch{
				await DisplayAlert($"Problemas na rede!", "Tente novamente mais tarde.", "OK");
			}
		}

		private async void ButtonCliclekd_NewUser(object sender, EventArgs e){
			await Navigation.PushModalAsync(new UserScreen());
		}

	}
}