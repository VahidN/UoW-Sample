using System.Collections.Generic;
using EF_Sample07.DomainClasses;

namespace EF_Sample07.ServiceLayer.Contracts
{
    public interface IProductService
    {
        void AddNewProduct(Product product);
        IList<Product> GetAllProducts();
    }
}
