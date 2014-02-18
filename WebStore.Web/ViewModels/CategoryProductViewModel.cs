using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebStore.Web.ViewModels
{
    public class CategoryProductViewModel
    {
        public int CategoryProductId { get; set; }

        public int CategoryId { get; set; }

        public CategoryViewModel CategoryViewModel { get; set; }

        public int ProductId { get; set; }

        public ProductViewModel ProductViewModel { get; set; }
    }
}