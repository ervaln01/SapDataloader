namespace SapDataloader
{
	using SapDataloader.Dataloading;
	using SapDataloader.Enums;
	using SapDataloader.Helpers;
	using SapDataloader.Recording;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Media.Imaging;

	/// <summary>
	/// Логика взаимодействия для SettingsWindow.xaml
	/// </summary>
	public partial class SettingsWindow : Window
	{
		/// <summary>
		/// Тип загрузчика.
		/// </summary>
		private readonly DlType _type;

		/// <summary>
		/// Конструктор класса <see cref="SettingsWindow"/>.
		/// </summary>
		/// <param name="type">Тип загрузчика.</param>
		public SettingsWindow(DlType type)
		{
			InitializeComponent();
			Title = $"{type}_Settings";
			_type = type;
			Period.ItemsSource = (Enum.GetValues(typeof(Frequency)) as IEnumerable<Frequency>).OrderBy(x => x.ToString());
			Icon = BitmapFrame.Create(new Uri("img/settings.png", UriKind.RelativeOrAbsolute));
		}

		/// <summary>
		/// Инициализация окна.
		/// </summary>
		private void Window_Loaded()
		{
			var settings = Hub.Dataloaders[_type].Settings;
			Period.SelectedItem = settings.Frequency;
			PeriodCount.SetText($"{settings.Period}");
			PeriodStart.SetText($"{settings.Before}");
			PeriodFinish.SetText($"{settings.After}");

			_ = Task.Run(() => SetProducts());
		}

		/// <summary>
		/// Фильтр ввода цифр в TextBox.
		/// </summary>
		/// <param name="sender">TextBox, вызвавший событие.</param>
		/// <param name="e">Аргументы события.</param>
		private void TB_KeyUp(object sender, KeyEventArgs e)
		{
			var textBox = sender as TextBox;
			var text = string.Join("", textBox.Text.Where(x => x >= '0' && x <= '9'));
			textBox.Text = text;
			if (e.Key == Key.Right || e.Key == Key.Left) return;
			textBox.CaretIndex = text.Length;
		}

		/// <summary>
		/// Применение новых настроек.
		/// </summary>
		private void Button_Click()
		{
			_ = int.TryParse(PeriodCount.GetText(), out var period);
			_ = int.TryParse(PeriodStart.GetText(), out var before);
			_ = int.TryParse(PeriodFinish.GetText(), out var after);
			var frequency = (Frequency)Period.GetSelectedItem();
			if (period == 0) period++;
			if (frequency == Frequency.H && period > 23) period = 23;
			if (frequency == Frequency.D && period > 30) period = 30;

			Hub.Dataloaders[_type].Settings.Frequency = frequency;
			Hub.Dataloaders[_type].Settings.Period = period;
			Hub.Dataloaders[_type].Settings.Before = before;
			Hub.Dataloaders[_type].Settings.After = after;
			Hub.Dataloaders[_type].Settings.UpdateNextDl();

			Window_Loaded();
		}

		/// <summary>
		/// Заполнение списка продуктов.
		/// </summary>
		private void SetProducts()
		{
			var today = DateTime.Today;
			var settings = Hub.Dataloaders[_type].Settings;
			var products = Sql.GetProducts(today.AddDays(-settings.Before), today.AddDays(settings.After), Hub.Dataloaders[_type].Line);
			CurrentProducts.SetText(string.Join("; ", products));
			ProductsCount.SetContent($"{products.Count}");
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) => Window_Loaded();
		private void Button_Click(object sender, RoutedEventArgs e) => Button_Click();
	}
}