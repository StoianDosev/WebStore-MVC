using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebStore.Core.Interfaces;
using WebStore.Core.Models;
using WebStore.Infrastructure.Repositories;

namespace WebStore.Web.ViewModels
{
    public class CategoryIndexViewModel
    {
        private IUowData db;

        private ICollection<SelectListItem> languageSelectItems;

        public ICollection<SelectListItem> LanguageSelectItems
        {
            get { return this.languageSelectItems; }
            set { this.languageSelectItems = value; }
        }

        public ICollection<CategoryViewModel> CategoryViewModels { get; set; }

        public CategoryIndexViewModel(IUowData data)
        {
            this.db = data;
        }

        public CategoryIndexViewModel()
            : this(new UowData())
        {
        }

        public void Load(int languageId = 1)
        {
            //loading languages
            var languages = db.Languages.All().ToList();

            this.languageSelectItems = languages.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();

            //loading categories
            var categoriesLanguages = db.CategoryLanguages.All().ToList();

            this.CategoryViewModels = categoriesLanguages.Where(x => x.LanguageID == languageId && x.Category.IsDeleted == false).Select(x => new CategoryViewModel()
                {
                    CategoryLanguageId = x.Id,
                    Title = x.Title,
                    CreatedOn = x.Category.CreatedOn,
                    CategoryId = x.CategoryID,
                    LanguageId = x.LanguageID,
                    ParentId = x.Category.ParentId,
                    IsDeleted = x.Category.IsDeleted,
                }).OrderBy(x => x.CategoryId).ToList();


            //loading categoryProducts 
            var categoryProducts = db.CategoryProducts.All().Select(x => new CategoryProductViewModel()
            {
                CategoryProductId = x.Id,
                CategoryId = x.CategoryId,
                ProductId = x.ProductId,
            }).ToList();

            foreach (var item in this.CategoryViewModels)
            {
                item.CategoryProductModels = categoryProducts.Where(x => x.CategoryId == item.CategoryId).ToList();
            }
        }


        public List<CategoryViewModel> SetTreeCategoriesViewModel()
        {
            List<CategoryViewModel> tree = new List<CategoryViewModel>();

            var parentCat = this.CategoryViewModels.Where(x => x.ParentId == null);

            foreach (var categoryNode in parentCat)
            {
                tree.Add(categoryNode);
                AddCategoryTreeNodes(tree, categoryNode, 1);
            }
            return tree;
        }

        private void AddCategoryTreeNodes(List<CategoryViewModel> tree, CategoryViewModel category, int level)
        {
            var childCategories = this.CategoryViewModels.Where(x => x.ParentId == category.CategoryId).ToList();

            foreach (var child in childCategories)
            {
                for (int i = 0; i < level; i++)
                {
                    child.Title = "---" + child.Title;
                }
                tree.Add(child);
                AddCategoryTreeNodes(tree, child, level + 1);
            }
        }

        public int CreateCategory(CategoryViewModel model)
        {
            Category cat = new Category()
            {
                CreatedOn = DateTime.Now,
                ParentId = model.ParentId
            };
            db.Categories.Add(cat);
            db.SaveChanges();

            CategoryLanguage catLan = new CategoryLanguage()
            {
                CategoryID = cat.Id,
                LanguageID = model.LanguageId,
                Title = model.Title,
            };
            db.CategoryLanguages.Add(catLan);
            db.SaveChanges();
            return catLan.CategoryID;
        }

        public void UpdateCategory(CategoryViewModel model)
        {
            var category = db.Categories.GetById(model.CategoryId);

            var categoryLanguages = category.CategoryLanguages.Where(x => x.LanguageID == model.LanguageId).FirstOrDefault();

            //adding new translation 
            if (categoryLanguages == null)
            {
                CategoryLanguage catLan = new CategoryLanguage()
                {
                    CategoryID = model.CategoryId,
                    LanguageID = model.LanguageId,
                    Title = model.Title,
                };
                db.CategoryLanguages.Add(catLan);
            }
            else//updating translation
            {
                categoryLanguages.Title = model.Title;
            }

            db.SaveChanges();
        }

        public void DeleteCategory(CategoryViewModel model)
        {
            var cat = db.Categories.GetById(model.CategoryId);
            DeleteSubCategories(cat);
            db.SaveChanges();
        }

        private void DeleteSubCategories(Category category)
        {
            category.IsDeleted = true;
            var subCategories = db.Categories.All().Where(x => x.ParentId == category.Id);
            foreach (var item in subCategories)
            {
                DeleteSubCategories(item);
            }
        }

    }
}