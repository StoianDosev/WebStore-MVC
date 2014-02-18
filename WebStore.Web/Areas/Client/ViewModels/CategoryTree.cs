using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebStore.Core.Interfaces;
using WebStore.Infrastructure.Repositories;

namespace WebStore.Web.Areas.Client.ViewModels
{
    public class CategoryTree
    {
        public int id { get; set; }

        public string label { get; set; }

        public List<CategoryTree> children { get; set; }

        public CategoryTree()
        {
            this.children = new List<CategoryTree>();
        }
    }
}