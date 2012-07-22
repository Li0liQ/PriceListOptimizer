using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
	public class Category : BaseEntity
	{
		[Key]
		public int Id { get; set;}

		[Required]
		public string Name { get; set;}

		[Required] // TODO: enforce uniqueness
		public string UrlPart { get; set;}

		public ICollection<Manufacturer> Manufacturers { get; set; }
	}
}