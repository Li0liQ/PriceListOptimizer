using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
	public class Manufacturer : BaseEntity
	{
		[Key]
		public int Id { get; set;}

		[Required]
		public string Name { get; set;}

		[Required] // TODO: enforce uniqueness
		public string UrlPart { get; set;}

		[Required] // ensure cascade deletion
		public virtual Category Category { get; set;}

		public virtual ICollection<Product> Products { get; set; }
	}
}