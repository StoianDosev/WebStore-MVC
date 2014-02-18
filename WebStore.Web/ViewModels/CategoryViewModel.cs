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
    public class CategoryViewModel
    {
        private IUowData db;

        private IEnumerable<SelectListItem> languageSelectItems;

        private ICollection<SelectListItem> categorySelectItems;

        private IEnumerable<CategoryViewModel> parentCategories;

        public IEnumerable<SelectListItem> LanguageSelectItems
        {
            get { return this.languageSelectItems; }
            set { this.languageSelectItems = value; }
        }

        public ICollection<SelectListItem> CategorySelectItems
        {
            get { return this.categorySelectItems; }
            set { this.categorySelectItems = value; }
        }

        public ICollection<CategoryProductViewModel> CategoryProductModels { get; set; }

        public int CategoryLanguageId { get; set; }

        public int CategoryProductId { get; set; }

        public int CategoryId { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ParentId { get; set; }

        public int LanguageId { get; set; }

        public string Title { get; set; }

        public bool IsDeleted { get; set; }

        public bool isChecked { get; set; }

        public ICollection<CategoryViewModel> ChildrenCategoryViews { get; set; }

        public CategoryViewModel(IUowData data)
        {
            this.db = data;
        }
        public CategoryViewModel()
            : this(new UowData())
        {

        }


        public void Load()
        {
            //load languages select
            var languages = db.Languages.All().ToList();

            this.languageSelectItems = languages.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });

            //load categories select
            var categoriesLanguages = db.CategoryLanguages.All().ToList();

            this.parentCategories = categoriesLanguages.Where(x => x.LanguageID == 1 && x.Category.IsDeleted == false).Select(x => new CategoryViewModel()
            {
                CategoryLanguageId = x.Id,
                Title = x.Title,
                CreatedOn = x.Category.CreatedOn,
                CategoryId = x.CategoryID,
                LanguageId = x.LanguageID,
                ParentId = x.Category.ParentId,
                IsDeleted = x.Category.IsDeleted,
            }).OrderBy(x => x.CategoryId).ToList();


            var categories = GetTreeCategoriesViewModel();

            this.categorySelectItems = categories.Select(x =>
               new SelectListItem { Text = x.Title, Value = x.CategoryId.ToString() }).ToList();
            this.categorySelectItems.Add(new SelectListItem()
            {
                Value = string.Empty,
                Text = "Root",
                Selected = true,
            });


            int catId;
            foreach (var item in this.categorySelectItems)
            {
                bool isInteger = int.TryParse(item.Value, out catId);
                if (isInteger)
                {
                    if (catId == ParentId)
                    {
                        item.Selected = true;
                    }
                }

                //if (int.Parse(item.Value) == ParentId)
                //{
                //    item.Selected = true;
                //}
            }
        }

        private List<CategoryViewModel> GetTreeCategoriesViewModel()
        {
            List<CategoryViewModel> tree = new List<CategoryViewModel>();

            var parentCat = this.parentCategories.Where(x => x.ParentId == null);

            foreach (var categoryNode in parentCat)
            {
                tree.Add(categoryNode);
                AddCategoryTreeNodes(tree, categoryNode, 1);
            }
            return tree;
        }

        private void AddCategoryTreeNodes(List<CategoryViewModel> tree, CategoryViewModel category, int level)
        {
            var childCategories = this.parentCategories.Where(x => x.ParentId == category.CategoryId).ToList();

            foreach (var child in childCategories)
            {
                for (int i = 0; i < level; i++)
                {
                    child.Title = "--" + child.Title;
                }
                tree.Add(child);
                AddCategoryTreeNodes(tree, child, level + 1);
            }
        }


    }
}