using SQLite;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text;
using ColetaDeReciclaveisClassLibrary.Models;

namespace ColetaDeReciclaveisClassLibrary.Utils {

	public class DatabaseConnection{
		public SQLiteConnection DATABASE_CONNECTION = new SQLiteConnection(StaticsInfo.DATABASE_PATH);
		public SQLiteConnection Database {
			get => DATABASE_CONNECTION != null ? DATABASE_CONNECTION : throw new ArgumentNullException("Necessário um conexão válida com o banco de dados.");
			set => DATABASE_CONNECTION = value;
		}
	}

	public class DatabaseConnectionAPI<T> where T : new(){

		public async Task<T> GetDatabaseAPI(string classURL, int id) {
			T responseData = new T();

			using (HttpClient client = new HttpClient() { Timeout = new TimeSpan(0,2,0)}) {
				using (HttpResponseMessage response = await client.GetAsync($"{StaticsInfo.ApiURL}/{classURL}/{id}")) {
					if (response.IsSuccessStatusCode) {
						string dataString = await response.Content.ReadAsStringAsync();

						responseData = JsonConvert.DeserializeObject<T>(dataString);

					}
				}
			}

			return responseData;
		}

		public async Task<List<T>> GetAllDatabaseAPI(string classURL) {
			List<T> responseData = new List<T>();

			using (HttpClient client = new HttpClient() { Timeout = new TimeSpan(0,2,0)}) {
				using (HttpResponseMessage response = await client.GetAsync($"{StaticsInfo.ApiURL}/{classURL}")) {
					if (response.IsSuccessStatusCode) {
						string dataString = await response.Content.ReadAsStringAsync();

						responseData = JsonConvert.DeserializeObject<List<T>>(dataString);

					}
				}
			}

			//foreach (T t in responseData) {
			//	DatabaseConnection<T>.DatabaseAdd(t, out status);
			//}

			return responseData;
		}
		
		public async Task<List<T>> GetAmountDatabaseAPI(string classURL, int amount, int offset) {
			List<T> responseData = new List<T>();

			using (HttpClient client = new HttpClient() { Timeout = new TimeSpan(0,2,0)}) {
				using (HttpResponseMessage response = await client.GetAsync($"{StaticsInfo.ApiURL}/{classURL}/{amount}/{offset}")) {
					if (response.IsSuccessStatusCode) {
						string dataString = await response.Content.ReadAsStringAsync();

						responseData = JsonConvert.DeserializeObject<List<T>>(dataString);

					}
				}
			}

			//foreach (T t in responseData) {
			//	DatabaseConnection<T>.DatabaseAdd(t, out status);
			//}

			return responseData;
		}

		public async Task<CallbackStatus> UpdateDatabaseAPI(string classURL, T item) {

			CallbackStatus status;

			using (HttpClient client = new HttpClient() { Timeout = new TimeSpan(0,2,0)}) {
				string serializedItem = JsonConvert.SerializeObject(item);
				StringContent content = new StringContent(serializedItem, Encoding.UTF8, "application/json");

				HttpResponseMessage responseMessage = await client.PutAsync($"{StaticsInfo.ApiURL}/{classURL}",content);
				if (responseMessage.IsSuccessStatusCode) {
					status = new CallbackStatus(StaticsInfo.CALLBACK_STATUS.FINISHED, "O item foi atualizado com sucesso.");
				} else {
					status = new CallbackStatus(StaticsInfo.CALLBACK_STATUS.FINISHED, $"Falha ao atualizar o item. {responseMessage.StatusCode}");
					return status;
				}
			}

			return status;
		}

		public async Task<CallbackStatus> DeleteDatabaseAPI(string classURL, int id) {

			CallbackStatus status;
			
			using (HttpClient client = new HttpClient() { Timeout = new TimeSpan(0,2,0)}) {
				HttpResponseMessage responseMessage = await client.DeleteAsync($"{StaticsInfo.ApiURL}/{classURL}/{id}");
				if (responseMessage.IsSuccessStatusCode) {
					status = new CallbackStatus(StaticsInfo.CALLBACK_STATUS.FINISHED, "O item foi deletado com sucesso.");
				} else {
					status = new CallbackStatus(StaticsInfo.CALLBACK_STATUS.FINISHED, $"Falha ao excluir o item. {responseMessage.StatusCode}");
					return status;
				}
			}

			return status;
		}

		public async Task<CallbackStatus> AddDatabaseAPI(string classURL, T item) {
			//	await GetAllDatabaseAPI(classURL);
			CallbackStatus status;
			HttpResponseMessage result = new HttpResponseMessage();

			using (HttpClient client = new HttpClient() { Timeout = new TimeSpan(0,2,0)}) {
				string serializedItem = JsonConvert.SerializeObject(item);
				StringContent content = new StringContent(serializedItem, Encoding.UTF8, "application/json");
				result = await client.PostAsync($"{StaticsInfo.ApiURL}/{classURL}", content);
			}

			if (result.IsSuccessStatusCode) {
				status = new CallbackStatus(StaticsInfo.CALLBACK_STATUS.FINISHED, "Item adicionado com sucesso.");
			} else {
				status = new CallbackStatus(StaticsInfo.CALLBACK_STATUS.FINISHED, "Não foi possível adicionar os dados.");
			}

			return status;
		}

	}
}