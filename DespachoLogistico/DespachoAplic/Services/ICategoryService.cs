using DespachoDomain.Models;
using System.Collections.Generic;

namespace DespachoAplic.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAll();
    }
}
