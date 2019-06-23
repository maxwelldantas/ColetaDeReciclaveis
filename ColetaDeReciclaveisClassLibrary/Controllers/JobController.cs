using ColetaDeReciclaveisClassLibrary.Models;
using System;
using System.Collections.Generic;
using ColetaDeReciclaveisClassLibrary.Utils;
using SQLite;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace ColetaDeReciclaveisClassLibrary.Controllers {
	public class JobController {

		#region Database
		private static DatabaseConnection _Database = new DatabaseConnection();

		private static DatabaseConnectionAPI<Job> _DatabaseApi = new DatabaseConnectionAPI<Job>();
		#endregion

		#region Local
		private static void HasTable() {
			_Database.DATABASE_CONNECTION = new SQLiteConnection(StaticsInfo.DATABASE_PATH);
			_Database.Database.CreateTable<Job>();
		}

		public static void DeleteAll() {
			HasTable();
			try {
				_Database.Database.DeleteAll<Job>();
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public async static void SaveAll() {
			HasTable();

			List<Job> Jobs = await GetAllAPI();

			try {
				foreach (Job Job in Jobs)
					_Database.Database.Insert(Job);
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public static List<Job> GetAll() {
			HasTable();
			try {
				return _Database.Database.Table<Job>().ToList();
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public static List<Job> GetAmount(int amount, int offset) {
			HasTable();
			try {
				return _Database.Database.Query<Job>($"SELECT * FROM Job LIMIT {amount} OFFSET {offset}");
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}
		#endregion

		#region API
		public async static Task<List<Job>> GetAllAPI() {
			return await _DatabaseApi.GetAllDatabaseAPI(StaticsInfo.JobURL);
		}

		public async static Task<Job> GetAPI(int id) {
			return await _DatabaseApi.GetDatabaseAPI(StaticsInfo.JobURL, id);
		}

		public async static Task<List<Job>> GetAmountApi(int amount, int offset) {
			return await _DatabaseApi.GetAmountDatabaseAPI(StaticsInfo.JobURL, amount, offset);
		}

		public async static Task<CallbackStatus> UpdateAPI(Job item) {
			return await _DatabaseApi.UpdateDatabaseAPI(StaticsInfo.JobURL, item);
		}

		public async static Task<CallbackStatus> DeleteAPI(int id) {
			return await _DatabaseApi.DeleteDatabaseAPI(StaticsInfo.JobURL, id);
		}

		public async static Task<CallbackStatus> AddAPI(User item) {
			//	await GetAllDatabaseAPI(classURL);
			
				CallbackStatus status;
				HttpResponseMessage result = new HttpResponseMessage();
			try {
				using (HttpClient client = new HttpClient() { Timeout = new TimeSpan(0, 2, 0) }) {
					string serializedItem = JsonConvert.SerializeObject(item);
					StringContent content = new StringContent(serializedItem, Encoding.UTF8, "application/json");
					result = await client.PostAsync($"{StaticsInfo.ApiURL}/{StaticsInfo.JobURL}", content);
				}

				if (result.IsSuccessStatusCode) {
					status = new CallbackStatus(StaticsInfo.CALLBACK_STATUS.FINISHED, "Dados adicionados com sucesso.");
				} else {
					status = new CallbackStatus(StaticsInfo.CALLBACK_STATUS.FINISHED, "Não foi possível adicionar os dados.");
				}
			} catch (Exception ex) { status = new CallbackStatus(StaticsInfo.CALLBACK_STATUS.ERROR, $"{ex.Message}     {ex.InnerException}"); }

			return status;
		}
		#endregion
	}
}