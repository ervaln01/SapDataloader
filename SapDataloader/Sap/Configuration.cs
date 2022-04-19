namespace SapDataloader.Sap
{
	using System;
	using System.Configuration;

	/// <summary>
	/// Конфигурация подключения к SAP.
	/// </summary>
	public static class Configuration
	{
		/// <summary>
		/// IP-адрес SAP.
		/// </summary>
		public static string Ip { get; private set; }

		/// <summary>
		/// Системный номер.
		/// </summary>
		public static int SystemNumber { get; private set; }

		/// <summary>
		/// ПОльзователь.
		/// </summary>
		public static string User { get; private set; }

		/// <summary>
		/// Пароль.
		/// </summary>
		public static string Password { get; private set; }

		/// <summary>
		/// Клиент.
		/// </summary>
		public static string Client { get; private set; }

		/// <summary>
		/// Параметр функции WERKS.
		/// </summary>
		public static string WERKS { get; private set; }

		/// <summary>
		/// Параметр функции DISMM.
		/// </summary>
		public static string DISMM_PD { get; private set; }

		/// <summary>
		/// Параметр функции CAPID.
		/// </summary>
		public static string CAPID { get; private set; }

		/// <summary>
		/// Параметр функции ATINN.
		/// </summary>
		public static string ATINN { get; private set; }

		/// <summary>
		/// Параметр функции MTART.
		/// </summary>
		public static string MTART { get; private set; }

		/// <summary>
		/// Параметр функции ATNAM.
		/// </summary>
		public static string ATNAM { get; private set; }

		/// <summary>
		/// Параметр функции Sign.
		/// </summary>
		public static string Sign { get; private set; }

		/// <summary>
		/// Параметр функции Option0.
		/// </summary>
		public static string Option0 { get; private set; }

		/// <summary>
		/// Статический конструктор класса <see cref="Configuration"/>.
		/// </summary>
		static Configuration()
		{
			Ip = ConfigurationManager.AppSettings["SAP_IP"];
			SystemNumber = Convert.ToInt32(ConfigurationManager.AppSettings["SAP_SYSTEMNUMBER"]);
			User = ConfigurationManager.AppSettings["user_rfc"];
			Password = ConfigurationManager.AppSettings["pass_rfc"];
			Client = ConfigurationManager.AppSettings["SAP_CLIENT"];

			WERKS = ConfigurationManager.AppSettings["WERKS"];
			DISMM_PD = ConfigurationManager.AppSettings["DISMM_PD"];
			CAPID = ConfigurationManager.AppSettings["CAPID"];
			ATINN = ConfigurationManager.AppSettings["ATINN"];
			MTART = ConfigurationManager.AppSettings["MTART"];
			Sign = ConfigurationManager.AppSettings["Sign"];
			Option0 = ConfigurationManager.AppSettings["Option0"];
		}
	}
}