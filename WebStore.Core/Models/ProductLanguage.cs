using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Core.Models
{
    public class ProductLanguage
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Manufacturer { get; set; }        

        public string Description { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int LanguageId { get; set; }

        public virtual Language Language { get; set; }
    }
}
