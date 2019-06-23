using ColetaDeReciclaveisClassLibrary.Models;
using System;
using System.Collections.Generic;
using ColetaDeReciclaveisClassLibrary.Utils;
using SQLite;
using System.Threading.Tasks;

namespace ColetaDeReciclaveisClassLibrary.Controllers {
	public class PhoneController {

		#region Database
		private static DatabaseConnection _Database = new DatabaseConnection();

		private static DatabaseConnectionAPI<Phone> _DatabaseApi = new DatabaseConnectionAPI<Phone>();
		#endregion

		#region Local
		private static void HasTable() {
			_Database.DATABASE_CONNECTION = new SQLiteConnection(StaticsInfo.DATABASE_PATH);
			_Database.Database.CreateTable<Phone>();
		}

		public static void DeleteAll() {
			HasTable();
			try {
				_Database.Database.DeleteAll<Phone>();
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public async static void SaveAll() {
			HasTable();

			List<Phone> Phones = await GetAllAPI();

			try {
				foreach (Phone Phone in Phones)
					_Database.Database.Insert(Phone);
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public static List<Phone> GetAll() {
			HasTable();
			try {
				return _Database.Database.Table<Phone>().ToList();
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public static List<Phone> GetAmount(int amount, int offset) {
			HasTable();
			try {
				return _Database.Database.Query<Phone>($"SELECT * FROM Phone LIMIT {amount} OFFSET {offset}");
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}
		#endregion

		#region API
		public async static Task<List<Phone>> GetAllAPI() {
			return await _DatabaseApi.GetAllDatabaseAPI(StaticsInfo.PhoneURL);
		}

		public async static Task<Phone> GetAPI(int id) {
			return await _DatabaseApi.GetDatabaseAPI(StaticsInfo.PhoneURL, id);
		}

		public async static Task<List<Phone>> GetAmountApi(int amount, int offset) {
			return await _DatabaseApi.GetAmountDatabaseAPI(StaticsInfo.PhoneURL, amount, offset);
		}

		public async static Task<CallbackStatus> UpdateAPI(Phone item) {
			return await _DatabaseApi.UpdateDatabaseAPI(StaticsInfo.PhoneURL, item);
		}

		public async static Task<CallbackStatus> DeleteAPI(int id) {
			return await _DatabaseApi.DeleteDatabaseAPI(StaticsInfo.PhoneURL, id);
		}

		public async static Task<CallbackStatus> AddAPI(Phone item) {
			return await _DatabaseApi.AddDatabaseAPI(StaticsInfo.PhoneURL, item);
		}
		#endregion
	}
}