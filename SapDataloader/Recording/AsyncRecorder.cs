namespace SapDataloader.Recording
{
	using System.Windows.Media;
	using System.Windows.Controls;
	using System.Windows.Shapes;
	using System.Windows.Threading;
	using System.Collections.Generic;
	using System;
	using System.Windows;

	/// <summary>
	/// Класс, предназначенный для асинхронной записи в окно.
	/// </summary>
	public static class AsyncRecorder
	{
		/// <summary>
		/// Диспетчер очереди элементов потока.
		/// </summary>
		private static Dispatcher _dispatcher;

		/// <summary>
		/// Инициализация полей <see cref="AsyncRecorder"/>.
		/// </summary>
		/// <param name="dispatcher">Диспетчер потока.</param>
		public static void Initialize(Dispatcher dispatcher) => _dispatcher = dispatcher;

		/// <summary>
		/// Безопасное выполнение лямбда-функции.
		/// </summary>
		/// <param name="lambda">Выполняемая функция.</param>
		private static void SafeSetInvoke<T>(Func<T> lambda) => _dispatcher.InvokeAsync(() => { try { _ = lambda(); } catch { } });

		/// <summary>
		/// Безопасное выполнение лямбда-функции.
		/// </summary>
		/// <param name="lambda">Выполняемая функция.</param>
		private static T SafeGetInvoke<T>(Func<T> lambda) => _dispatcher.Invoke(() => { try { return lambda(); } catch { return default; } });

		public static void SetIndication(this Ellipse indicator, bool? isConnected) => SafeSetInvoke(() => indicator.Fill = isConnected == null ? Brushes.LightGray : isConnected.Value ? Brushes.LightGreen : Brushes.Red);
		public static void SetText(this TextBox textBox, string msg) => SafeSetInvoke(() => textBox.Text = msg);
		public static string GetText(this TextBox textBox) => SafeGetInvoke(() => textBox.Text);
		public static void SetContent(this Label label, string content) => SafeSetInvoke(() => label.Content = content);
		public static void SetVisibility(this Label label, bool visible) => SafeSetInvoke(() => label.Visibility = visible ? Visibility.Visible : Visibility.Hidden);
		public static void SetEnable(this Button button, bool enable) => SafeSetInvoke(() => button.IsEnabled = enable);
		public static object GetSelectedItem(this ComboBox comboBox) => SafeGetInvoke(() => comboBox.SelectedItem);
		public static bool GetChecked(this CheckBox checkBox) => SafeGetInvoke(() => checkBox.IsChecked.Value);
		public static void SetDataContext<T>(this DataGrid dataGrid, List<T> content) => SafeSetInvoke(() => dataGrid.DataContext = content);
	}
}