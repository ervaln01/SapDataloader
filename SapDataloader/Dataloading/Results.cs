namespace SapDataloader.Dataloading
{
	using SapDataloader.Sap;

	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Класс результатов загрузки.
	/// </summary>
	public class Results
	{
		/// <summary>
		/// Список загружаемых продуктов.
		/// </summary>
		public List<string> Products;

		/// <summary>
		/// Список выгруженных данных из SAP.
		/// </summary>
		public List<SapData> SapData;

		/// <summary>
		/// Дата последней загрузки.
		/// </summary>
		public DateTime Date;

		/// <summary>
		/// Длительность последней загрузки.
		/// </summary>
		public TimeSpan Duration;

		/// <summary>
		/// Конструктор класса <see cref="Results"/>.
		/// </summary>
		public Results()
		{
			Products = new List<string>();
			SapData = new List<SapData>();
			Date = default;
			Duration = default;
		}
	}
}