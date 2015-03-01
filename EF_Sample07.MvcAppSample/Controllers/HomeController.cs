using System.Web.Configuration;
using System.Web.Mvc;
using EF_Sample07.DataLayer.Context;
using EF_Sample07.DomainClasses;
using EF_Sample07.IoCConfig;
using EF_Sample07.ServiceLayer;

namespace EF_Sample07.MvcAppSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISessionProvider _sessionProvider;  // todo: use it during the login to set the CurrentConnectionString
        readonly IProductService _productService;
        readonly ICategoryService _categoryService;
        readonly IUnitOfWork _uow;
        public HomeController(
            ISessionProvider sessionProvider,
            IUnitOfWork uow,
            IProductService productService,
            ICategoryService categoryService)
        {
            _sessionProvider = sessionProvider;
            _productService = productService;
            _categoryService = categoryService;
            _uow = uow;


            // todo: call these method during the login phase, when the user selects the current database name.
            sessionProvider.Store("CurrentConnectionString", "Sample07Context");
            uow.SetConnectionString(WebConfigurationManager.ConnectionStrings[_sessionProvider.Get<string>("CurrentConnectionString")].ConnectionString);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var list = _productService.GetAllProducts();
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CategoriesList = new SelectList(_categoryService.GetAllCategories(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (this.ModelState.IsValid)
            {
                _productService.AddNewProduct(product);
                _uow.SaveAllChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCategory(Category category)
        {
            if (this.ModelState.IsValid)
            {
                _categoryService.AddNewCategory(category);
                _uow.SaveAllChanges();
            }
            return RedirectToAction("Index");
        }
    }
}