using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ColetaDeReciclaveisClassLibrary.Models {
[Table("EcoPointMaterial")]
	public class EcoPointMaterial {
		[ForeignKey(typeof(EcoPoint))] public int EcoPointId { get; set; }
		[ForeignKey(typeof(Material))] public int MaterialId { get; set; }
	}
}
