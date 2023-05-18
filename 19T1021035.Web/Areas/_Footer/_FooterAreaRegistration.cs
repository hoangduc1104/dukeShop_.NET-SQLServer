using System.Web.Mvc;

namespace _19T1021035.Web.Areas._Footer
{
    public class _FooterAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "_Footer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "_Footer_default",
                "_Footer/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}