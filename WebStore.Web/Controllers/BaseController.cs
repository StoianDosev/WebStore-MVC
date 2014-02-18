using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebStore.Core.Interfaces;
using WebStore.Infrastructure.Repositories;

namespace WebStore.Web.Controllers
{
    public class BaseController : Controller
    {
        protected IUowData db;

        public BaseController(IUowData data)
        {
            this.db = data;
        }

        public BaseController()
            : this(new UowData())
        {

        }
	}
}