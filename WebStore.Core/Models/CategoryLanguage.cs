using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Core.Models
{
    public class CategoryLanguage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public int CategoryID { get; set; }

        public int LanguageID { get; set; }

        public virtual Category Category { get; set; }

        public virtual Language Language { get; set; }

        

        public CategoryLanguage()
        {
            
        }
    }
}
