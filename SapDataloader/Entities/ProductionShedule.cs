namespace SapDataloader.Entities
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("PRODUCTION_SHEDULE")]
	public partial class ProductionShedule
	{
		[Key]
		[Column("versus", TypeName = "datetime")]
		public DateTime Versus { get; set; }

		[Key]
		[Column("product")]
		[StringLength(50)]
		public string Product { get; set; }

		[Key]
		[Column("model")]
		[StringLength(50)]
		public string Model { get; set; }

		[Key]
		[Column("definition")]
		[StringLength(50)]
		public string Definition { get; set; }

		[Key]
		[Column("date", TypeName = "datetime")]
		public DateTime Date { get; set; }

		[Key]
		[Column("count")]
		public int Count { get; set; }

		[Column("notes")]
		[StringLength(50)]
		public string Notes { get; set; }

		[Key]
		[Column("created", TypeName = "datetime")]
		public DateTime Created { get; set; }

		[Column("line")]
		public int? Line { get; set; }
	}
}