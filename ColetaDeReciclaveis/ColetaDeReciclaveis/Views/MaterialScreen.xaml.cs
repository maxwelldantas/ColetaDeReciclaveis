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
    public partial class MaterialScreen : ContentPage
    {
        public MaterialScreen()
        {
            InitializeComponent();
        }

        private async void BtnAddMaterial_Clicked(object sender, EventArgs e)
        {
		if(!Validate()){
				await DisplayAlert($"Ocorreu um erro!","Alguns campos nulos não são aceitos.", "OK");
				return;
		}
			try {
				Material material = new Material(MaterialEntry.Text, DescritionEntry.Text);
				CallbackStatus status = await MaterialController.AddAPI(material);
				await DisplayAlert($"{ status.CurrentStatus.CallbackStatusToText()}", status.CallbackMessage, "OK");
			}
			catch{ await DisplayAlert($"Ocorreu um erro ao adicionar novo material!", "Tente novamente mais tarde.", "OK"); }
		}

		private bool Validate(){
			if (MaterialEntry.Text == null)
				return false;
			return true;
		}
    }
}