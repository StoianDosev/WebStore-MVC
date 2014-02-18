using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Core.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Created on")]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }

        public int? ParentId { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<CategoryProduct> CategoryProducts { get; set; }

        public virtual ICollection<CategoryLanguage> CategoryLanguages { get; set; }

        public Category()
        {
            this.CategoryProducts = new HashSet<CategoryProduct>();
            this.CategoryLanguages = new HashSet<CategoryLanguage>();
        }
    }
}
