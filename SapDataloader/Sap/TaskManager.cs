namespace SapDataloader.SapDownloading
{
	using SapDataloader.Sap;

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Класс управления параллельной загрузкой данных.
	/// </summary>
	public class TaskManager
	{
		/// <summary>
		/// Событие, посылающее статус загрузки данных.
		/// </summary>
		public Action<bool?> Downloading;

		/// <summary>
		/// Событие, посылающее количество задач.
		/// </summary>
		public Action<int> TaskCount;

		/// <summary>
		/// Событие, посылающее количество завершенных задач.
		/// </summary>
		public Action<int> TaskCompleted;

		/// <summary>
		/// Количество завершенных задач.
		/// </summary>
		private int completedTasks = 0;

		/// <summary>
		/// Конструктор класса <see cref="TaskManager"/>.
		/// </summary>
		public TaskManager()
		{
			Downloading?.Invoke(null);
			TaskCompleted?.Invoke(0);
			TaskCount?.Invoke(0);
		}

		/// <summary>
		/// Получает список данных из SAP.
		/// </summary>
		/// <param name="products">Список продуктов.</param>
		/// <param name="mrps">Список MRP.</param>
		/// <param name="lang">Язык.</param>
		/// <returns>Список данных из SAP.</returns>
		public List<SapData> GetSapDataList(IEnumerable<string> products, List<string> mrps, string lang)
		{
			var sapDataList = new List<SapData>();
			Downloading?.Invoke(true);
			var tasks = GetGroupsForTasks(products).Select(productGroup => Task.Run(() =>
			{
				if (productGroup.Count() == 0)
				{
					TaskCompleted?.Invoke(++completedTasks);
					return;
				}

				var sapData = GetSapData(productGroup, mrps, lang);
				if (sapData != null)
					sapDataList.AddRange(sapData);

				TaskCompleted?.Invoke(++completedTasks);
			}));

			try
			{
				Task.WaitAll(tasks.ToArray());
				Downloading?.Invoke(null);
				return sapDataList;
			}
			catch
			{
				Downloading?.Invoke(false);
				return null;
			}
		}

		/// <summary>
		/// Получает список сгруппированных продуктов.
		/// </summary>
		/// <param name="products">Список продуктов.</param>
		/// <param name="countInTask">Количество продуктов в одной зааче.</param>
		/// <returns>Список сгруппированных продуктов.</returns>
		private IEnumerable<IEnumerable<string>> GetGroupsForTasks(IEnumerable<string> products, int countInTask = 25)
		{
			var taskCount = products.Count() / countInTask + 1;
			TaskCount?.Invoke(taskCount);
			for (var i = 0; i < taskCount; i++)
			{
				yield return products.Skip(countInTask * i).Take(countInTask);
			}
		}

		/// <summary>
		/// Получает список данных из SAP для группы продуктов.
		/// </summary>
		/// <param name="productGroup">Список продуктов.</param>
		/// <param name="mrps">Список MRP.</param>
		/// <param name="lang">Язык.</param>
		/// <returns>Список данных из SAP.</returns>
		private List<SapData> GetSapData(IEnumerable<string> productGroup, List<string> mrps, string lang)
		{
			try
			{
				using (var connection = new Connection(lang))
				{
					var opennedConnection = connection.GetOpenConnection();
					if (opennedConnection == null) return null;

					var receiving = new TaskProcessor(opennedConnection, mrps);
					return receiving.GetInformation(productGroup);
				}
			}
			catch
			{
				return null;
			}
		}
	}
}