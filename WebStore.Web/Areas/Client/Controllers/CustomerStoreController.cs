using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebStore.Core.Interfaces;
using WebStore.Web.Areas.Client.ViewModels;
using WebStore.Web.ViewModels;

namespace WebStore.Web.Areas.Client.Controllers
{
    public class CustomerStoreController : Controller
    {
        private CustomerIndexViewModel indexModel;

        public CustomerStoreController(IUowData data)
        {
            indexModel = new CustomerIndexViewModel(data);
        }


        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(CustomerLoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = indexModel.FindCustomerByNameAndPass(model.UserName, model.Password);
                if (user != null)
                {
                    Session["Customer"] = user;

                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            return View(model);
        }


        [AllowAnonymous]
        public ActionResult Logout(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            Session["Customer"] = null;
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public ActionResult Register(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(CustomerRegisterViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                indexModel.Load();
                CustomerViewModel customer = new CustomerViewModel();
                customer.Password = model.Password;
                customer.FirstName = model.FirstName;
                customer.LastName = model.LastName;
                customer.Address = model.Address;
                customer.City = model.City;
                customer.Email = model.Email;
                customer.Phone = model.Phone;
                customer.Username = model.UserName;

                int id = indexModel.CreateCustomer(customer);
                if (id != 0)
                {
                    Session["Customer"] = customer;
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ViewBag.Message = "Error in creating a user!";
                    return View(model);
                }
            }
            ViewBag.Message = "Not a valid user!";
            return View(model);
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("TopProducts", "Store");
            }
        }

        public ActionResult Index()
        {

            indexModel.Load();
            var cust = indexModel.FindCustomerByNameAndPass("petar1", "123456");

            return View();
        }
    }
}