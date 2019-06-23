using System;
using System.Collections.Generic;
using ColetaDeReciclaveisClassLibrary.Models;
using ColetaDeReciclaveisClassLibrary.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SQLite;

namespace ColetaDeReciclaveisWebService.Controllers {

	[Route("api/[controller]")]
	[ApiController]
	[AllowAnonymous]
	public class MaterialController : ControllerBase {

		#region API

		[HttpGet]
		public ActionResult<string> Get() {
			string json = JsonConvert.SerializeObject(DatabaseGetAll(), Formatting.Indented);
			return json;
		}

		[HttpGet("{id}")]
		public ActionResult<string> Get(int id) {
			string json = JsonConvert.SerializeObject(DatabaseGet(id));
			return json;
		}

		[HttpGet("{amount}/{from}")]
		public ActionResult<string> Get(int amount, int offset) {
			string json = JsonConvert.SerializeObject(DatabaseGetAmount(amount, offset), Formatting.Indented);
			return json;
		}

		[HttpPost]
		public void Post([FromBody] Material item) {
			DatabaseAdd(item);
		}

		[HttpPut]
		public void Put([FromBody] Material item) {
			DatabaseUpdate(item);
		}

		[HttpDelete("{id}")]
		public void Delete(int id) {
			DatabaseDelete(id);
		}

		#endregion

		#region LocalAPI
		private DatabaseConnection _Database = new DatabaseConnection();

		private void HasTable() {
			_Database.DATABASE_CONNECTION = new SQLiteConnection(StaticsInfo.DATABASE_PATH_API);
			_Database.Database.CreateTable<Material>();
		}

		public int DatabaseAdd(Material item) {
			HasTable();
			try {
				return _Database.Database.Insert(item);
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public void DatabaseDelete(int id) {
			HasTable();
			try {
				_Database.Database.Delete(id);
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public void DatabaseDeleteAll() {
			HasTable();
			try {
				_Database.Database.DeleteAll<Material>();
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public int DatabaseUpdate(Material item) {
			HasTable();
			try {
				return _Database.Database.Update(item);
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public Material DatabaseGet(int id) {
			HasTable();
			try {
				return _Database.Database.Get<Material>(id);
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public List<Material> DatabaseGetAmount(int amount = 0, int offset = 0) {
			HasTable();
			try {
				return _Database.Database.Query<Material>($"SELECT * FROM Material LIMIT {amount} OFFSET {offset}");
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public List<Material> DatabaseGetAll() {
			HasTable();
			try {
				return _Database.Database.Table<Material>().ToList();
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		#endregion
	}
}
