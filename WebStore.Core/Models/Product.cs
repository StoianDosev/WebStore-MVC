using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Core.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public decimal Price { get; set; }

        public int Size { get; set; }

        public string Image { get; set; }

        public string ProductCode { get; set; }

        public int Quantity { get; set; }

        public bool InStock { get; set; }

        public bool IsTopProduct { get; set; }

        public bool isDeleted { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<ProductLanguage> ProductLanguages { get; set; }

        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }

        public Product()
        {
            this.Comments = new HashSet<Comment>();
            this.ProductLanguages = new HashSet<ProductLanguage>();
            this.CategoryProducts = new HashSet<CategoryProduct>();
        }
    }
}
