namespace SapDataloader.Recording
{
	using SapDataloader.Entities;

	using System.Data.Entity;

	/// <summary>
	/// Класс взаимодействия с базой данных BekoLLCSQL.
	/// </summary>
	public partial class AnyContext : DbContext
	{
		/// <summary>
		/// Конструктор класса <see cref="AnyContext"/>.
		/// </summary>
		/// <param name="connectionString">Строка подключения.</param>
		public AnyContext(string connectionString = "DBConnection") : base(connectionString) { }

		/// <summary>
		/// Таблица PRODUCT.
		/// </summary>
		public virtual DbSet<Product> Products { get; set; }

		/// <summary>
		/// Таблица PRODUCTION_SHEDULE.
		/// </summary>
		public virtual DbSet<ProductionShedule> ProductionShedules { get; set; }

		/// <summary>
		/// Конфигурирует сущности.
		/// </summary>
		/// <param name="modelBuilder">Построитель модели.</param>
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product>().HasKey(e => e.Product1);
			modelBuilder.Entity<ProductionShedule>().HasKey(e => new { e.Versus, e.Product, e.Model, e.Definition, e.Date, e.Count, e.Created });
		}
	}
}