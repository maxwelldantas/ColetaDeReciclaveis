using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ColetaDeReciclaveisClassLibrary.Controllers;
using ColetaDeReciclaveisClassLibrary.Models;
using ColetaDeReciclaveisClassLibrary.Utils;
using Xamarin.Forms;
using Xamarin.Forms.MultiSelectListView;
using Xamarin.Forms.Xaml;

namespace ColetaDeReciclaveis.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EcoPointScreen : ContentPage, INotifyPropertyChanged {
		public MultiSelectObservableCollection<Material> Materials = new MultiSelectObservableCollection<Material>();

		public EcoPoint Eco { get; set; }

		public EcoPointScreen() {
			Eco = new EcoPoint();
			InitializeComponent();
			Init();
		}

		#region Property

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null) {
			if (EqualityComparer<T>.Default.Equals(storage, value)) {
				return false;
			}

			storage = value;
			OnPropertyChanged(propertyName);
			return true;
		}

		#endregion

		private async void Init() {

			await SetMaterials();
		}
		private async Task<int> SetMaterials() {
			try {
				Materials = new MultiSelectObservableCollection<Material>();
				List<Material> MaterialsApi = await MaterialController.GetAllAPI();
				foreach (Material material in MaterialsApi)
					Materials.Add(material);

				lvwMaterials.ItemsSource = Materials;
			} catch { await DisplayAlert("Ocorreu um erro ao tentar obter a lista de materiais!", "Tente novamente mais tarde.", "OK"); }
			return 0;
		}

		private async void BtnAddEcoPonto_Clicked(object sender, EventArgs e) {
			if (!Validate()) {
				await DisplayAlert("Ocorreu um erro!", "Revise suas informações, alguns campos não podem ser nulos.", "OK");
				return;
			}
			try {

				Eco = new EcoPoint(NameEcoPontoEntry.Text, DescritionEcoPontoEntry.Text, Materials.SelectedItems.ToList(),
				new Address(CepEcoPontoEntry.Text, RuaEcoPontoEntry.Text, NumberEcoPontoEntry.Text, ComplementEcoPontoEntry.Text,
				BairroEcoPontoEntry.Text, CityEcoPontoEntry.Text, StateEcoPontoEntry.Text), new Phone(PhoneEcoPontoEntry.Text));

				UserController.CurrentUser.EcoPoints.Add(Eco);
				CallbackStatus status = await EcoPointController.AddAPI(UserController.CurrentUser);
				await DisplayAlert($"{ status.CurrentStatus.CallbackStatusToText()}", status.CallbackMessage, "OK");
			} catch { await DisplayAlert("Ocorreu um erro ao tentar adicionar novo ecoponto!", "Tente novamente mais tarde.", "OK"); }
		}

		private bool Validate() {

			if (NameEcoPontoEntry.Text == null || Materials.SelectedItems.ToList() == null || CepEcoPontoEntry.Text == null || RuaEcoPontoEntry.Text == null ||
			NumberEcoPontoEntry.Text == null || BairroEcoPontoEntry.Text == null || CityEcoPontoEntry.Text == null || StateEcoPontoEntry.Text == null ||
			PhoneEcoPontoEntry.Text == null)
				return false;
			return true;
		}
	}
}