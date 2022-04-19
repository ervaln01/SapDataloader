namespace SapDataloader
{
	using SapDataloader.Recording;
	using SapDataloader.Sap;
	using System;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Media.Imaging;

	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		/// <summary>
		/// Событие, срабатывающее каждый час.
		/// </summary>
		public Action<DateTime> StruckHour;

		/// <summary>
		/// Конструктор класса <see cref="MainWindow"/>.
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();
			AsyncRecorder.Initialize(Dispatcher);
			Icon = BitmapFrame.Create(new Uri("img/icon.ico", UriKind.RelativeOrAbsolute));
		}

		/// <summary>
		/// Инициализация окна.
		/// </summary>
		private void Window_Loaded()
		{
			_ = Task.Run(() => TimerInit());
			_ = NavigationFrame.Navigate(new HubPage(this));
		}

		/// <summary>
		/// Инициализация таймера обновления времени.
		/// </summary>
		private void TimerInit()
		{
			var timer = new System.Timers.Timer { Interval = 1000 };
			timer.Elapsed += async (s, e) => await Task.Run(() =>
			{
				var now = DateTime.Now;
				lTimer.SetContent($"{now:dd.MM.yyyy HH:mm:ss}");
				if (now.Minute == 0 && now.Second == 0)
					StruckHour?.Invoke(now);

				if (now.Second % 30 == 0)
				{
					using (var connection = new Connection())
						Ping_Indicator.SetIndication(connection.PingSap());
				}
			});
			timer.Start();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) => Window_Loaded();
	}
}