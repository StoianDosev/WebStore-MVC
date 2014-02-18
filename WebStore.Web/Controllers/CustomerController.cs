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
    public class CustomerController : Controller
    {
        DataContext db = new DataContext();

        private CustomerIndexViewModel indexModel;

        public CustomerController(IUowData data)
        {
            this.indexModel = new CustomerIndexViewModel(data);
        }
        
        public ActionResult Index()
        {

            this.indexModel.Load();

            return View(indexModel);
        }

        public ActionResult CustomerList(int page)
        {
            this.indexModel.Load(page);

            return PartialView("_CustomersList", this.indexModel.CustomerViewModels);
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            this.indexModel.Load();
            CustomerViewModel customer = new CustomerViewModel();
            customer = indexModel.CustomerViewModelsList.Where(x => x.Id == id.Value).FirstOrDefault();

            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( CustomerViewModel customer)
        {
            if (ModelState.IsValid)
            {
                this.indexModel.CreateCustomer(customer);
                return RedirectToAction("Index");
            }

            return View(customer);
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            this.indexModel.Load();
            CustomerViewModel customer = indexModel.CustomerViewModelsList.Where(x => x.Id == id.Value).FirstOrDefault();
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerViewModel customer)
        {
            if (ModelState.IsValid)
            {
                this.indexModel.UpdateCustomer(customer);
                return View(customer);
            }
            return View(customer);
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
