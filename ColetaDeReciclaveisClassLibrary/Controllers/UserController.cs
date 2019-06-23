using ColetaDeReciclaveisClassLibrary.Models;
using System;
using System.Collections.Generic;
using ColetaDeReciclaveisClassLibrary.Utils;
using SQLite;
using System.Threading.Tasks;

namespace ColetaDeReciclaveisClassLibrary.Controllers {
	public class UserController {

		public static User CurrentUser = new User();

		#region Database
		private static DatabaseConnection _Database = new DatabaseConnection();

		private static DatabaseConnectionAPI<User> _DatabaseApi = new DatabaseConnectionAPI<User>();
		#endregion

		#region Local
		private static void HasTable() {
			_Database.DATABASE_CONNECTION = new SQLiteConnection(StaticsInfo.DATABASE_PATH);
			_Database.Database.CreateTable<User>();
		}

		public static void DeleteAll() {
			HasTable();
			try {
				_Database.Database.DeleteAll<User>();
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public async static void SaveAll() {
			HasTable();

			List<User> Users = await GetAllAPI();

			try {
				foreach (User User in Users)
					_Database.Database.Insert(User);
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public static List<User> GetAll() {
			HasTable();
			try {
				return _Database.Database.Table<User>().ToList();
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public static List<User> GetAmount(int amount, int offset) {
			HasTable();
			try {
				return _Database.Database.Query<User>($"SELECT * FROM User LIMIT {amount} OFFSET {offset}");
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}
		#endregion

		#region API
		public async static Task<List<User>> GetAllAPI() {
			return await _DatabaseApi.GetAllDatabaseAPI(StaticsInfo.UserURL);
		}

		public async static Task<User> GetAPI(int id) {
			return await _DatabaseApi.GetDatabaseAPI(StaticsInfo.UserURL, id);
		}

		public async static Task<List<User>> GetAmountApi(int amount, int offset) {
			return await _DatabaseApi.GetAmountDatabaseAPI(StaticsInfo.UserURL, amount, offset);
		}

		public async static Task<CallbackStatus> UpdateAPI(User item) {
			return await _DatabaseApi.UpdateDatabaseAPI(StaticsInfo.UserURL, item);
		}

		public async static Task<CallbackStatus> DeleteAPI(int id) {
			return await _DatabaseApi.DeleteDatabaseAPI(StaticsInfo.UserURL, id);
		}

		public async static Task<CallbackStatus> AddAPI(User item) {
			return await _DatabaseApi.AddDatabaseAPI(StaticsInfo.UserURL, item);
		}
		#endregion
	}
}