using System.Web.UI;
using EF_Sample07.IoCConfig;

namespace EF_Sample07.WebFormsAppSample
{
    public class BasePage : Page
    {
        public BasePage()
        {
            SmObjectFactory.Container.BuildUp(this);
        }
    }
}