using System;
using System.IO;
using System.Linq;

namespace ColetaDeReciclaveisClassLibrary.Utils {
	public static class StaticsInfo {

        public enum CALLBACK_STATUS
        {
            NONE,
            STARTED,
            IN_PROGRESS,
            FINISHED,
            FINISHED_WITH_ERROR,
            ERROR,
            UNABLE_TO_ACCESS
        }

        public enum DaysOfWeek {
			DOMINGO, SEGUNDA, TERCA, QUARTA, QUINTA, SEXTA, SABADO
		}

		public struct OpenCloseAtStruct {
            #pragma warning disable CS0649
            public DaysOfWeek dayOfWeek;
            public int openTime;
			public int closeTime;
		}

		public static readonly string ApiURL = "";
		public static readonly string AddressURL = "address";
		public static readonly string EcoPointURL = "ecopoint";
		public static readonly string JobURL = "job";
		public static readonly string MaterialURL = "material";
		public static readonly string MessageURL = "message";
		public static readonly string PhoneURL = "phone";
		public static readonly string PhotoURL = "photo";
		public static readonly string UserURL = "user";
		public static readonly string LoginURL = "login";

		public static readonly string DEFAULT_PATH = Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public static readonly string DATABASE_PATH = Path.Combine(DEFAULT_PATH, "db.db");

		public static readonly string StartUpPath = Environment.CurrentDirectory;

		public static readonly string DATABASE_PATH_API = Path.Combine($"{StartUpPath}\\SqLiteDB", "db.db");

    }
}
