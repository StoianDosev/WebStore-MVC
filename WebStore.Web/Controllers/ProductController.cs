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
using PagedList;

namespace WebStore.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private DataContext db = new DataContext();

        private ProductIndexViewModel indexViewModel;

        public ProductController(IUowData data)
        {
            this.indexViewModel = new ProductIndexViewModel(data);
        }


        public ActionResult Index(int languageId = 1)
        {
            indexViewModel.Load(languageId);
            return View(indexViewModel);
           
        }

        public ActionResult ProductList(int languageId = 1, int page = 1)
        {
            this.indexViewModel.Load(languageId, page);

            return PartialView("_ProductList", this.indexViewModel.PagedProductViewModels);
        }

        public ActionResult Details(int? productId, int? languageId, int page = 1)
        {
            if (productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (languageId == null)
            {
                indexViewModel.Load();
            }
            else
            {
                indexViewModel.Load(languageId.Value);
            }

            ProductViewModel product = new ProductViewModel();

            product = indexViewModel.ProductViewModels.Where(x => x.ProductId == productId.Value).FirstOrDefault();

            if (product == null)
            {
                indexViewModel.Load();
                product = indexViewModel.ProductViewModels.Where(x => x.ProductId == productId.Value).FirstOrDefault();
                ViewBag.Message = "No translation to that language yet.";
            }

            product.Load();

            return View(product);
        }

        // GET: /Product/Create
        public ActionResult Create()
        {
            ProductViewModel newProduct = new ProductViewModel();

            newProduct.Load();
            newProduct.SetCategoryTree();

            return View(newProduct);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                int productId = this.indexViewModel.CreateProduct(product);

                product.ProductId = productId;

                return RedirectToAction("Edit", product);
            }

            return View(product);
        }


        public ActionResult Edit(int? productId, int? languageId, int page = 1)
        {
            if (productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (languageId == null)
            {
                this.indexViewModel.Load(1, page);
            }
            else
            {
                this.indexViewModel.Load(languageId.Value, page);
            }

            ProductViewModel product = new ProductViewModel();
            product = this.indexViewModel.PagedProductViewModels.Where(x => x.ProductId == productId.Value).FirstOrDefault();



            if (product == null)
            {

                indexViewModel.Load(1, page);
                product = this.indexViewModel.PagedProductViewModels.Where(x => x.ProductId == productId.Value).FirstOrDefault();
                ViewBag.Message = "No translation to that language yet.";

                //created new product but page is not set
                if (product == null)
                {
                    while (true)
                    {
                        indexViewModel.Load(1, page);
                        if (this.indexViewModel.PagedProductViewModels.Count == 0)
                        {
                            break;
                        }
                        product = this.indexViewModel.PagedProductViewModels.Where(x => x.ProductId == productId.Value).FirstOrDefault();
                        page += 1;
                    }
                    ViewBag.Message = "";
                    page = page - 1;
                }

            }

            product.CurrentPage = page;
            product.Load();
            product.SetCategoryTree();


            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel product)
        {

            var cats = product.CategoryViewModels;

            if (ModelState.IsValid)
            {
                this.indexViewModel.UpdateProduct(product);
                this.indexViewModel.Load(product.LanguageId);
                product.Load();
                product.SetCategoryTree();

                return View(product);
            }

            product.Load();
            product.SetCategoryTree();

            return View(product);
        }

        // GET: /Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: /Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
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
