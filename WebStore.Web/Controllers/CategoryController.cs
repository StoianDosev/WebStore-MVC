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
    public class CategoryController : Controller
    {
        private CategoryIndexViewModel indexModel;
        
        public CategoryController(IUowData data)
        {
            indexModel = new CategoryIndexViewModel(data);
        }

        public ActionResult Index(int languageId = 1)
        {
            
            indexModel.Load(languageId);
            indexModel.CategoryViewModels = indexModel.SetTreeCategoriesViewModel();
            return View(indexModel);
        }

        public ActionResult Create()
        {
            CategoryViewModel categoryModel = new CategoryViewModel();
            categoryModel.Load();
            return View(categoryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryViewModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                int categoryId = indexModel.CreateCategory(categoryModel);

                categoryModel.CategoryId = categoryId;


                return RedirectToAction("Edit", categoryModel);
            }

            return View(categoryModel);
        }

        [HttpGet]
        public ActionResult Edit(int? categoryId, int? languageId)
        {
            if (categoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryViewModel categoryModel = new CategoryViewModel();

            if (languageId == null)
            {
                indexModel.Load();
            }
            else
            {
                indexModel.Load(languageId.Value);
            }

            categoryModel = indexModel.CategoryViewModels.Where(x => x.CategoryId == categoryId.Value).FirstOrDefault();

            if (categoryModel == null)
            {
                indexModel.Load();
                categoryModel = indexModel.CategoryViewModels.Where(x => x.CategoryId == categoryId.Value).FirstOrDefault();
                ViewBag.Message = "No translation to that language yet.";

                if(categoryModel==null)
                {

                }
            }
            categoryModel.Load();

            return View(categoryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryViewModel categoryModel)
        {
            
            if (ModelState.IsValid)
            {
                indexModel.UpdateCategory(categoryModel);
                categoryModel.Load();
                return View(categoryModel);
            }
            categoryModel.Load();
            return View(categoryModel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            indexModel.Load();
            var category = indexModel.CategoryViewModels.Where(x => x.CategoryId == id.Value).FirstOrDefault();

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            indexModel.Load();
            CategoryViewModel category = indexModel.CategoryViewModels.Where(x => x.CategoryId == id).FirstOrDefault();
            indexModel.DeleteCategory(category);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //data.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
