using ColetaDeReciclaveisClassLibrary.Utils;
using SQLite;
using System;
using System.Collections.Generic;
using ColetaDeReciclaveisClassLibrary.Controllers;
using SQLiteNetExtensions.Attributes;

namespace ColetaDeReciclaveisClassLibrary.Models {

	[Table("EcoPoint")]
	public class EcoPoint : EcoPointController {

		private int _Id;
		private DateTime _CreatedAt;
		private DateTime _UpdatedAt;

		private string _Name;
		private string _Description;
		private Address _Address;

		private Phone _Phones;
		private List<Material> _MaterialsAccepted;

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

		[NotNull,MaxLength(100)]
		public string Name {
			get => _Name;
			set => _Name = value;
		}

		[NotNull]
		public string Description {
			get => _Description;
			set => _Description = value;
		}

		[OneToOne(CascadeOperations = CascadeOperation.CascadeDelete)]
		public Address Address {
			get => _Address;
			set => _Address = value;
		}

		[OneToOne(CascadeOperations = CascadeOperation.CascadeDelete)]
		public Phone Phones {
			get => _Phones;
			set => _Phones = value;
		}

		[ManyToMany(typeof(EcoPointMaterial))]
		public List<Material> MaterialsAccepted{
			get => _MaterialsAccepted;
			set => _MaterialsAccepted = value;
		}

		[ForeignKey(typeof(User))] public int UserId { get; set; }

		public EcoPoint(string Name, string Description, List<Material> MaterialsAccepted, Address address, Phone phone) {
			this.CreatedAt = DateTime.UtcNow;
			this.UpdatedAt = DateTime.UtcNow;
			this.Name = Name;
			this.Description = Description ;
			this.MaterialsAccepted = MaterialsAccepted;
			this.Address = address;
			this.Phones = phone;
		}

		public EcoPoint() {
		}
	}
}
