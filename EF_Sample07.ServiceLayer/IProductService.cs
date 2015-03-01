using EF_Sample07.DomainClasses;
using System.Collections.Generic;

namespace EF_Sample07.ServiceLayer
{
    public interface IProductService
    {
        void AddNewProduct(Product product);
        IList<Product> GetAllProducts();
    }
}
