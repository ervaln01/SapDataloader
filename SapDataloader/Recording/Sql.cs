namespace SapDataloader.Recording
{
	using SapDataloader.Dataloading;
	using SapDataloader.Enums;
	using SapDataloader.Entities;
	using SapDataloader.Helpers;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Configuration;

	/// <summary>
	/// Класс взаимодействия с базой данных.
	/// </summary>
	public static class Sql
	{
		private static readonly string connection;
		static Sql()
		{
			var test = ConfigurationManager.AppSettings["TestDB"];
			bool.TryParse(test, out var result);
			connection = result ? "TestConnection" : "DBConnection";
		}

		/// <summary>
		/// Получает список продуктов.
		/// </summary>
		/// <param name="first">Дата начала периода.</param>
		/// <param name="last">Дата конца периода.</param>
		/// <param name="line">Линия.</param>
		/// <returns>Список продуктов.</returns>
		public static List<string> GetProducts(DateTime first, DateTime last, Line line) => GetProducts(x => x.Date >= first && x.Date <= last && x.Line == (int)line);

		/// <summary>
		/// Получает список всех продуктов.
		/// </summary>
		/// <param name="line">Линия.</param>
		/// <returns>Список всех продуктов.</returns>
		public static List<string> GetAllProducts(Line line) => GetProducts(x => x.Line == (int)line);

		/// <summary>
		/// Сохраняет результаты.
		/// </summary>
		/// <param name="type">Тип загрузчика.</param>
		/// <param name="results">Результаты загрузки.</param>
		/// <param name="line">Линия.</param>
		public static void Save(DlType type, Results results, Line line)
		{
			switch (type)
			{
				case DlType.RP:
					results.GetSpecifications().SaveToDB();
					return;
				case DlType.RC:
				case DlType.WC:
					results.GetSapItColls(line).SaveToDB(line).UpdateDB();
					return;
				default:
					return;
			}
		}

		/// <summary>
		/// Получает список продуктов из базы данных.
		/// </summary>
		/// <param name="lambda">Лямбда-выражение.</param>
		/// <returns>Список продуктов.</returns>
		private static List<string> GetProducts(Func<ProductionShedule, bool> lambda)
		{
			using (var context = new AnyContext())
			{
				return context.ProductionShedules.Where(lambda)
					.Select(x => x.Product)
					.Where(x => x.Length == 10 && x.Count(c => c >= '0' && c <= '9') == x.Length)
					.Distinct()
					.ToList();
			}
		}

		/// <summary>
		/// Сохраняет список спецификаций в базу данных.
		/// </summary>
		/// <param name="specifications">Список спецификаций.</param>
		private static void SaveToDB(this List<Specification> specifications)
		{
			using (var context = new AnyContext(connection))
			{
				foreach (var product in specifications.Select(x => x.Product).Distinct())
				{
					try
					{
						var sql = $"DELETE FROM [BekoLLCSQL].[dbo].[SPECIFICATIONS] WHERE [PRODUCT] = '{product}' " +
							$"INSERT INTO [BekoLLCSQL].[dbo].[SPECIFICATIONS] ([PRODUCT],[MODEL],[SPECIFICATIONS],[LINE],[STATION],[SYSDATE]) VALUES " +
							$"{string.Join(",", specifications.Where(x => x.Product.Equals(product)).Select(s => $"('{s.Product}', N'{s.Model}', N'{s.Specifications}', {s.Line}, {s.Station}, '{s.Sysdate:yyyy-MM-dd HH:mm:ss.fff}')"))}";
						_ = context.Database.ExecuteSqlCommand(sql);
					}
					catch
					{
						continue;
					}
				}
			}
		}

		/// <summary>
		/// Сохраняет список загруженных из SAP данных во временную таблицу SAP_IT_COLL.
		/// </summary>
		/// <param name="sapItColls">Список данных для записи в таблицу SAP_IT_COLL.</param>
		/// <param name="line">Линия.</param>
		/// <returns>Список продуктов.</returns>
		private static IEnumerable<string> SaveToDB(this List<SapItColl> sapItColls, Line line)
		{
			using (var context = new AnyContext(connection))
			{
				context.Database.ExecuteSqlCommand($"DELETE FROM [BekoLLCSQL].[dbo].[SAP_IT_COLL] WHERE [LINE] = {(int)line}");
				var products = sapItColls.Select(x => x.Matnr).Distinct();
				foreach (var product in products)
				{
					try
					{
						var sql = $"INSERT INTO [BekoLLCSQL].[dbo].[SAP_IT_COLL] ([MATNR],[IDNRK],[MNGLG],[MEINS],[DISPO],[ATBEZ],[ALTGR],[OJTXP],[SANKA],[LINE],[SYSDATE]) VALUES " +
							$"{string.Join(",", sapItColls.Where(x => x.Matnr.Equals(product)).Select(s => $"('{s.Matnr}','{s.Idnrk}',{s.Mnglg},'{s.Meins}','{s.Dispo}','{s.Atbez}','{s.Altgr}','{s.Ojtxp}','{s.Sanka}',{s.Line},'{s.Sysdate:yyyy-MM-dd HH:mm:ss.fff}')"))}";
						_ = context.Database.ExecuteSqlCommand(sql);
					}
					catch { }
					yield return product;
				}
			}
		}

		/// <summary>
		/// Обновляет таблицу T_KKTS_SAP_COMPONENTS данными из таблицы SAP_IT_COLL.
		/// </summary>
		/// <param name="products">Список продуктов.</param>
		private static void UpdateDB(this IEnumerable<string> products)
		{
			using (var context = new AnyContext(connection))
			{
				foreach (var product in products)
				{
					try
					{
						var sql = $"DELETE FROM [BekoLLCSQL].[dbo].[T_KKTS_SAP_COMPONENTS] where PRODUCT = '{product}'" +
							$"INSERT INTO [BekoLLCSQL].[dbo].[T_KKTS_SAP_COMPONENTS] ([PRODUCT],[COMPONENT_CODE],[MNGLG],[MEINS],[DISPO],[ATBEZ],[ALTGR],[COMPONENT_NAME],[SANKA],[LINE],[UPDATETIME],[SYSDATE])" +
							$"SELECT [MATNR],[IDNRK],[MNGLG],[MEINS],[DISPO],[ATBEZ],[ALTGR],[OJTXP],[SANKA],[LINE],GETDATE(),[SYSDATE] FROM [BekoLLCSQL].[dbo].[SAP_IT_COLL] WHERE [MATNR] = '{product}'";
						_ = context.Database.ExecuteSqlCommand(sql);
					}
					catch
					{
						continue;
					}
				}
			}
		}
	}
}