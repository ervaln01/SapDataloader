namespace SapDataloader.Entities
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("PRODUCT")]
	public partial class Product
	{
		[Key]
		[Column("PRODUCT")]
		[StringLength(10)]
		public string Product1 { get; set; }

		[Column("CUSTPRODUCT")]
		[StringLength(10)]
		public string Custproduct { get; set; }

		[Column("SHORTNAME")]
		[StringLength(20)]
		public string Shortname { get; set; }

		[Column("CAPACITY")]
		public short? Capacity { get; set; }

		[Column("MODEL")]
		[StringLength(7)]
		public string Model { get; set; }

		[Column("PRODUCTTYPE")]
		[StringLength(20)]
		public string Producttype { get; set; }

		[Column("PRODUCTNAME")]
		[StringLength(45)]
		public string Productname { get; set; }

		[Column("EAN")]
		[StringLength(13)]
		public string Ean { get; set; }

		[Column("EANR")]
		[StringLength(20)]
		public string Eanr { get; set; }

		[Column("COLOR")]
		[StringLength(50)]
		public string Color { get; set; }

		[Column("VERSION")]
		[StringLength(20)]
		public string Version { get; set; }

		[Column("LIGHTING")]
		[StringLength(20)]
		public string Lighting { get; set; }

		[Column("COMPONENTMODEL")]
		[StringLength(20)]
		public string Componentmodel { get; set; }
	}
}