namespace SapDataloader.Entities
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("SPECIFICATIONS")]
	public partial class Specification
	{
		[Required]
		[Column("PRODUCT")]
		[StringLength(10)]
		public string Product { get; set; }

		[Required]
		[Column("MODEL")]
		[StringLength(255)]
		public string Model { get; set; }

		[Required]
		[Column("SPECIFICATIONS")]
		[StringLength(255)]
		public string Specifications { get; set; }

		[Column("LINE")]
		public short Line { get; set; }

		[Column("STATION")]
		public short Station { get; set; }

		[Column("SYSDATE", TypeName = "datetime")]
		public DateTime Sysdate { get; set; }
	}
}