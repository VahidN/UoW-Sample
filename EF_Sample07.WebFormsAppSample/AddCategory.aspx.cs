using System;
using EF_Sample07.DataLayer.Context;
using EF_Sample07.DomainClasses;
using EF_Sample07.IoCConfig;
using EF_Sample07.ServiceLayer;

namespace EF_Sample07.WebFormsAppSample
{
    public partial class AddCategory : BasePage
    {
        public IUnitOfWork UoW { set; get; }
        public IProductService ProductService { set; get; }
        public ICategoryService CategoryService { set; get; }
        public ISessionProvider SessionProvider { set; get; } // todo: use it during the login to set the CurrentConnectionString

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var category = new Category
            {
                Name = txtName.Text,
                Title = txtTitle.Text
            };
            CategoryService.AddNewCategory(category);
            UoW.SaveAllChanges();
            Response.Redirect("~/Default.aspx");
        }
    }
}