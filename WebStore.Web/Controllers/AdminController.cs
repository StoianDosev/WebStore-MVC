using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebStore.Core.Interfaces;
using WebStore.Web.ViewModels;

namespace WebStore.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        AdminIndexViewModel indexModel;

        public AdminController(IUowData data)
        {
            indexModel = new AdminIndexViewModel(data);
        }

        public ActionResult Index()
        {
            this.indexModel.Load();

            return View(indexModel);
        }

        public ActionResult Test()
        {
            return Content("aaaa");
        }
        
        public ActionResult Details(string id)
        {

            this.indexModel.Load();
            AdminViewModel adminModel = indexModel.AdminViewModels.Where(x => x.Id == id).FirstOrDefault();
            return View(adminModel);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Create(AdminViewModel admin)
        {
            try
            {
                this.indexModel.Load();
                this.indexModel.CreateAdmin(admin);
                
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        
        public ActionResult Edit(string id)
        {
            this.indexModel.Load();

            AdminViewModel adminModel = indexModel.AdminViewModels.Where(x => x.Id == id).FirstOrDefault();

            return View(adminModel);
        }

        
        [HttpPost]
        public ActionResult Edit(string id, AdminViewModel model)
        {
            try
            {
                this.indexModel.Load();
                this.indexModel.UpdateAdmin(model);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
