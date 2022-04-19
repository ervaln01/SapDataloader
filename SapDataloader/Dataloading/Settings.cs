namespace SapDataloader.Dataloading
{
	using SapDataloader.Enums;

	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;

	/// <summary>
	/// Класс настроек загрузчика.
	/// </summary>
	public class Settings
	{
		/// <summary>
		/// Период автозагрузки.
		/// </summary>
		public int Period;

		/// <summary>
		/// Множитель периода автозагрузки.
		/// </summary>
		public Frequency Frequency;

		/// <summary>
		/// Количество дней до текущего дня.
		/// </summary>
		public int Before;

		/// <summary>
		/// Количество дней после текущего дня.
		/// </summary>
		public int After;

		/// <summary>
		/// Дата следующей автозагрузки.
		/// </summary>
		public DateTime NextDL;

		/// <summary>
		/// Список MRP загрузчика.
		/// </summary>
		public List<string> MRPs;

		/// <summary>
		/// Язык загрузчика.
		/// </summary>
		public string Language;

		/// <summary>
		/// Необходимость сохранения данных после загрузки.
		/// </summary>
		public bool NeedToSave;

		/// <summary>
		/// Конструктор класса <see cref="Settings"/>.
		/// </summary>
		/// <param name="type">Тип загрузчика.</param>
		public Settings(DlType type)
		{
			MRPs = ConfigurationManager.AppSettings[$"MRP_{type}"].Split(',').ToList();
			Language = ConfigurationManager.AppSettings[$"SAP_LANGUAGE_{type}"];
		}
	}
}