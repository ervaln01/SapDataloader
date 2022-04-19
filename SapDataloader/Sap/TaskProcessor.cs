namespace SapDataloader
{
	using ERPConnect;

	using SapDataloader.Sap;

	using System;
	using System.Collections.Generic;
	using System.Data;

	/// <summary>
	/// Класс, обрабатывающий задачу загрузки из SAP.
	/// </summary>
	public class TaskProcessor
	{
		/// <summary>
		/// Экземпляр соединения с SAP.
		/// </summary>
		private readonly R3Connection _connection;

		/// <summary>
		/// Список MRP.
		/// </summary>
		private readonly List<string> _mrps;

		/// <summary>
		/// Конструктор класса <see cref="TaskProcessor"/>.
		/// </summary>
		/// <param name="connection">Экземпляр соединения с SAP.</param>
		/// <param name="mrps">Список MRP.</param>
		public TaskProcessor(R3Connection connection, List<string> mrps)
		{
			_connection = connection;
			_mrps = mrps;
		}

		/// <summary>
		/// Выкачивает данные из SAP.
		/// </summary>
		/// <param name="products">Список продуктов.</param>
		/// <returns>Список загруженных из SAP данных.</returns>
		public List<SapData> GetInformation(IEnumerable<string> products)
		{
			try
			{
				var func = GetFunction(products);
				var data = DownloadData(func);
				return data == null || data.Count == 0 ? null : data;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Получает заполненную функцию выкачки данных из SAP.
		/// </summary>
		/// <param name="products">Список продуктов.</param>
		/// <returns>Функция выкачки данных из SAP.</returns>
		private RFCFunction GetFunction(IEnumerable<string> products)
		{
			var func = GetSharedFunction();
			foreach (var product in products)
			{
				var dsMATNR = new RFCStructure(func.Tables["MATNR"].Columns);
				dsMATNR["SIGN"] = Configuration.Sign;
				dsMATNR["OPTION"] = Configuration.Option0;
				dsMATNR["MATNR_LOW"] = product.PadLeft(18, '0');
				dsMATNR["MATNR_HIGH"] = string.Empty;
				func.Tables["MATNR"].Rows.Add(dsMATNR);
			}
			return func;
		}

		/// <summary>
		/// Запускает выполнение функции выкачки данных из SAP.
		/// </summary>
		/// <param name="func">Функция выкачки данных из SAP.</param>
		/// <returns>Список загруженных из SAP данных.</returns>
		private List<SapData> DownloadData(RFCFunction func)
		{
			try
			{
				func.Execute();
			}
			catch
			{
				return null;
			}

			var table = func.Tables["IT_COLL"].ToADOTable();
			var info = new List<SapData>();
			foreach (DataRow row in table.Rows)
			{
				info.Add(new SapData(row.ItemArray));
			}
			return info;
		}

		/// <summary>
		/// Получает общую часть функции для всех продуктов.
		/// </summary>
		/// <returns>Функция выкачки данных из SAP.</returns>
		private RFCFunction GetSharedFunction()
		{
			var func = _connection.CreateFunction("Z_RFC_BOM_FILTER2");
			func.Exports["WERKS"].ParamValue = Configuration.WERKS;
			func.Exports["DISMM"].ParamValue = Configuration.DISMM_PD;
			func.Exports["CAPID"].ParamValue = Configuration.CAPID;
			func.Exports["ATINN"].ParamValue = Configuration.ATINN;
			func.Exports["MTART"].ParamValue = Configuration.MTART;
			func.Exports["DATUV"].ParamValue = DateTime.Now.ToString("yyyy:MM:dd").Replace(":", "");

			_mrps.ForEach(mrp =>
			{
				var dsMIP = new RFCStructure(func.Tables["MIP"].Columns);
				dsMIP["SIGN"] = Configuration.Sign;
				dsMIP["OPTION"] = Configuration.Option0;
				dsMIP["LOW"] = mrp;
				dsMIP["HIGH"] = string.Empty;
				func.Tables["MIP"].Rows.Add(dsMIP);
			});

			return func;
		}
	}
}