using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "SubCategory Name")]
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        // foreign key CategoryId is from Category
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
