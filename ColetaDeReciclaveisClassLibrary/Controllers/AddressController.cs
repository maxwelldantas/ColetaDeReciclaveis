using ColetaDeReciclaveisClassLibrary.Models;
using System;
using System.Collections.Generic;
using ColetaDeReciclaveisClassLibrary.Utils;
using SQLite;
using System.Threading.Tasks;

namespace ColetaDeReciclaveisClassLibrary.Controllers {
	public class AddressController {

		#region Database
		private static DatabaseConnection _Database = new DatabaseConnection();

		private static DatabaseConnectionAPI<Address> _DatabaseApi = new DatabaseConnectionAPI<Address>();
		#endregion

		#region Local
		private static void HasTable() {
			_Database.DATABASE_CONNECTION = new SQLiteConnection(StaticsInfo.DATABASE_PATH);
			_Database.Database.CreateTable<Address>();
		}

		public static void DeleteAll() {
			HasTable();
			try {
				_Database.Database.DeleteAll<Address>();
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public async static void SaveAll() {
			HasTable();

			List<Address> Addresss = await GetAllAPI();

			try {
				foreach (Address Address in Addresss)
					_Database.Database.Insert(Address);
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public static List<Address> GetAll() {
			HasTable();
			try {
				return _Database.Database.Table<Address>().ToList();
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public static List<Address> GetAmount(int amount, int offset) {
			HasTable();
			try {
				return _Database.Database.Query<Address>($"SELECT * FROM Address LIMIT {amount} OFFSET {offset}");
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}
		#endregion

		#region API
		public async static Task<List<Address>> GetAllAPI() {
			return await _DatabaseApi.GetAllDatabaseAPI(StaticsInfo.AddressURL);
		}

		public async static Task<Address> GetAPI(int id) {
			return await _DatabaseApi.GetDatabaseAPI(StaticsInfo.AddressURL, id);
		}

		public async static Task<List<Address>> GetAmountApi(int amount, int offset) {
			return await _DatabaseApi.GetAmountDatabaseAPI(StaticsInfo.AddressURL, amount, offset);
		}

		public async static Task<CallbackStatus> UpdateAPI(Address item) {
			return await _DatabaseApi.UpdateDatabaseAPI(StaticsInfo.AddressURL, item);
		}

		public async static Task<CallbackStatus> DeleteAPI(int id) {
			return await _DatabaseApi.DeleteDatabaseAPI(StaticsInfo.AddressURL, id);
		}

		public async static Task<CallbackStatus> AddAPI(Address item) {
			return await _DatabaseApi.AddDatabaseAPI(StaticsInfo.AddressURL, item);
		}
		#endregion
	}
}