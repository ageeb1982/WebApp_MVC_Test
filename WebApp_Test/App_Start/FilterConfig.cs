using System.Web;
using System.Web.Mvc;

namespace WebApp_Test
{

    /// <summary>
    /// إضافة فلتر Authorize 
    /// لكي يتم المطالبة باسم المستخدم وكلمة السر
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            
            filters.Add(new System.Web.Mvc.AuthorizeAttribute());
        
        }
    }
}
