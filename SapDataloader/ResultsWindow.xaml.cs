namespace SapDataloader
{
	using SapDataloader.Dataloading;
	using SapDataloader.Enums;
	using SapDataloader.Helpers;
	using SapDataloader.Recording;
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Media.Imaging;

	/// <summary>
	/// Логика взаимодействия для ResultsWindow.xaml
	/// </summary>
	public partial class ResultsWindow : Window
	{
		/// <summary>
		/// Тип загрузчика.
		/// </summary>
		private readonly DlType _type;

		/// <summary>
		/// Конструктор класса <see cref="ResultsWindow"/>.
		/// </summary>
		/// <param name="type">Тип загрузчика.</param>
		public ResultsWindow(DlType type)
		{
			InitializeComponent();
			Icon = BitmapFrame.Create(new Uri("img/results.png", UriKind.RelativeOrAbsolute));
			Title = $"{type}_Results";
			_type = type;
		}

		/// <summary>
		/// Инициализация окна.
		/// </summary>
		private void Window_Loaded()
		{
			var results = Hub.Dataloaders[_type].Results;
			if (results.Products.Count != 0)
			{
				_ = Task.Run(() => GetErrorProducts(results));
				ProductsRequested.SetContent($"{results.Products.Count}");
				DownloadDuration.SetContent($"{results.Duration}");
				DownloadDate.SetContent($"{results.Date:HH:mm:ss dd.MM.yyyy}");
			}

			if (results.SapData.Count != 0)
			{
				var MATNRs = results.SapData.Where(x => x != null && !string.IsNullOrEmpty(x.MATNR)).Select(a => a.MATNR).Distinct();

				ProductsReceived.SetContent($"{MATNRs.Count()}");

				DownloadSuccess.SetContent($"{MATNRs.Count() / (double)results.Products.Count:P2}");
				RecievedDataCount.SetContent($"{results.SapData.Count()}");


				_ = Task.Run(() => Info.SetDataContext(results.GetSpecifications()));
			}
		}

		/// <summary>
		/// Получение незагруженных продуктов.
		/// </summary>
		/// <param name="result">Результаты загрузки.</param>
		private void GetErrorProducts(Results results)
		{
			var MATNRs = results.SapData.Select(x => x.MATNR).Distinct();
			var errors = results.Products.Where(x => !MATNRs.Contains(x)).ToList();
			ErrorProducts.SetText(string.Join("; ", errors));
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) => Task.Run(() => Window_Loaded());
	}
}