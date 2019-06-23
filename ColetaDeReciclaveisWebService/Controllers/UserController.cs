using System;
using System.Collections.Generic;
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
	public class UserController : ControllerBase {

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
			_Database.Database.CreateTable<Job>();
			_Database.Database.CreateTable<EcoPoint>();
			_Database.Database.CreateTable<EcoPointMaterial>();
			DatabaseAdd(user);

			List<Phone> phones = item.Phones;
			List<Address> addresses = item.Addresses;

			PhoneController phoneController = new PhoneController();
			AddressController addressController = new AddressController();

			foreach (Phone phone in phones) {
				phoneController.DatabaseAdd(phone);
			}

			foreach (Address address in addresses) {
				addressController.DatabaseAdd(address);
			}

			user.Phones = phones;
			user.Addresses = addresses;

			_Database.Database.UpdateWithChildren(user);
		}

		[HttpPut]
		public void Put([FromBody] User item) {
			_Database.Database.UpdateWithChildren(item);
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
			_Database.Database.CreateTable<User>();
		}

		public int DatabaseAdd(User item) {
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
				_Database.Database.DeleteAll<User>();
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public int DatabaseUpdate(User item) {
			HasTable();
			try {
				return _Database.Database.Update(item);
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public User DatabaseGet(int id) {
			HasTable();
			try {
				return _Database.Database.GetWithChildren<User>(id);
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public List<User> DatabaseGetAmount(int amount = 0, int offset = 0) {
			HasTable();
			try {
				return _Database.Database.Query<User>($"SELECT * FROM User as u, Phone as p, Address as a WHERE u.Id = p.UserId AND u.Id = a.UserId " +
														$"LIMIT {amount} OFFSET {offset}");
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public List<User> DatabaseGetAll() {
			HasTable();
			try {
				return _Database.Database.GetAllWithChildren<User>();
			} catch (Exception ex) {
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		#endregion
	}
}
