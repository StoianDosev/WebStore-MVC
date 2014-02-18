using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Core.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public int ProductId { get; set; }

        public virtual Product Prduct { get; set; }

        public int? OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}
