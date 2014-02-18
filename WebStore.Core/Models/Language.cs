using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Core.Models
{
    public class Language
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Language")]
        public string Name { get; set; }

        public virtual ICollection<CategoryLanguage> CategoryLanguages { get; set; }

        public virtual ICollection<ProductLanguage> ProductLanguages { get; set; }

        public Language()
        {
            this.CategoryLanguages = new HashSet<CategoryLanguage>();
            this.ProductLanguages = new HashSet<ProductLanguage>();
        }
    }
}
