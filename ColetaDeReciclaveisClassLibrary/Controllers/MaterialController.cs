using ColetaDeReciclaveisClassLibrary.Models;
using System;
using System.Collections.Generic;
using ColetaDeReciclaveisClassLibrary.Utils;
using SQLite;
using System.Threading.Tasks;

namespace ColetaDeReciclaveisClassLibrary.Controllers {
	public class MaterialController {

		#region Database
		private static DatabaseConnection _Database = new DatabaseConnection();

		private static DatabaseConnectionAPI<Material> _DatabaseApi = new DatabaseConnectionAPI<Material>();
		#endregion

		#region Local
		private static void HasTable() {
			_Database.DATABASE_CONNECTION = new SQLiteConnection(StaticsInfo.DATABASE_PATH);
			_Database.Database.CreateTable<Material>();
		}

		public static void DeleteAll() {
			HasTable();
			try {
				_Database.Database.DeleteAll<Material>();
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public async static void SaveAll() {
			HasTable();

			List<Material> Materials = await GetAllAPI();

			try {
				foreach (Material Material in Materials)
					_Database.Database.Insert(Material);
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public static List<Material> GetAll() {
			HasTable();
			try {
				return _Database.Database.Table<Material>().ToList();
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public static List<Material> GetAmount(int amount, int offset) {
			HasTable();
			try {
				return _Database.Database.Query<Material>($"SELECT * FROM Material LIMIT {amount} OFFSET {offset}");
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}
		#endregion

		#region API
		public async static Task<List<Material>> GetAllAPI() {
			return await _DatabaseApi.GetAllDatabaseAPI(StaticsInfo.MaterialURL);
		}

		public async static Task<Material> GetAPI(int id) {
			return await _DatabaseApi.GetDatabaseAPI(StaticsInfo.MaterialURL, id);
		}

		public async static Task<List<Material>> GetAmountApi(int amount, int offset) {
			return await _DatabaseApi.GetAmountDatabaseAPI(StaticsInfo.MaterialURL, amount, offset);
		}

		public async static Task<CallbackStatus> UpdateAPI(Material item) {
			return await _DatabaseApi.UpdateDatabaseAPI(StaticsInfo.MaterialURL, item);
		}

		public async static Task<CallbackStatus> DeleteAPI(int id) {
			return await _DatabaseApi.DeleteDatabaseAPI(StaticsInfo.MaterialURL, id);
		}

		public async static Task<CallbackStatus> AddAPI(Material item) {
			return await _DatabaseApi.AddDatabaseAPI(StaticsInfo.MaterialURL, item);
		}
		#endregion
	}
}