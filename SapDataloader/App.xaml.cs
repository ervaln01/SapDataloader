namespace SapDataloader
{
	using System.Reflection;
	using System.Threading;
	using System.Windows;

	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	public partial class App : Application
	{
		/// <summary>
		/// Мьютекс.
		/// </summary>
		private Mutex mutex;

		/// <summary>
		/// Флаг, показывающий является ли текущий процесс владельцем мьютекса.
		/// </summary>
		private bool currentProcessOwner;

		/// <summary>
		/// Выполняет контроль повтоного запуска и открывает основное окно.
		/// </summary>
		private void Application_Startup()
		{
			mutex = new Mutex(true, Assembly.GetExecutingAssembly().GetName().Name, out currentProcessOwner);
			if (!currentProcessOwner)
			{
				_ = MessageBox.Show("Приложение уже запущено");
				Current.Shutdown();
				return;
			}

			new MainWindow().Show();
		}

		/// <summary>
		/// Вызывает событие Application.Exit и освобождает мьютекс.
		/// </summary>
		/// <param name="e">Аргументы события.</param>
		protected override void OnExit(ExitEventArgs e)
		{
			if (currentProcessOwner) mutex.ReleaseMutex();

			try
			{
				base.OnExit(e);
			}
			catch
			{
				Current.Shutdown();
			}
		}

		private void Application_Startup(object sender, StartupEventArgs e) => Application_Startup();
	}
}