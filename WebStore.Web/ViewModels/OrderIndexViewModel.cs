using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebStore.Core.Interfaces;
using WebStore.Infrastructure.Repositories;
using PagedList;
using System.Web.Mvc;
using WebStore.Core.Models;

namespace WebStore.Web.ViewModels
{
    public class OrderIndexViewModel
    {
        private IUowData db;

        public int Id { get; set; }

        public IPagedList<OrderViewModel> OrderViewModels { get; set; }

        public IList<OrderViewModel> OrderViewModelsList { get; set; }

        public List<SelectListItem> SortSelectListItems { get; set; }

        public OrderIndexViewModel()
            : this(new UowData())
        { }

        public OrderIndexViewModel(IUowData data)
        {
            this.db = data;
        }

        public void Load(string isProcessed, int page = 1)
        {

            bool processedValue = true;
            if (!string.IsNullOrEmpty(isProcessed))
            {
                processedValue = bool.Parse(isProcessed);
            }
            else
            {
                isProcessed = string.Empty;
            }

            this.OrderViewModels = db.Orders.All().Where(x => isProcessed == string.Empty || x.IsProcessed == processedValue).Select(x => new OrderViewModel()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Address = x.Address,
                City = x.City,
                IsCanceled = x.IsCanceled,
                IsProcessed = x.IsProcessed,
                OrderDate = x.OrderDate,
                TotalPrice = x.TotalPrice,
                Email = x.Email,
                Phone = x.Phone,
            }).OrderBy(x => x.OrderDate).ToPagedList(page, 5);

            //adding orderdetails 
            foreach (var item in this.OrderViewModels)
            {
                item.OrderDetailsViewModel = db.OrderDetails.All().Where(x=>x.OrderId==item.Id).Select(x =>
                    new OrderDetailsViewModel()
                    {
                        OrderId = x.OrderId.Value,
                        ProductId = x.ProductId,
                        OrderDetailsId = x.Id,
                        Quantity = x.Quantity,
                        UnitPrice = x.UnitPrice,
                        PruductCode= x.Prduct.ProductCode,
                    }).ToList();
            }

            this.OrderViewModelsList = db.Orders.All().Select(x => new OrderViewModel()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Address = x.Address,
                City = x.City,
                IsCanceled = x.IsCanceled,
                IsProcessed = x.IsProcessed,
                OrderDate = x.OrderDate,
                TotalPrice = x.TotalPrice,
                Email = x.Email,
                Phone = x.Phone,
            }).OrderBy(x => x.OrderDate).ToList();

            foreach (var item in this.OrderViewModelsList)
            {
                item.OrderDetailsViewModel = db.OrderDetails.All().Where(x => x.OrderId == item.Id).Select(x =>
                    new OrderDetailsViewModel()
                    {
                        OrderId = x.OrderId.Value,
                        ProductId = x.ProductId,
                        OrderDetailsId =x.Id,
                        Quantity=x.Quantity,
                        UnitPrice = x.UnitPrice,
                        PruductCode = x.Prduct.ProductCode,
                    }).ToList();
            }

            this.SortSelectListItems = new List<SelectListItem>();
            this.SortSelectListItems.Add(new SelectListItem() { Value = string.Empty, Text = "All" });
            this.SortSelectListItems.Add(new SelectListItem() { Value = "True", Text = "Send" });
            this.SortSelectListItems.Add(new SelectListItem() { Value = "False", Text = "Pending" });

        }

        public void CreateOrder(OrderViewModel model)
        {
            Order order = new Order()
            {
                Address = model.Address,
                City = model.City,
                Email = model.Email,
                FirstName = model.FirstName,
                IsCanceled = false,
                IsProcessed = false,
                LastName = model.LastName,
                OrderDate = DateTime.Now,
                Phone = model.Phone,
                TotalPrice = model.TotalPrice,
            };

            db.Orders.Add(order);
            db.SaveChanges();

            List<OrderDetail> orderDetails = new List<OrderDetail>();
            orderDetails = model.OrderDetailsViewModel.Select(x =>
                new OrderDetail()
                {
                    OrderId = order.Id,
                    ProductId = x.ProductId,
                    UnitPrice = x.UnitPrice,
                    Quantity = x.Quantity,
                }).ToList();

            order.OrderDetails = orderDetails;

            ProductIndexViewModel indexProductModel = new ProductIndexViewModel(db);
            indexProductModel.DecreaseQuantityOfProducts(model);

            db.SaveChanges();
        }

        public void UpdateOrder(OrderViewModel model)
        {
            Order order = db.Orders.GetById(model.Id);

            order.Address = model.Address;
            order.City = model.City;
            order.Email = model.Email;
            order.FirstName = model.FirstName;
            order.IsCanceled = model.IsCanceled;
            order.IsProcessed = model.IsProcessed;
            order.LastName = model.LastName;
            order.Phone = model.Phone;
            order.TotalPrice = model.TotalPrice;

            db.SaveChanges();
        }

        public void ChangeCancelState(int id, bool isCanceled)
        {
            var order = db.Orders.GetById(id);
            order.IsCanceled = isCanceled;

            db.SaveChanges();
        }
        public void ChangeProcessState(int id, bool isProcessed)
        {
            var order = db.Orders.GetById(id);
            order.IsProcessed = isProcessed;
            db.SaveChanges();
        }
    }
}