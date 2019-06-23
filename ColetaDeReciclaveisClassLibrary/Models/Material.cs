using SQLite;
using System;
using ColetaDeReciclaveisClassLibrary.Controllers;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace ColetaDeReciclaveisClassLibrary.Models {

	[Table("Material")]
	public class Material : MaterialController {
		private int _Id;
		private DateTime _CreatedAt;
		private DateTime _UpdatedAt;

		private string _Name;
		private string _Description;

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

		[NotNull,Unique]
		public string Name {
			get => _Name;
			set => _Name = value;
		}

		[ManyToMany(typeof(EcoPointMaterial))]
		public List<EcoPoint> Ecopoints { get; set; }

		public string Description { get => _Description; set => _Description = value; }

		public Material(string Name, string Description) {
			_Name = Name ?? throw new ArgumentNullException(nameof(Name));
			this.Description = Description ?? throw new ArgumentNullException(nameof(Description));
		}

		public Material() {
		}
	}
}
