namespace SapDataloader
{
	using SapDataloader.Dataloading;
	using SapDataloader.Enums;
	using SapDataloader.Recording;
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media.Imaging;

	/// <summary>
	/// Логика взаимодействия для ManualWindow.xaml
	/// </summary>
	public partial class ManualWindow : Window
	{
		/// <summary>
		/// Тип загрузчика.
		/// </summary>
		private readonly DlType _type;

		/// <summary>
		/// Конструктор класса <see cref="ManualWindow"/>.
		/// </summary>
		/// <param name="type">Тип загрузчика.</param>
		public ManualWindow(DlType type)
		{
			InitializeComponent();
			Icon = BitmapFrame.Create(new Uri("img/manual.png", UriKind.RelativeOrAbsolute));
			Title = $"{type}_Manual";
			_type = type;
		}

		/// <summary>
		/// Инициализация окна.
		/// </summary>
		private void Window_Loaded()
		{
			PeriodStart.SelectedDate = DateTime.Today;
			PeriodFinish.SelectedDate = DateTime.Today;
		}

		/// <summary>
		/// Обрабатывает нажатие на переключатель.
		/// </summary>
		/// <param name="radioButton">Переключатель.</param>
		private void RadioButton_Click(RadioButton radioButton)
		{
			RbExtra.IsChecked = false;
			RbAll.IsChecked = false;
			RbPeriod.IsChecked = false;
			RbList.IsChecked = false;
			Period.Visibility = Visibility.Hidden;
			Products.Visibility = Visibility.Hidden;
			radioButton.IsChecked = true;

			switch (radioButton.Name)
			{
				case nameof(RbExtra):
					ModeInfo.Content = "Dl now, next auto dl will be transferred";
					break;
				case nameof(RbAll):
					ModeInfo.Content = "Dl for all products";
					break;
				case nameof(RbPeriod):
					ModeInfo.Content = "Dl for products for a specific period";
					Period.Visibility = Visibility.Visible;
					break;
				case nameof(RbList):
					ModeInfo.Content = "Dl for the entered product list";
					Products.Visibility = Visibility.Visible;
					break;
				default:
					ModeInfo.Content = string.Empty;
					break;
			}
		}

		/// <summary>
		/// Устанавливает пределы выбора даты.
		/// </summary>
		/// <param name="datePicker">Селектор дат.</param>
		private void DatePicker_SelectedDateChanged(DatePicker datePicker)
		{
			switch (datePicker.Name)
			{
				case nameof(PeriodStart):
					PeriodFinish.DisplayDateStart = PeriodStart.SelectedDate;
					if (PeriodFinish.SelectedDate < PeriodStart.SelectedDate)
						PeriodFinish.SelectedDate = PeriodStart.SelectedDate;
					break;

				case nameof(PeriodFinish):
					PeriodStart.DisplayDateEnd = PeriodFinish.SelectedDate;
					if (PeriodStart.SelectedDate > PeriodFinish.SelectedDate)
						PeriodStart.SelectedDate = PeriodFinish.SelectedDate;
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Устанавливает флаг необходимости сохранения результатов.
		/// </summary>
		/// <param name="checkBox">Флаг.</param>
		private void SaveResultsClick(CheckBox checkBox) => Hub.Dataloaders[_type].Settings.NeedToSave = checkBox.GetChecked();

		/// <summary>
		/// Обрабатывает нажатие на кнопку ручной загрузки.
		/// </summary>
		private void ManualDlClick()
		{
			if (RbList.IsChecked.Value)
			{
				var products = ProductList.Text;
				var separators = products.Where(x => x < '0' || x > '9').ToArray();
				var list = products.Split(separators, StringSplitOptions.RemoveEmptyEntries);
				_ = Task.Run(() => Hub.Dataloaders[_type].LoadData(list.Distinct().ToList()));
				Close();
			}

			if (RbPeriod.IsChecked.Value)
			{
				var first = PeriodStart.SelectedDate;
				var last = PeriodFinish.SelectedDate;
				_ = Task.Run(() => Hub.Dataloaders[_type].LoadData(first.Value, last.Value));
				Close();
			}

			if (RbAll.IsChecked.Value)
			{
				_ = Task.Run(() => Hub.Dataloaders[_type].LoadDataAll());
				Close();
			}

			if (RbExtra.IsChecked.Value)
			{
				_ = Task.Run(() => Hub.Dataloaders[_type].LoadData());
				Close();
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) => Window_Loaded();
		private void RadioButton_Click(object sender, RoutedEventArgs e) => RadioButton_Click(sender as RadioButton);
		private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e) => DatePicker_SelectedDateChanged(sender as DatePicker);
		private void SaveResultsClick(object sender, RoutedEventArgs e) => SaveResultsClick(sender as CheckBox);
		private void ManualDlClick(object sender, RoutedEventArgs e) => ManualDlClick();
	}
}