using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColetaDeReciclaveisClassLibrary.Controllers;
using ColetaDeReciclaveisClassLibrary.Models;
using ColetaDeReciclaveisClassLibrary.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ColetaDeReciclaveis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserScreen : ContentPage
    {
        public UserScreen()
        {
            InitializeComponent();
			if (User.CurrentUser.IsAppAdmin || User.CurrentUser.IsDbaAdmin) {
				DbaAdminCheckbox.IsVisible = true;
				AppAdmCheckbox.IsVisible = true;
			}
		}

		public UserScreen(User user) {
			InitializeComponent();
			if (user.IsAppAdmin || user.IsDbaAdmin) {
				DbaAdminCheckbox.IsVisible = true;
				AppAdmCheckbox.IsVisible = true;
			}
		}
		private async void BtnAdicionarUser_Clicked(object sender, EventArgs e)
        {
			bool canPass = await Validate();
			if ( !canPass)
				return;
			try {
				Content.BackgroundColor = Color.Gray;
				Content.InputTransparent = true;

				if (PasswordEntry.Text == ConfPasswordEntry.Text) {
					User user = new User(FullNameEntry.Text, CpfEntry.Text, EmailEntry.Text, AboutEntry.Text, BirthdayEntry.Date, ColetorCheckbox.Checked, DoadorCheckbox.Checked, UsernameEntry.Text, PasswordEntry.Text);
					Address address = new Address(CepEntry.Text, RuaEntry.Text, NumberEntry.Text, ComplementEntry.Text, BairroEntry.Text, CityEntry.Text, StateEntry.Text);
					Phone phone = new Phone(PhoneEntry.Text);
					user.Addresses.Add(address);
					user.Phones.Add(phone);
					CallbackStatus status = new CallbackStatus();
					status = await UserController.AddAPI(user);
					await DisplayAlert($"{ status.CurrentStatus.CallbackStatusToText()}", status.CallbackMessage, "OK");
					if (Application.Current.MainPage.GetType() == typeof(Login))
						await Navigation.PopModalAsync();
				} else {
					await DisplayAlert("Erro", "Senhas digitadas não conferem!", "Ok");
					Content.BackgroundColor = Color.White;
					Content.InputTransparent = false;
				}
			} catch{ await DisplayAlert("Ocorreu um erro ao tentar adicionar novo usuário!", "Tente novamente mais tarde!", "Ok"); }
        }

		private async Task<bool> Validate(){
			if(FullNameEntry.Text == null || CpfEntry.Text == null|| EmailEntry.Text == null|| BirthdayEntry.Date == null || UsernameEntry.Text == null || PasswordEntry.Text == null || CepEntry.Text == null || RuaEntry.Text == null || NumberEntry.Text == null || BairroEntry.Text == null || CityEntry.Text == null || StateEntry.Text == null || PhoneEntry.Text == null){
				await DisplayAlert("Erro", "Não são permitidos campos nulos", "Ok");
				return false;
			}else{
				return true;
			}
		}
	}
}