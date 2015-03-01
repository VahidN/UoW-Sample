using System;
using System.Web.Configuration;
using EF_Sample07.DataLayer.Context;
using EF_Sample07.ServiceLayer.Contracts;

namespace EF_Sample07.WebFormsAppSample
{
    public partial class _Default : BasePage
    {
        public IUnitOfWork UoW { set; get; }
        public IProductService ProductService { set; get; }
        public ISessionProvider SessionProvider { set; get; } // todo: use it during the login to set the CurrentConnectionString

        protected void Page_Load(object sender, EventArgs e)
        {
            // todo: call these method during the login phase, when the user selects the current database name.
            SessionProvider.Store("CurrentConnectionString", "Sample07Context");
            UoW.SetConnectionString(WebConfigurationManager.ConnectionStrings[SessionProvider.Get<string>("CurrentConnectionString")].ConnectionString);


            if (!IsPostBack)
            {
                GridView1.DataSource = ProductService.GetAllProducts();
                GridView1.DataBind();
            }
        }
    }
}