using SQLite;
using System;
using ColetaDeReciclaveisClassLibrary.Controllers;
using SQLiteNetExtensions.Attributes;

namespace ColetaDeReciclaveisClassLibrary.Models {

	[Table("Address")]
	public class Address : AddressController {

		#region privateVariables

		private int _Id;
		private DateTime _CreatedAt;
		private DateTime _UpdatedAt;

		private string _CEP;
		private string _AddressStreet;
		private string _Number;
		private string _Complement;
		private string _District;
		private string _City;
		private string _State;

		private const string _Country = "Brasil";

		#endregion

		[PrimaryKey, AutoIncrement,Unique]
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

		[NotNull,MaxLength(9)]
		public string CEP {
			get => _CEP;
			set => _CEP = value;
		}

		[NotNull]
		public string AddressStreet {
			get => _AddressStreet;
			set => _AddressStreet = value;
		}

		[NotNull]
		public string Number {
			get => _Number;
			set => _Number = value;
		}
		public string Complement {
			get => _Complement;
			set => _Complement = value;
		}

		[NotNull]
		public string District {
			get => _District;
			set => _District = value;
		}

		[NotNull]
		public string City {
			get => _City;
			set => _City = value;
		}

		[NotNull]
		public string State {
			get => _State;
			set => _State = value;
		}

		[NotNull,Ignore]
		public static string Country => _Country;

		[ForeignKey(typeof(User))] public int UserId { get; set; }

		[ForeignKey(typeof(EcoPoint))] public int EcoPointId { get; set; }

		public Address(string cep, string addressStreet, string number, string complement, string district, string city, string state) {
			this.CEP = cep ?? throw new ArgumentNullException(nameof(cep));
			this.AddressStreet = addressStreet ?? throw new ArgumentNullException(nameof(addressStreet));
			this.Number = number ?? throw new ArgumentNullException(nameof(number));
			this.Complement = complement ?? throw new ArgumentNullException(nameof(complement));
			this.District = district ?? throw new ArgumentNullException(nameof(district));
			this.City = city ?? throw new ArgumentNullException(nameof(city));
			this.State = state ?? throw new ArgumentNullException(nameof(state));
		}

		public Address() {
		}

	}
}
