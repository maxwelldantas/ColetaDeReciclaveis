using ColetaDeReciclaveisClassLibrary.Models;
using ColetaDeReciclaveisClassLibrary.Utils;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Collections.Generic;

namespace ColetaDeReciclaveisClassLibrary.Controllers {
	public class LoginController{

		public CallbackStatus status = new CallbackStatus();

		public int? Id;

		public async Task<CallbackStatus> Login(string login, string password){
			int? responseData = null;

			string json = $"{{ \"Login\":\"{login}\", \"Password\":\"{password}\" }}";

			try {
				HttpRequestMessage request = new HttpRequestMessage {
					Method = HttpMethod.Post,
					RequestUri = new Uri($"{StaticsInfo.ApiURL}/{StaticsInfo.LoginURL}"),
					Content = new StringContent(json, Encoding.UTF8, "application/json")
				};

				using (HttpClient client = new HttpClient() { Timeout = new TimeSpan(0,2,0) }) {
					using (HttpResponseMessage response = await client.SendAsync(request)) {
						if (response.IsSuccessStatusCode) {
							string dataString = response.Content.ReadAsStringAsync().Result;

							//responseData = JsonConvert.DeserializeObject<int?>(dataString);
							int i;
							if (int.TryParse(dataString, out i)) {
								responseData = i;
								status = new CallbackStatus(StaticsInfo.CALLBACK_STATUS.FINISHED, "Login realizado com sucesso.");
							} else {
								responseData = null;
								status = new CallbackStatus(StaticsInfo.CALLBACK_STATUS.FINISHED, "Não foi possível realizar o login. \n Revise suas informações!");
							}

						} else {
							status = new CallbackStatus(StaticsInfo.CALLBACK_STATUS.FINISHED_WITH_ERROR, $"Não foi possível realizar o login: {response.StatusCode}");
						}
					}
				}
			}catch(Exception ex){ status = new CallbackStatus(StaticsInfo.CALLBACK_STATUS.ERROR, $"{ex.Message}     {ex.InnerException}"); }
			Id = responseData;

			return status;
		}
	}
}
