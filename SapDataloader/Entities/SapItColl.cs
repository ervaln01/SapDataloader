namespace SapDataloader.Entities
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("SAP_IT_COLL")]
	public partial class SapItColl
	{
		[Key]
		[Column("MATNR")]
		[StringLength(18)]
		public string Matnr { get; set; }

		[Key]
		[Column("IDNRK")]
		[StringLength(18)]
		public string Idnrk { get; set; }

		[Column("MNGLG")]
		public decimal? Mnglg { get; set; }

		[Column("MEINS")]
		[StringLength(3)]
		public string Meins { get; set; }

		[Column("DISPO")]
		[StringLength(3)]
		public string Dispo { get; set; }

		[Column("ATBEZ")]
		[StringLength(100)]
		public string Atbez { get; set; }

		[Column("ALTGR")]
		[StringLength(21)]
		public string Altgr { get; set; }

		[Column("OJTXP")]
		[StringLength(255)]
		public string Ojtxp { get; set; }

		[Column("SANKA")]
		[StringLength(100)]
		public string Sanka { get; set; }

		[Column("LINE")]
		public int? Line { get; set; }

		[Column("SYSDATE", TypeName = "datetime")]
		public DateTime? Sysdate { get; set; }
	}
}