using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
	public class Product : BaseEntity
	{
		[Key]
		public int Id { get; set;}

		[Required]
		public string Name { get; set;}

		[Required] // TODO: enforce uniqueness
		public string UrlPart { get; set;}

		[Required] // ensure cascade deletion
		public virtual Manufacturer Manufacturer { get; set;}

		public DateTime? UpdatedDate { get; set;}
	}
}