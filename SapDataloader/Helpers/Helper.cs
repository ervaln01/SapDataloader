namespace SapDataloader.Helpers
{
	using SapDataloader.Enums;
	using SapDataloader.Entities;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using SapDataloader.Dataloading;
	using SapDataloader.Recording;

	/// <summary>
	/// Класс расширений.
	/// </summary>
	public static class Helper
	{
		/// <summary>
		/// Сравнение дат.
		/// </summary>
		/// <param name="dt1">Первая дата.</param>
		/// <param name="dt2">Вторая дата.</param>
		/// <returns>ДАты равны?</returns>
		public static bool EqualDlDate(this DateTime dt1, DateTime dt2) => dt1.Month == dt2.Month && dt1.Day == dt2.Day && dt1.Hour == dt2.Hour;

		/// <summary>
		/// Обновляет время следующей автоматической загрузки.
		/// </summary>
		/// <param name="settings">Настройки загрузчика.</param>
		public static void UpdateNextDl(this Settings settings) => settings.UpdateNextDl(settings.Period, settings.Frequency);

		public static void UpdateNextDl(this Settings settings, int time, Frequency freq)
		{
			var now = DateTime.Now;

			switch (freq)
			{
				case Frequency.H:
					settings.NextDL = DateTime.Today.AddHours(now.Hour + time);
					return;
				case Frequency.D:
					settings.NextDL = DateTime.Today.AddDays(time);
					return;
				default:
					return;
			}
		}

		/// <summary>
		/// Получает список спецификаций из результатов выгрузки.
		/// </summary>
		/// <param name="results">Результаты загрузки.</param>
		/// <returns>Список спецификаций.</returns>
		public static List<Specification> GetSpecifications(this Results results)
		{
			var list = new List<Specification>();
			using (var context = new AnyContext())
			{
				var currentProducts = context.Products.Where(x => results.Products.Contains(x.Product1));
				foreach (var i in results.SapData)
				{
					var product = currentProducts.FirstOrDefault(x => x.Product1.Equals(i.MATNR));
					list.Add(new Specification()
					{
						Line = 1,
						Sysdate = DateTime.Now,
						Station = 10,
						Product = i.MATNR,
						Specifications = $"{i.IDNRK} {i.OJTXP}",
						Model = $"{product?.Model.Trim('\r', '\n')} {product?.Productname.Trim('\r', '\n')}"
					});
				}
				return list;
			}
		}

		/// <summary>
		/// Получает список SapItColl из результатов выгрузки.
		/// </summary>
		/// <param name="results">Результаты загрузки.</param>
		/// <param name="line">Линия.</param>
		/// <returns>Список SapItColl.</returns>
		public static List<SapItColl> GetSapItColls(this Results results, Line line) => results.SapData.Select(x => new SapItColl()
		{
			Matnr = x.MATNR,
			Idnrk = x.IDNRK,
			Mnglg = x.MNGLG,
			Meins = x.MEINS,
			Dispo = x.DISPO,
			Atbez = x.ATBEZ,
			Altgr = x.ALTGR,
			Ojtxp = RenameComporessor(x.OJTXP),
			Sanka = x.SANKA,
			Line = (int)line,
			Sysdate = DateTime.Now
		}).ToList();

		/// <summary>
		/// Переименовывает компрессор.
		/// </summary>
		/// <param name="name">Название компонента.</param>
		/// <returns>Правильно именованный компонент.</returns>
		private static string RenameComporessor(string name)
		{
			var compressor = "COMPRESSOR";
			if (name.Length < 11 || !name.StartsWith(compressor))
				return name;

			name = $"{compressor}_{name.Substring(11)}";

			var splitedName = name.Split('_');

			if (splitedName.Length == 2)
				return name;

			if (splitedName[1].Equals("JIAXIPERA TG 1112"))
				return $"{splitedName[0]}_JIAXIPERA_TG1112_{splitedName[2]}";

			if (splitedName[1].Equals("JIAXIPERA TG 1114Y"))
				return $"{splitedName[0]}_JIAXIPERA_TG1114Y_{splitedName[2]}";

			return name;
		}
	}
}