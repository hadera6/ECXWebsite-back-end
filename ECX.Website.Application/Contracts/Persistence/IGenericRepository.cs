using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.Contracts.Persistence
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(string id);
        Task<T> Add(T entity);
        Task <T>Update(T entity);
        Task Delete(T entity);
        Task<bool> Exists(string id);
    }
}
