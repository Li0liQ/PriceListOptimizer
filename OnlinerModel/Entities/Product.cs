using System;
using System.ComponentModel.DataAnnotations;

namespace OnlinerModel
{
	public class Product : BaseEntity
	{
		[Key]
		public int Id { get; set;}

		[Required]
		public string Name { get; set;}

		[Required] // TODO: enforce uniqueness
		public string UrlPart { get; set;}

		[Required] // Required attribute ensures cascade deletion.
		public virtual Manufacturer Manufacturer { get; set;}

		public DateTime? UpdatedDate { get; set;}
	}
}