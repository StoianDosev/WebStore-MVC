using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Core.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        public decimal TotalPrice { get; set; }

        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }

        public string City { get; set; }

        public bool IsProcessed { get;set; }

        public bool IsCanceled { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
    }
}
