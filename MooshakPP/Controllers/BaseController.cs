using System;
using System.Web.Mvc;

namespace MooshakPP.Controllers
{
    public class BaseController : Controller
    {   
        protected ApplicationException onException()
        {
            return null;
        }
    }
}