namespace SapDataloader.Dataloading
{
	using SapDataloader.Enums;
	using SapDataloader.Recording;
	using SapDataloader.SapDownloading;

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Класс загрузчика данных.
	/// </summary>
	public class Dataloader
	{
		/// <summary>
		/// Событие статуса загрузки.
		/// </summary>
		public Action<bool?> Downloading;

		/// <summary>
		/// Событие статуса сохранения.
		/// </summary>
		public Action<bool?> Saving;

		/// <summary>
		/// Событие окончания загрузки.
		/// </summary>
		public Action DataReceived;

		/// <summary>
		/// Событие, отправляющее количество задач.
		/// </summary>
		public Action<int> TaskCount;

		/// <summary>
		/// Событие, отправляющее количество завершенных задач.
		/// </summary>
		public Action<int> TaskCompleted;

		/// <summary>
		/// Настройки загрузчика.
		/// </summary>
		public Settings Settings;

		/// <summary>
		/// Результаты последней загрузки.
		/// </summary>
		public Results Results;

		/// <summary>
		/// Флаг автозагрузки.
		/// </summary>
		public bool AutoDL;

		/// <summary>
		/// Флаг занятости загрузчика.
		/// </summary>
		public bool IsBusy;

		/// <summary>
		/// Линия загрузчика.
		/// </summary>
		public readonly Line Line;

		/// <summary>
		/// Тип загрузчика.
		/// </summary>
		public readonly DlType Type;

		/// <summary>
		/// Конструктор класса <see cref="Dataloader"/>.
		/// </summary>
		/// <param name="type">Тип загрузчика.</param>
		public Dataloader(DlType type)
		{
			Type = type;
			Line = $"{type}".StartsWith("R") ? Line.RF : Line.WM;

			AutoDL = true;
			Settings = new Settings(type)
			{
				Frequency = Frequency.D,
				Period = 2,
				After = 20,
				Before = 0
			};
			Results = new Results();
		}

		/// <summary>
		/// Загружает данные.
		/// </summary>
		public void LoadData()
		{
			var today = DateTime.Today;
			var first = today.AddDays(-Settings.Before);
			var last = today.AddDays(Settings.After);
			LoadData(first, last);
		}

		/// <summary>
		/// Загружает данные.
		/// </summary>
		/// <param name="first">Дата начала периода.</param>
		/// <param name="last">Дата конца периода.</param>
		public void LoadData(DateTime first, DateTime last) => LoadData(Sql.GetProducts(first, last, Line));

		/// <summary>
		///  Загружает данные.
		/// </summary>
		/// <param name="products">Список продуктов.</param>
		public void LoadData(List<string> products)
		{
			Results.Products = products;
			Start();
		}

		/// <summary>
		/// Загружает все данные.
		/// </summary>
		public void LoadDataAll()
		{
			Results.Products = Sql.GetAllProducts(Line);
			Start();
		}

		/// <summary>
		/// Запускает загрузку.
		/// </summary>
		private void Start()
		{
			IsBusy = true;
			var sw = new Stopwatch();
			sw.Start();

			var manager = GetNewManager();
			Results.SapData = manager.GetSapDataList(Results.Products, Settings.MRPs, Settings.Language).Where(x => x != null && !string.IsNullOrEmpty(x.MATNR)).ToList();
			Results.Duration = sw.Elapsed;
			Results.Date = DateTime.Now;
			sw.Stop();
			DataReceived?.Invoke();
			if (Settings.NeedToSave) Task.Run(() => SaveToDB());

			Settings.NeedToSave = true;
			IsBusy = false;
		}

		/// <summary>
		/// Предоставляет экземпляр менеджера загрузки.
		/// </summary>
		/// <returns></returns>
		private TaskManager GetNewManager() => new TaskManager()
		{
			TaskCount = x => TaskCount?.Invoke(x),
			TaskCompleted = x => TaskCompleted?.Invoke(x),
			Downloading = x => Downloading?.Invoke(x)
		};

		/// <summary>
		/// Сохраняет данные в БД.
		/// </summary>
		private void SaveToDB()
		{
			Saving?.Invoke(true);
			try
			{
				Sql.Save(Type, Results, Line);
				Saving?.Invoke(null);
			}
			catch
			{
				Saving?.Invoke(false);
			}
		}
	}
}