namespace SapDataloader
{
	using SapDataloader.Dataloading;
	using SapDataloader.Enums;
	using SapDataloader.Helpers;
	using SapDataloader.Recording;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;

	/// <summary>
	/// Логика взаимодействия для HubPage.xaml
	/// </summary>
	public partial class HubPage : Page
	{
		/// <summary>
		/// Конструктор класса <see cref="HubPage"/>.
		/// </summary>
		/// <param name="main">Основное окно.</param>
		public HubPage(MainWindow main)
		{
			InitializeComponent();
			InitializeForms();
			main.StruckHour = dt =>
			{
				var dls = Hub.Dataloaders.Select(x => x.Value).Where(x => x.Settings.NextDL.EqualDlDate(dt) && x.AutoDL);
				foreach (var dl in dls)
				{
					_ = Task.Run(() =>
					{
						if (dl.IsBusy)
						{
							dl.Settings.UpdateNextDl(1, Frequency.H);
							Hub.VisualForms[dl.Type].NextDL.SetContent($"{dl.Settings.NextDL:HH:mm:ss dd.MM.yyyy}");
							return;
						}

						dl.Settings.NeedToSave = true;
						dl.Settings.UpdateNextDl();
						dl.LoadData();
					});
				}
			};
		}

		/// <summary>
		/// Заполняет словарь элементами.
		/// </summary>
		public void InitializeForms()
		{
			Hub.VisualForms.Add(DlType.RP, new VisualForm(RP_AutoDL, RP_NextDL, RP_Success, RP_Duration, RP_Date, RP_DLIndicator, RP_SaveIndicator, RP_TaskCount, RP_TaskCompleted, RP_Settings, RP_Results, RP_Manual));
			Hub.VisualForms.Add(DlType.RC, new VisualForm(RC_AutoDL, RC_NextDL, RC_Success, RC_Duration, RC_Date, RC_DLIndicator, RC_SaveIndicator, RC_TaskCount, RC_TaskCompleted, RC_Settings, RC_Results, RC_Manual));
			Hub.VisualForms.Add(DlType.WC, new VisualForm(WC_AutoDL, WC_NextDL, WC_Success, WC_Duration, WC_Date, WC_DLIndicator, WC_SaveIndicator, WC_TaskCount, WC_TaskCompleted, WC_Settings, WC_Results, WC_Manual));
		}

		/// <summary>
		/// Инициализация страницы.
		/// </summary>
		/// <param name="type">Тип загрузчика.</param>
		private void PageLoad(DlType type)
		{
			Hub.Dataloaders[type].Settings.UpdateNextDl();
			Hub.Dataloaders[type].Downloading = x =>
			{
				Hub.VisualForms[type].IndicatorDL.SetIndication(x);
				Hub.VisualForms[type].Settings.SetEnable(x != true);
				Hub.VisualForms[type].Results.SetEnable(x != true);
				Hub.VisualForms[type].Manual.SetEnable(x != true);
				if (x != null)
				{
					Hub.VisualForms[type].TaskCount.SetContent("0");
					Hub.VisualForms[type].TaskCompleted.SetContent("0");
				}
			};

			Hub.Dataloaders[type].DataReceived = () =>
			{
				var results = Hub.Dataloaders[type].Results;
				var sapList = results.SapData.Where(x => x != null && !string.IsNullOrEmpty(x.MATNR)).ToList();
				if (sapList.Count == 0)	Hub.VisualForms[type].Success.SetContent($"{0:P2}");
				else Hub.VisualForms[type].Success.SetContent($"{sapList.Select(a => a.MATNR).Distinct().Count() / (double)results.Products.Count:P2}");
				Hub.VisualForms[type].Duration.SetContent($"{results.Duration}");
				Hub.VisualForms[type].Date.SetContent($"{results.Date:HH:mm:ss dd.MM.yyyy}");
				Hub.VisualForms[type].NextDL.SetContent($"{Hub.Dataloaders[type].Settings.NextDL:HH:mm:ss dd.MM.yyyy}");
			};

			Hub.Dataloaders[type].TaskCount = x => Hub.VisualForms[type].TaskCount.SetContent($"{x}");
			Hub.Dataloaders[type].TaskCompleted = x => Hub.VisualForms[type].TaskCompleted.SetContent($"{x}");
			Hub.Dataloaders[type].Saving = x => Hub.VisualForms[type].IndicatorSave.SetIndication(x);

			Hub.VisualForms[type].IndicatorDL.SetIndication(null);
			Hub.VisualForms[type].IndicatorSave.SetIndication(null);
			Hub.VisualForms[type].NextDL.SetContent($"{Hub.Dataloaders[type].Settings.NextDL:HH:mm:ss dd.MM.yyyy}");
		}

		/// <summary>
		/// Обрабатывает клик по кнопке настроек.
		/// </summary>
		/// <param name="type">Тип загрузчика.</param>
		private void SettingsClick(DlType type)
		{
			_ = new SettingsWindow(type).ShowDialog();
			Hub.VisualForms[type].NextDL.SetContent($"{Hub.Dataloaders[type].Settings.NextDL:HH:mm:ss dd.MM.yyyy}");
		}

		/// <summary>
		/// Обрабатывает клик по кнопке результатов.
		/// </summary>
		/// <param name="type">Тип загрузчика.</param>
		private void ResultsClick(DlType type) => new ResultsWindow(type).Show();

		/// <summary>
		/// Обрабатывает клик по кнопке ручной загрузки.
		/// </summary>
		/// <param name="type">Тип загрузчика.</param>
		private void ManualClick(DlType type)
		{
			Hub.Dataloaders[type].Settings.NeedToSave = false;
			_ = new ManualWindow(type).ShowDialog();
		}

		/// <summary>
		/// Обрабатывает клик по флагу автоматической загрузки.
		/// </summary>
		/// <param name="type">Тип загрузчика.</param>
		private void AutoDlClick(DlType type)
		{
			var auto = Hub.VisualForms[type].AutoDL.GetChecked();
			Hub.VisualForms[type].NextDL.SetVisibility(auto);

			if (!Hub.Dataloaders[type].IsBusy)
			{
				Hub.Dataloaders[type].AutoDL = auto;
				Hub.VisualForms[type].Settings.SetEnable(auto);
			}
		}

		private void Page_Loaded(object sender, RoutedEventArgs e) => Task.WaitAll(Hub.VisualForms.Select(x => Task.Run(() => PageLoad(x.Key))).ToArray());

		private void RPSettingsClick(object sender, RoutedEventArgs e) => SettingsClick(DlType.RP);
		private void RPResultsClick(object sender, RoutedEventArgs e) => ResultsClick(DlType.RP);
		private void RPManualClick(object sender, RoutedEventArgs e) => ManualClick(DlType.RP);
		private void RPAutoDlClick(object sender, RoutedEventArgs e) => AutoDlClick(DlType.RP);

		private void RCSettingsClick(object sender, RoutedEventArgs e) => SettingsClick(DlType.RC);
		private void RCResultsClick(object sender, RoutedEventArgs e) => ResultsClick(DlType.RC);
		private void RCManualClick(object sender, RoutedEventArgs e) => ManualClick(DlType.RC);
		private void RCAutoDlClick(object sender, RoutedEventArgs e) => AutoDlClick(DlType.RC);

		private void WCSettingsClick(object sender, RoutedEventArgs e) => SettingsClick(DlType.WC);
		private void WCResultsClick(object sender, RoutedEventArgs e) => ResultsClick(DlType.WC);
		private void WCManualClick(object sender, RoutedEventArgs e) => ManualClick(DlType.WC);
		private void WCAutoDlClick(object sender, RoutedEventArgs e) => AutoDlClick(DlType.WC);
	}
}