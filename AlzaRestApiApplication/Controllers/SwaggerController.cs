using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlzaRestApiApplication.Controllers
{
    public class SwaggerController : Controller
    {
        // GET: Swagger
        public ActionResult Index()
        {
            return View();
        }
    }
}