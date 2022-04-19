namespace SapDataloader.Sap
{
	using ERPConnect;

	using System;

	/// <summary>
	/// Класс, открывающий соединение с SAP.
	/// </summary>
	public class Connection : IDisposable
	{
		/// <summary>
		/// Экземпляр подключения к SAP.
		/// </summary>
		private readonly R3Connection connection;

		/// <summary>
		/// Конструктор класса <see cref="Connection"/>.
		/// </summary>
		public Connection() : this("EN") { }

		/// <summary>
		/// Конструктор класса <see cref="Connection"/>.
		/// </summary>
		/// <param name="lang">Язык обращения к SAP.</param>
		public Connection(string lang)
		{
			LIC.SetLic("F360Z67DZF");
			connection = new R3Connection(Configuration.Ip, Configuration.SystemNumber, Configuration.User, Configuration.Password, lang, Configuration.Client);
		}

		/// <summary>
		/// Получает открытое соединение.
		/// </summary>
		/// <returns>Экземпляр открытого соединения.</returns>
		public R3Connection GetOpenConnection()
		{
			try
			{
				connection.Open(false);
				return connection;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Проверяет доступность соединения с SAP.
		/// </summary>
		/// <returns>Возможно ли подключиться к SAP?</returns>
		public bool PingSap()
		{
			var openConnection = GetOpenConnection();
			if (openConnection == null) return false;

			try
			{
				return openConnection.Ping();
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Закрывает и очищает соединение.
		/// </summary>
		public void Dispose()
		{
			connection.Close();
			connection.Dispose();
		}
	}
}