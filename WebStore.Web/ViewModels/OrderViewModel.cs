using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebStore.Core.Interfaces;
using WebStore.Infrastructure.Repositories;

namespace WebStore.Web.ViewModels
{
    public class OrderViewModel
    {
        private IUowData db;

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public bool IsProcessed { get; set; }

        public bool IsCanceled { get; set; }
        
        public IList<OrderDetailsViewModel> OrderDetailsViewModel { get; set; }

        public OrderViewModel(IUowData data)
        {
            this.db = data;
        }
        public OrderViewModel()
            : this(new UowData())
        { }

        public void Load(int languageId = 1)
        {
            //loading order details
            //this.OrderDetailsViewModel = db.OrderDetails.All().Where(x => x.OrderId.Value == Id).Select(x =>
            //    new OrderDetailsViewModel()
            //    {
            //        OrderId = x.OrderId.Value,
            //        ProductId = x.ProductId,
            //        OrderDetailsId = x.Id,
            //        Quantity = x.Quantity,
            //        UnitPrice = x.UnitPrice,
            //        PruductCode = x.Prduct.ProductCode,
            //    }).ToList();


            //loading products name
            foreach (var item in this.OrderDetailsViewModel)
            {
                var productLanguage = db.ProductLanguages.All().Where(x => x.LanguageId == languageId && x.ProductId == item.ProductId).FirstOrDefault();

                item.ProductName = productLanguage.Name;
            }

            OrderDate = DateTime.Now;
        }

    }
}