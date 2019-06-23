using ColetaDeReciclaveisClassLibrary.Models;
using ColetaDeReciclaveisClassLibrary.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using SQLite;
using Microsoft.AspNetCore.Authorization;

namespace ColetaDeReciclaveisWebService.Controllers{

	[Route("api/[controller]")]
	[AllowAnonymous]
	public class LoginController {

		private CallbackStatus _Callback = new CallbackStatus(StaticsInfo.CALLBACK_STATUS.NONE, "");

		private DatabaseConnection _Database = new DatabaseConnection();

		private UserController userController = new UserController();

		[HttpGet]
		[HttpPost]
		public ActionResult<string> GetUser([FromBody] User user)
		{
			_Database.Database = new SQLiteConnection(StaticsInfo.DATABASE_PATH_API);
			User finded = null;
			try
			{
				finded = userController.DatabaseGetAll().First(x => x.Login == user.Login && x.Password == user.Password);
			}
			catch { }
			try
			{
				finded = userController.DatabaseGetAll().First(x => x.Email == user.Login && x.Password == user.Password);
			}
			catch { }

			int? id = finded != null ? finded.Id : new int?();

			return JsonConvert.SerializeObject(id);
		}

    }
}
