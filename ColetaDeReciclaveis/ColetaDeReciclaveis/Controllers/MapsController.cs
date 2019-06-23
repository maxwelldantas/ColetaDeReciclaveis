using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ColetaDeReciclaveis.Controllers {
	public static class MapsController {

		public static List<Pin> CustomPins = new List<Pin>();

		public static Map CustomMap = new Map();

		private static Slider _Slider = new Slider();

		public static void AddPin(Position position, string label, string addressAndInfos) {
			Pin pin = new Pin {
				Type = PinType.Place,
				Position = position,
				Label = label,
				Address = addressAndInfos
			};
			CustomPins.Add(pin);

		}

		public async static Task<Position> GetLocationFromAddress(string address) {
			try {
				IEnumerable<Xamarin.Essentials.Location> locations = await Xamarin.Essentials.Geocoding.GetLocationsAsync(address);

				Xamarin.Essentials.Location location = locations?.FirstOrDefault();
				if (location != null) {
					return new Position(location.Latitude, location.Longitude);
				} else {
					return new Position();//throw new Exception("Impossible to get address results. 404");
				}
			} catch (Xamarin.Essentials.FeatureNotSupportedException fnsEx) {
				// Feature not supported on device
				return new Position();//throw new Xamarin.Essentials.FeatureNotSupportedException(fnsEx.Message, fnsEx.InnerException);
			} catch (Exception ex) {
				// Handle exception that may have occurred in geocoding
				return new Position();//throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public async static Task<Map> InstantiateNewMap(StackLayout content) {

			Position currentPosition = await GetCurrentLocation();

			CustomMap = new Map(
			MapSpan.FromCenterAndRadius(currentPosition, Distance.FromKilometers(1))) {
				IsShowingUser = true,
				HeightRequest = content.Height,
				WidthRequest = content.Width,
				VerticalOptions = LayoutOptions.FillAndExpand,
				MapType = MapType.Street
			};
			content.Children.Add(CustomMap); 

			_Slider = new Slider(1, 18, 1);
			_Slider.ValueChanged += (sender, e) => {
				double zoomLevel = e.NewValue; // between 1 and 18
				double latlongdegrees = 360 / (Math.Pow(2, zoomLevel));
				CustomMap.MoveToRegion(new MapSpan(CustomMap.VisibleRegion.Center, latlongdegrees, latlongdegrees));
			};

			return null;
		}

		public async static Task<Position> GetCurrentLocation() {
			Xamarin.Essentials.Location location = new Xamarin.Essentials.Location();
			try {
				location = await Xamarin.Essentials.Geolocation.GetLastKnownLocationAsync();
				return new Position(location.Latitude, location.Longitude);
			} catch (Xamarin.Essentials.FeatureNotSupportedException fnsEx) {
				throw new Exception(fnsEx.Message, fnsEx.InnerException);
			} catch (Xamarin.Essentials.FeatureNotEnabledException fneEx) {
				throw new Exception(fneEx.Message, fneEx.InnerException);
			} catch (Xamarin.Essentials.PermissionException pEx) {
				throw new Exception(pEx.Message, pEx.InnerException);
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}

			

		}
	}
}
