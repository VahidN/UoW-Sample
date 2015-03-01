using System.Collections.Generic;
using EF_Sample07.DomainClasses;

namespace EF_Sample07.ServiceLayer.Contracts
{
    public interface ICategoryService
    {
        void AddNewCategory(Category category);
        IList<Category> GetAllCategories();
    }
}
