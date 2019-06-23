using System;
using System.Collections.Generic;
using SQLite;
using ColetaDeReciclaveisClassLibrary.Controllers;
using SQLiteNetExtensions.Attributes;

namespace ColetaDeReciclaveisClassLibrary.Models {
	[Table("User")]
	public class User : UserController{
		#region InfosToDatabase
		private int _Id;
		private DateTime _CreatedAt;
		private DateTime _UpdatedAt;
		#endregion

		#region UserDetails
		private string _FullName;
		private string _CPF;
		private string _Email;
		private string _About;
		private DateTime _Birthday;
		#endregion

		#region UserDetailsLists
		private List<Phone> _Phones;
		private List<Address> _Addresses;
		private List<Job> _Jobs;
		#endregion

		#region UserDetailsChecks
		private bool _IsAppAdmin;
		private bool _IsDbaAdmin;
		private bool _IsColector;
		private bool _IsDonator;
		#endregion

		#region UserLogin
		private string _Login;
		private string _Password;
		#endregion

		[PrimaryKey,AutoIncrement,Unique]
		public int Id {
			get => _Id;
			set => _Id = value;
		}

		public DateTime CreatedAt {
			get => _CreatedAt;
			set => _CreatedAt = value;
		}

		public DateTime UpdatedAt {
			get => _UpdatedAt;
			set => _UpdatedAt = value;
		}

		[MaxLength(100),NotNull]
		public string FullName {
			get => _FullName;
			set => _FullName = value;
		}

		[MaxLength(14),Unique,NotNull]
		public string CPF {
			get => _CPF;
			set => _CPF = value;
		}

		[MaxLength(100),Unique,NotNull]
		public string Email {
			get => _Email;
			set => _Email = value;
		}

		[MaxLength(300)]
		public string About {
			get => _About;
			set => _About = value;
		}

		[NotNull]
		public DateTime Birthday {
			get => _Birthday;
			set => _Birthday = value;
		}

		public bool IsAppAdmin {
			get => _IsAppAdmin;
			set => _IsAppAdmin = value;
		}

		public bool IsDbaAdmin {
			get => _IsDbaAdmin;
			set => _IsDbaAdmin = value;
		}

		public bool IsColector {
			get => _IsColector;
			set => _IsColector = value;
		}

		public bool IsDonator {
			get => _IsDonator;
			set => _IsDonator = value;
		}

		[MaxLength(20),Unique,NotNull]
		public string Login {
			get => _Login;
			set => _Login = value;
		}

		[MaxLength(30),NotNull]
		public string Password {
			get => _Password;
			set => _Password = value;
		}

		[OneToMany(CascadeOperations = CascadeOperation.CascadeDelete)]
		public List<Phone> Phones {
			get => _Phones;
			set => _Phones = value;
		}

		[OneToMany(CascadeOperations = CascadeOperation.CascadeDelete)]
		public List<Address> Addresses {
			get => _Addresses;
			set => _Addresses = value;
		}

		[OneToMany(CascadeOperations = CascadeOperation.CascadeDelete)]
		public List<Job> Jobs {
			get => _Jobs;
			set => _Jobs = value;
		}

		[OneToMany(CascadeOperations = CascadeOperation.CascadeDelete)]
		public List<EcoPoint> EcoPoints {get;set;}

		public User(string FullName, string CPF, string Email, string About, DateTime Birthday, bool IsColector, bool IsDonator, string Login, string Password) {
			this.FullName = FullName ?? throw new ArgumentNullException(nameof(FullName));
			this.CPF = CPF ?? throw new ArgumentNullException(nameof(CPF));
			this.Email = Email ?? throw new ArgumentNullException(nameof(Email));
			this.About = About ?? throw new ArgumentNullException(nameof(About));
			this.Birthday = Birthday;
			this.IsColector = IsColector;
			this.IsDonator = IsDonator;
			this.Login = Login ?? throw new ArgumentNullException(nameof(Login));
			this.Password = Password ?? throw new ArgumentNullException(nameof(Password));
			this.Addresses = new List<Address>();
			this.Phones = new List<Phone>();
			this.Jobs = new List<Job>();
		}

		public User() {
		}

		public User(string Login, string Password)
		{
			if(Login == null)
			{
				throw new ArgumentNullException(nameof(Login));
			}
			this.Login = Login;
			this.Password = Password ?? throw new ArgumentNullException(nameof(Password));
		}
	}
}
