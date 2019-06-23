using System;
using System.Collections.Generic;
using ColetaDeReciclaveisClassLibrary.Models;
using ColetaDeReciclaveisClassLibrary.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace ColetaDeReciclaveisWebService.Controllers{

	[Route("api/[controller]")]
	[ApiController]
	[AllowAnonymous]
	public class EcopointController : ControllerBase{

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
			string json = JsonConvert.SerializeObject(DatabaseGetAmount(amount,offset), Formatting.Indented);
			return json;
		}

		[HttpPost]
		public void Post([FromBody] User item) {
			User user = item;

			List<EcoPoint> ecos = item.EcoPoints;

			AddressController addressController = new AddressController();
			PhoneController phoneController = new PhoneController();

			foreach (EcoPoint eco in ecos) {
				if (eco.Id <= 0) {
					DatabaseAdd(eco);
					addressController.DatabaseAdd(eco.Address);
					phoneController.DatabaseAdd(eco.Phones);
					_Database.Database.UpdateWithChildren(eco);
				}
			}
			user.EcoPoints = ecos;

			_Database.Database.UpdateWithChildren(user);
		}

		[HttpPut]
		public void Put([FromBody] EcoPoint item) {
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
			_Database.Database.CreateTable<EcoPoint>();
		}

		public int DatabaseAdd(EcoPoint item) {
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
				_Database.Database.DeleteAll<EcoPoint>();
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public int DatabaseUpdate(EcoPoint item) {
			HasTable();
			try {
				return _Database.Database.Update(item);
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public EcoPoint DatabaseGet(int id) {
			HasTable();
			try {
				return _Database.Database.GetWithChildren<EcoPoint>(id);
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public List<EcoPoint> DatabaseGetAmount(int amount = 0, int offset = 0) {
			HasTable();
			try {
				return _Database.Database.Query<EcoPoint>($"SELECT * FROM EcoPoint as e, Phone as p, Address as a WHERE e.Id = p.UserId AND e.Id = a.UserId " +
											$"LIMIT {amount} OFFSET {offset}");
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public List<EcoPoint> DatabaseGetAll() {
			HasTable();
			try {
				return _Database.Database.GetAllWithChildren<EcoPoint>();
			} catch (Exception ex) {
				return new List<EcoPoint>();
			}
			
		}

	#endregion
	}
}
