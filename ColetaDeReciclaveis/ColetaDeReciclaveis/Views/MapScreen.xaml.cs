using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColetaDeReciclaveis.Controllers;
using ColetaDeReciclaveisClassLibrary.Controllers;
using ColetaDeReciclaveisClassLibrary.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace ColetaDeReciclaveis.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapScreen : ContentPage {

		public static MapScreen Instance;

		private List<EcoPoint> _EcoPoints = new List<EcoPoint>();

		public MapScreen() {
			InitializeComponent();
			Instance = this;
			InitializePage();
		}

		public async void InitializePage() {
			try {
				await MapsController.InstantiateNewMap(MapRegion);
				_EcoPoints = await EcoPointController.GetAllAPI();
				AddPinsToMap();
			}
			catch{ await DisplayAlert("Ocorreu um erro ao tentar obter os dados do mapa!","Tente novamente mais tarde!","Ok"); }
		}

		public async void AddPinsToMap() {

			MapsController.CustomPins.Clear();

			foreach (EcoPoint ecoPoint in _EcoPoints) {
				try {

					string materials = "";
					foreach (Material material in ecoPoint.MaterialsAccepted)
						materials += material.Name + ", ";

					MapsController.AddPin(await MapsController.GetLocationFromAddress($"{ecoPoint.Address.AddressStreet} - {ecoPoint.Address.Number}, {ecoPoint.Address.Complement} - {ecoPoint.Address.District}, {ecoPoint.Address.City} - {ecoPoint.Address.State}"),
					$"{ecoPoint.Name}", $"{ecoPoint.Address.AddressStreet} - {ecoPoint.Address.Number}, {ecoPoint.Address.Complement} - {ecoPoint.Address.District}\n{ecoPoint.Description}\n{materials}");
				} catch { }
			}

			foreach (Pin pin in MapsController.CustomPins) {
				try { MapsController.CustomMap.Pins.Add(pin); } catch { }
				pin.Clicked += (sender, e) => {
					DisplayAlert(pin.Label, pin.Address, "Fechar");
				};
			}
		}
	}
}