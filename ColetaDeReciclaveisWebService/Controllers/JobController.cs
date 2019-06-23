using System;
using System.Collections.Generic;
using System.Linq;
using ColetaDeReciclaveisClassLibrary.Models;
using ColetaDeReciclaveisClassLibrary.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace ColetaDeReciclaveisWebService.Controllers {

	[Route("api/[controller]")]
	[ApiController]
	[AllowAnonymous]
	public class JobController : ControllerBase {

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
		public void Post([FromBody] User item) {
			User user = item;

			List<Job> jobs = item.Jobs;

			foreach(Job job in jobs){
				if(job.Id <= 0)
					DatabaseAdd(job);
			}
			user.Jobs = jobs;

			_Database.Database.UpdateWithChildren(user);
		}

		[HttpPut]
		public void Put([FromBody] Job item) {
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
			_Database.Database.CreateTable<Job>();
		}

		public int DatabaseAdd(Job item) {
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
				_Database.Database.DeleteAll<Job>();
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public int DatabaseUpdate(Job item) {
			HasTable();
			try {
				return _Database.Database.Update(item);
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public Job DatabaseGet(int id) {
			HasTable();
			try {
				return _Database.Database.GetWithChildren<Job>(id);
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public List<Job> DatabaseGetAmount(int amount = 0, int offset = 0) {
			HasTable();
			try {
				return _Database.Database.Query<Job>($"SELECT * FROM Job " +
											$"LIMIT {amount} OFFSET {offset}");
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public List<Job> DatabaseGetAll() {
			HasTable();
			try {
				return _Database.Database.GetAllWithChildren<Job>();
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		#endregion
	}
}
