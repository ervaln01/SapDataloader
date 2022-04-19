namespace SapDataloader.Dataloading
{
	using SapDataloader.Enums;
	using SapDataloader.Helpers;

	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// Класс взаимодействия с загрузчиками и визуальными формами.
	/// </summary>
	public static class Hub
	{
		/// <summary>
		/// Словарь загрузчиков данных по <see cref="DlType"/>.
		/// </summary>
		public static Dictionary<DlType, Dataloader> Dataloaders;

		/// <summary>
		/// Словарь визуальных элементов <see cref="DlType"/>.
		/// </summary>
		public static Dictionary<DlType, VisualForm> VisualForms;

		/// <summary>
		/// Статический конструктор класса <see cref="Hub"/>.
		/// </summary>
		static Hub()
		{
			Dataloaders = (Enum.GetValues(typeof(DlType)) as IEnumerable<DlType>).ToDictionary(type => type, type => new Dataloader(type));
			VisualForms = new Dictionary<DlType, VisualForm>();
		}
	}
}