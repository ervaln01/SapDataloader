namespace SapDataloader.Sap
{
	/// <summary>
	/// Модель загружаемых из SAP данных.
	/// </summary>
	public class SapData
	{
		/// <summary>
		/// PRODUCT.
		/// </summary>
		public string MATNR { get; private set; }

		/// <summary>
		/// PRODUCT_CODE.
		/// </summary>
		public string IDNRK { get; private set; }

		/// <summary>
		/// Некое число ~'0.000-2.000'.
		/// </summary>
		public decimal MNGLG { get; private set; }

		/// <summary>
		/// Обычно 'ST'.
		/// </summary>
		public string MEINS { get; private set; }

		/// <summary>
		/// MRP.
		/// </summary>
		public string DISPO { get; private set; }

		/// <summary>
		/// Обычно 'CRITICAL'.
		/// </summary>
		public string ATBEZ { get; private set; }

		/// <summary>
		/// Некий код ~'2243000054-03'.
		/// </summary>
		public string ALTGR { get; private set; }

		/// <summary>
		/// COMPONENT NAME.
		/// </summary>
		public string OJTXP { get; private set; }

		/// <summary>
		/// Обычно 'X'.
		/// </summary>
		public string SANKA { get; private set; }

		/// <summary>
		/// Конструктор класса <see cref="SapData"/>.
		/// </summary>
		/// <param name="list">Массив объектов, полученных из SAP.</param>
		public SapData(object[] list)
		{
			if (list.Length != 9) return;

			MATNR = ((string)list[0]).Trim();
			IDNRK = ((string)list[1]).Trim();
			MNGLG = (decimal)list[2];
			MEINS = ((string)list[3]).Trim();
			DISPO = ((string)list[4]).Trim();
			ATBEZ = ((string)list[5]).Trim();
			ALTGR = ((string)list[6]).Trim();
			OJTXP = ((string)list[7]).Trim();
			SANKA = ((string)list[8]).Trim();
		}
	}
}