using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebStore.Core.Interfaces;
using WebStore.Infrastructure;
using WebStore.Infrastructure.Repositories;
using WebStore.Web.ViewModels;

namespace WebStore.Web.Controllers
{
    public class HomeController : BaseController
    {
        

        private AdminIndexViewModel indexModel;
        public HomeController(IUowData data)
        {
            indexModel = new AdminIndexViewModel(data);
        }
        public ActionResult Index()
        {

            indexModel.Load();
            var admins = indexModel.AdminViewModels;
            
            return View(admins);
        }

        [HttpPost]
        public ActionResult Index(string[] CategoryViewModels)
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}