using ColetaDeReciclaveisClassLibrary.Models;
using System;
using System.Collections.Generic;
using ColetaDeReciclaveisClassLibrary.Utils;
using SQLite;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;

namespace ColetaDeReciclaveisClassLibrary.Controllers {
	public class EcoPointController {

		#region Database
		private static DatabaseConnection _Database = new DatabaseConnection();

		private static DatabaseConnectionAPI<EcoPoint> _DatabaseApi = new DatabaseConnectionAPI<EcoPoint>();
		#endregion

		#region Local
		private static void HasTable() {
			_Database.DATABASE_CONNECTION = new SQLiteConnection(StaticsInfo.DATABASE_PATH);
			_Database.Database.CreateTable<EcoPoint>();
		}

		public static void DeleteAll() {
			HasTable();
			try {
				_Database.Database.DeleteAll<EcoPoint>();
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public async static void SaveAll() {
			HasTable();

			List<EcoPoint> ecoPoints = await GetAllAPI();

			try {
				foreach (EcoPoint ecoPoint in ecoPoints)
					_Database.Database.Insert(ecoPoint);
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public static List<EcoPoint> GetAll() {
			HasTable();
			try {
				return _Database.Database.Table<EcoPoint>().ToList();
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public static List<EcoPoint> GetAmount(int amount, int offset) {
			HasTable();
			try {
				return _Database.Database.Query<EcoPoint>($"SELECT * FROM EcoPoint LIMIT {amount} OFFSET {offset}");
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}
		#endregion

		#region API
		public async static Task<List<EcoPoint>> GetAllAPI() {
			return await _DatabaseApi.GetAllDatabaseAPI(StaticsInfo.EcoPointURL);
		}
		
		public async static Task<EcoPoint> GetAPI(int id) {
			return await _DatabaseApi.GetDatabaseAPI(StaticsInfo.EcoPointURL,id);
		}
		
		public async static Task<List<EcoPoint>> GetAmountApi(int amount, int offset) {
			return await _DatabaseApi.GetAmountDatabaseAPI(StaticsInfo.EcoPointURL,amount,offset);
		}
		
		public async static Task<CallbackStatus> UpdateAPI(EcoPoint item) {
			return await _DatabaseApi.UpdateDatabaseAPI(StaticsInfo.EcoPointURL, item);
		}
		
		public async static Task<CallbackStatus> DeleteAPI(int id) {
			return await _DatabaseApi.DeleteDatabaseAPI(StaticsInfo.EcoPointURL,id);
		}
		
		public async static Task<CallbackStatus> AddAPI(User item) {
			//	await GetAllDatabaseAPI(classURL);
			CallbackStatus status;
			HttpResponseMessage result = new HttpResponseMessage();
			try {
				using (HttpClient client = new HttpClient() { Timeout = new TimeSpan(0, 2, 0) }) {
					string serializedItem = JsonConvert.SerializeObject(item);
					StringContent content = new StringContent(serializedItem, Encoding.UTF8, "application/json");
					result = await client.PostAsync($"{StaticsInfo.ApiURL}/{StaticsInfo.EcoPointURL}", content);
				}

				if (result.IsSuccessStatusCode) {
					status = new CallbackStatus(StaticsInfo.CALLBACK_STATUS.FINISHED, "Item adicionado com sucesso.");
				} else {
					status = new CallbackStatus(StaticsInfo.CALLBACK_STATUS.FINISHED, "Não foi possível adicionar os dados.");
				}
			} catch (Exception ex) { status = new CallbackStatus(StaticsInfo.CALLBACK_STATUS.ERROR, $"{ex.Message}     {ex.InnerException}"); }

			return status;
		}
		#endregion
	}
}