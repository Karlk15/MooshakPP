using System.Web;
using System.Web.Mvc;
using MooshakPP.Handlers;

namespace MooshakPP
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandlerAttribute());
        }
    }
}
