using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebStore.Core.Interfaces;
using WebStore.Core.Models;
using WebStore.Infrastructure.Repositories;

namespace WebStore.Web.Areas.Client.ViewModels
{
    public class CategoryTreeListModel
    {
        public IUowData db;

        public List<CategoryTree> CategoryTreeModels { get; set; }

        private List<CategoryLanguage> categoryLanguages;

        public CategoryTreeListModel(IUowData data)
        {
            db = data;
            
        }
        public CategoryTreeListModel()
            : this(new UowData())
        { }

        public void Load(int languageId = 1)
        {
            this.categoryLanguages = db.CategoryLanguages.All().ToList();

            this.CategoryTreeModels = this.categoryLanguages.Where(x => x.LanguageID == languageId && x.Category.ParentId == null
                && x.Category.IsDeleted==false).Select(x =>
                new CategoryTree()
                {
                    id = x.CategoryID,
                    label = x.Title,
                }).ToList();

            SetTreeCategory(languageId);
        }

       private void SetTreeCategory(int languageId)
        {
            foreach (var categoryNode in this.CategoryTreeModels)
            {
                AddCategoryTreeNodes(categoryNode, languageId);
            }
        }


        private void AddCategoryTreeNodes(CategoryTree category, int languageId)
        {
            var childCategories = this.categoryLanguages.Where(x => x.Category.ParentId == category.id && x.LanguageID == languageId)
                .Select(x => new CategoryTree()
                {
                    id = x.Category.Id,
                    label = x.Title,
                }).ToList();
            foreach (var child in childCategories)
            {
                category.children.Add(child);
                AddCategoryTreeNodes(child, languageId);
            }
        }
    }
}