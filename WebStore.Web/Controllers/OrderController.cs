using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebStore.Core.Interfaces;
using WebStore.Core.Models;
using WebStore.Infrastructure;
using WebStore.Web.ViewModels;

namespace WebStore.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private DataContext db = new DataContext();


        private OrderIndexViewModel orderIndexModel;

        public OrderController(IUowData data)
        {
            this.orderIndexModel = new OrderIndexViewModel(data);
        }


        public ActionResult Index(string sortedItem)
        {
            orderIndexModel.Load(sortedItem);

            return View(orderIndexModel);
        }

        public ActionResult OrderList(string sortedItem, int page = 1)
        {
            orderIndexModel.Load(sortedItem, page);
            return PartialView("_OrderList", orderIndexModel.OrderViewModels);
        }

        public ActionResult ConfirmAction(string productId, string processed, string canceled, string sortedItem)
        {
            int id = int.Parse(productId);
            bool isCanceled = bool.Parse(canceled);
            bool isProcessed = bool.Parse(processed);
            orderIndexModel.ChangeCancelState(id, isCanceled);
            orderIndexModel.ChangeProcessState(id, isProcessed);
            return RedirectToAction("Index", "Order", new { sortedItem  });
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            this.orderIndexModel.Load(null);
            OrderViewModel order = this.orderIndexModel.OrderViewModelsList.Where(x => x.Id == id.Value).FirstOrDefault();
           
            if (order == null)
            {
                return HttpNotFound();
            }

            order.Load();

            return View(order);
        }
        
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            this.orderIndexModel.Load(null);
            OrderViewModel order = this.orderIndexModel.OrderViewModelsList.Where(x => x.Id == id.Value).FirstOrDefault();
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrderViewModel order)
        {
            if (ModelState.IsValid)
            {
                this.orderIndexModel.UpdateOrder(order);
                return View(order);
            }
            return View(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
