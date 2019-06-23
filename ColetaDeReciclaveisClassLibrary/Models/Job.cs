using SQLite;
using System;
using System.Collections.Generic;
using ColetaDeReciclaveisClassLibrary.Controllers;
using SQLiteNetExtensions.Attributes;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using System.IO;

namespace ColetaDeReciclaveisClassLibrary.Models {

	[Table("Jobs")]
	public class Job : JobController{
		private int _Id;
		private DateTime _CreatedAt;
		private DateTime _UpdatedAt;

		private string _Title;
		private string _Subtitle;
		private string _Description;
		private string _Price;

		private DateTime _ServiceDate;

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

		[NotNull,MaxLength(20)]
		public string Title {
			get => _Title;
			set => _Title = value;
		}

		[MaxLength(40)]
		public string Subtitle {
			get => _Subtitle;
			set => _Subtitle = value;
		}

		[NotNull,MaxLength(500)]
		public string Description {
			get => _Description;
			set => _Description = value;
		}

		public string Price {
			get => _Price;
			set => _Price = value;
		}

		[Ignore]
		public string Phone{ get; set; }

		//[NotNull]
		public DateTime ServiceDate {
			get => _ServiceDate;
			set => _ServiceDate = value;
		}

		public byte[] PhotoInBytes{ get; set; }

		[Ignore]
		public ImageSource image { get; set; }

		public void GetImage(){ image = ImageSource.FromStream(() => new MemoryStream(PhotoInBytes)); }

		[ForeignKey(typeof(User))] public int UserId { get; set; }

		public Job(int UserId, string Title, string Subtitle, string Description, string Price, DateTime ServiceDate,byte[] photo) {
			this.CreatedAt = DateTime.UtcNow;
			this.UpdatedAt = DateTime.UtcNow;
			this.Title = Title ?? throw new ArgumentNullException(nameof(Title));
			this.Subtitle = Subtitle ?? throw new ArgumentNullException(nameof(Subtitle));
			this.Description = Description ?? throw new ArgumentNullException(nameof(Description));
			this.Price = Price ?? throw new ArgumentNullException(nameof(Price));
			this.ServiceDate = ServiceDate;
			this.PhotoInBytes = photo;
		}

		public Job() {
		}
	}
}
