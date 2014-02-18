using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebStore.Core.Interfaces;
using WebStore.Infrastructure.Repositories;

namespace WebStore.Web.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        public int ProductLanguageId { get; set; }

        public int LanguageId { get; set; }

        public int CategoryProductId { get; set; }

        public string Name { get; set; }

        public string Manufacturer { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Size { get; set; }

        public string Image { get; set; }

        public string ProductCode { get; set; }

        public int Quantity { get; set; }

        public bool InStock { get; set; }

        public bool IsTopProduct { get; set; }

        public int CategoryName { get; set; }

        public string[] CheckListCategory { get; set; }

        public int CurrentPage { get; set; }

        public ICollection<CategoryProductViewModel> CategoryProductModels { get; set; }

        private IUowData db;

        private IEnumerable<SelectListItem> languageSelectItems;

        public IEnumerable<SelectListItem> LanguageSelectItems
        {
            get { return this.languageSelectItems; }
            set { this.languageSelectItems = value; }
        }

        public ICollection<CategoryViewModel> CategoryViewModels { get; set; }

        public ProductViewModel(IUowData data)
        {
            this.db = data;
        }

        public ProductViewModel()
            : this(new UowData())
        {
            
        }
        public void Load()
        {
            var languages = db.Languages.All().ToList();

            this.languageSelectItems = languages.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });

            //loading categories 
            CategoryIndexViewModel categories = new CategoryIndexViewModel(db);

            categories.Load();

            //loading categoryProducts 
            var categoryProducts = db.CategoryProducts.All().Select(x => new CategoryProductViewModel()
            {
                CategoryProductId = x.Id,
                CategoryId = x.CategoryId,
                ProductId = x.ProductId,
            }).ToList();

            this.CategoryProductModels = categoryProducts.Where(x => x.ProductId == ProductId).ToList();

            foreach (var item in this.CategoryProductModels)
            {
                item.CategoryViewModel = categories.CategoryViewModels.Where(x => x.CategoryId == item.CategoryId).FirstOrDefault();
            }
        }

        public void SetCategoryTree()
        {
            CategoryIndexViewModel categories = new CategoryIndexViewModel(db);

            categories.Load();

            this.CategoryViewModels = categories.SetTreeCategoriesViewModel();

            foreach (var item in this.CategoryViewModels)
            {
                item.CategoryProductModels = item.CategoryProductModels.Where(x => x.ProductId == this.ProductId).ToList();
                if(item.CategoryProductModels.Count>0)
                {
                    item.isChecked = true;
                }
            }

        }

    }
}