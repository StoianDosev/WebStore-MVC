using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebStore.Core.Interfaces;
using WebStore.Infrastructure.Repositories;

namespace WebStore.Web.ViewModels
{
    public class AdminViewModel
    {
        public string Id { get; set; }

        public string HashPasword { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool IsEnabled { get; set; }

        private IUowData db;

        public AdminViewModel(IUowData data)
        {
            this.db = data;
        }
        public AdminViewModel()
            : this(new UowData())
        { }
    }
}