using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using Plugin.Media;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ColetaDeReciclaveisClassLibrary.Models;
using ColetaDeReciclaveisClassLibrary.Controllers;
using ColetaDeReciclaveisClassLibrary.Utils;
using System.IO;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace ColetaDeReciclaveis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JobScreen : ContentPage
    {

		private byte[] filePath { get; set; }

        public JobScreen()
        {
            InitializeComponent();
			CheckPermissions();
		}

		private async void CheckPermissions(){
			var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
			if (status != PermissionStatus.Granted) {
				//if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location)) {
				//	await DisplayAlert("Need location", "Gunna need that location", "OK");
				//}

				 await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
			}

			status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
			if (status != PermissionStatus.Granted) {
				//if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location)) {
				//	await DisplayAlert("Need location", "Gunna need that location", "OK");
				//}

				await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
			}
		}

        private async void BtnAlbum_Clicked(object sender, EventArgs e)
        {

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Ops", "Galeria de fotos não suportada", "Ok");
                return;
            }

			try {
				var file = await CrossMedia.Current.PickPhotoAsync();

				if (file == null)
					return;
				filePath = File.ReadAllBytes(file.Path);
				ImgMaterial.Source = ImageSource.FromStream(() => file.GetStream());
			}
			catch{ await DisplayAlert("Ops", "Galeria de fotos não suportada", "Ok"); }
        }

        private async void BtnTirarFoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

			// verifica se a camera foi identificada
			if (!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
            {
                await DisplayAlert("Ops", "Nenhuma câmera detectada.", "OK");

                return;
            }
			try {
				// abre a camera para tirar a foto
				var file = await CrossMedia.Current.TakePhotoAsync(
					new StoreCameraMediaOptions {
						SaveToAlbum = true,
						Directory = "Material"
						//Directory = Path.Combine(StaticsInfo.DEFAULT_PATH,"Material")
					});

				if (file == null)
					return;
				filePath = File.ReadAllBytes(file.Path);
				ImgMaterial.Source = ImageSource.FromStream(() => file.GetStream());
			} catch (Exception ex){ await DisplayAlert("Ops", "Ocorreu um erro com a câmera." + ex.InnerException + ex.Message, "OK"); }
		}

        private async void BtnAddService_Clicked(object sender, EventArgs e)
        {
			if (!Validate()) {
				await DisplayAlert("Ocorreu um erro!", "Campos não podem ser nulos.", "OK");
				return;
			}
		
			Job job = new Job(UserController.CurrentUser.Id,TitleEntry.Text, UserController.CurrentUser.Phones.First().Number.ToString(),DescritionServiceEntry.Text,"0",DateTime.UtcNow,filePath);
			UserController.CurrentUser.Jobs.Add(job);
			CallbackStatus status = await JobController.AddAPI(UserController.CurrentUser);
			await DisplayAlert($"{ status.CurrentStatus.CallbackStatusToText()}", status.CallbackMessage, "OK");
		}

		private bool Validate() {
			if (TitleEntry.Text == null || DescritionServiceEntry.Text == null || filePath == null)
				return false;
			return true;
		}
    }
}