namespace SapDataloader.Helpers
{
	using System.Windows.Controls;
	using System.Windows.Shapes;

	/// <summary>
	/// Визуальные элементы для загрузчика.
	/// </summary>
	public class VisualForm
	{
		/// <summary>
		/// Флажок автозагрузки.
		/// </summary>
		public CheckBox AutoDL { get; private set; }

		/// <summary>
		/// Поле вывода времени следующей выгрузки.
		/// </summary>
		public Label NextDL { get; private set; }

		/// <summary>
		/// Поле вывода успешности выгрузки.
		/// </summary>
		public Label Success { get; private set; }

		/// <summary>
		/// Поле вывода длительности последней выгрузки.
		/// </summary>
		public Label Duration { get; private set; }

		/// <summary>
		/// Поле вывода даты последней выгрузки.
		/// </summary>
		public Label Date { get; private set; }

		/// <summary>
		/// Эллипс-индикатор выгрузки.
		/// </summary>
		public Ellipse IndicatorDL { get; private set; }

		/// <summary>
		/// Эллипс-индикатор сохранения.
		/// </summary>
		public Ellipse IndicatorSave { get; private set; }

		/// <summary>
		/// Поле вывода общего количества задач.
		/// </summary>
		public Label TaskCount { get; private set; }

		/// <summary>
		/// Поле вывода количества завершенных задач.
		/// </summary>
		public Label TaskCompleted { get; private set; }

		/// <summary>
		/// Кнопка перехода к окну настроек.
		/// </summary>
		public Button Settings { get; private set; }

		/// <summary>
		/// Кнопка перехода к окну результатов.
		/// </summary>
		public Button Results { get; private set; }

		/// <summary>
		/// Кнопка перехода к окну ручной выгрузки.
		/// </summary>
		public Button Manual { get; private set; }

		/// <summary>
		/// Конструктор класса <see cref="VisualForm"/>.
		/// </summary>
		/// <param name="autoDl">Флажок автозагрузки.</param>
		/// <param name="nextDl">Поле вывода времени следующей выгрузки.</param>
		/// <param name="success">Поле вывода успешности выгрузки.</param>
		/// <param name="duration">Поле вывода длительности последней выгрузки.</param>
		/// <param name="date">Поле вывода даты последней выгрузки.</param>
		/// <param name="indicatorDL">Эллипс-индикатор выгрузки.</param>
		/// <param name="indicatorSave">Эллипс-индикатор сохранения.</param>
		/// <param name="taskCount">Поле вывода общего количества задач.</param>
		/// <param name="taskCompleted">Поле вывода количества завершенных задач.</param>
		/// <param name="settings">Кнопка перехода к окну настроек.</param>
		/// <param name="results">Кнопка перехода к окну результатов.</param>
		/// <param name="manual">Кнопка перехода к окну ручной выгрузки.</param>
		public VisualForm(CheckBox autoDl, Label nextDl, Label success, Label duration, Label date, Ellipse indicatorDL,
			Ellipse indicatorSave, Label taskCount, Label taskCompleted, Button settings, Button results, Button manual)
		{
			AutoDL = autoDl;
			NextDL = nextDl;
			Success = success;
			Duration = duration;
			Date = date;
			IndicatorDL = indicatorDL;
			IndicatorSave = indicatorSave;
			TaskCount = taskCount;
			TaskCompleted = taskCompleted;
			Settings = settings;
			Results = results;
			Manual = manual;
		}
	}
}