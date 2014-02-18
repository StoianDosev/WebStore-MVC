using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebStore.Core.Interfaces;
using WebStore.Infrastructure.Repositories;

namespace WebStore.Web.ViewModels
{
    public class OrderDetailsViewModel
    {
        public int OrderDetailsId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string PruductCode { get; set; }

        public int OrderId { get; set; }
        

        private IUowData db;

        public OrderDetailsViewModel(IUowData data)
        {
            this.db = data;
        }

        public OrderDetailsViewModel()
            : this(new UowData())
        { }

        

    }
}