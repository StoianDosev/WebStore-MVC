using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebStore.Web.Areas.Client.Models
{
    public class ShoppingCart 
    {
        public int ProductId { get; set; }

        public int ProductLanguageId { get; set; }

        public int LanguageId { get; set; }

        public string ProductName { get; set; }
        
        public decimal Price { get; set; }

        public string Image { get; set; }

        public string ProductCode { get; set; }
        
        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        
    }
}