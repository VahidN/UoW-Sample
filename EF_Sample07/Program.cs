using System.Collections.Generic;
using System.Data.Entity;
using EF_Sample07.DataLayer.Context;
using EF_Sample07.DomainClasses;
using EF_Sample07.IoCConfig;
using EF_Sample07.ServiceLayer.Contracts;
using StructureMap.Web.Pipeline;

namespace EF_Sample07
{
    class Program
    {
        static void Main(string[] args)
        {
            initDatabases();

            //todo: در برنامه‌هاي دسكتاپ از حالت هيبريد استفاده نكنيد

            method1();
            method2();
        }

        private static void method2()
        {
            var uow = SmObjectFactory.Container.GetInstance<IUnitOfWork>();
            var categoryService = SmObjectFactory.Container.GetInstance<ICategoryService>();

            var product1 = new Product { Name = "P100", Price = 100 };
            var product2 = new Product { Name = "P200", Price = 200 };
            var category1 = new Category
            {
                Name = "Cat100",
                Title = "Title100",
                Products = new List<Product> { product1, product2 }
            };
            categoryService.AddNewCategory(category1);
            uow.SaveAllChanges();

            new HybridLifecycle().FindCache(null).DisposeAndClear();
            //((IDisposable)uow).Dispose();
        }

        private static void method1()
        {
            // كانتكست را به صورت خودكار ديسپوز مي‌كند
            // البته نه در حالت هيبريد
            using (var container = SmObjectFactory.Container.GetNestedContainer())
            {
                var uow = container.GetInstance<IUnitOfWork>();
                var categoryService = container.GetInstance<ICategoryService>();

                var product1 = new Product { Name = "P100", Price = 100 };
                var product2 = new Product { Name = "P200", Price = 200 };
                var category1 = new Category
                {
                    Name = "Cat100",
                    Title = "Title100",
                    Products = new List<Product> { product1, product2 }
                };
                categoryService.AddNewCategory(category1);
                uow.SaveAllChanges();
            }
        }

        private static void initDatabases()
        {
            // defined in app.config
            string[] connectionStringNames =
            {
                "Sample07Context",
                "Database2012"
            };

            foreach (var connectionStringName in connectionStringNames)
            {
                Database.SetInitializer(
                    new MigrateDatabaseToLatestVersion<Sample07Context, Configuration>(connectionStringName));

                using (var ctx = new Sample07Context(connectionStringName))
                {
                    ctx.Database.Initialize(force: true);
                }
            }
        }
    }
}