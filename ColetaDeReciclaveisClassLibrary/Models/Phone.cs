using SQLite;
using System;
using ColetaDeReciclaveisClassLibrary.Controllers;
using SQLiteNetExtensions.Attributes;

namespace ColetaDeReciclaveisClassLibrary.Models {

	[Table("Phone")]
	public class Phone : PhoneController {
		private int _Id;
		private DateTime _CreatedAt;
		private DateTime _UpdatedAt;

		private string _Number;

		[PrimaryKey, AutoIncrement, Unique]
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

		[NotNull]
		public string Number {
			get => _Number;
			set => _Number = value;
		}

		[ForeignKey(typeof(User))] public int UserId { get; set; }

		[ForeignKey(typeof(EcoPoint))] public int EcoPointId { get; set; }

		public Phone(string Number) {
			_Number = Number ?? throw new ArgumentNullException(nameof(Number));
		}

		public Phone() {
		}
	}
}
