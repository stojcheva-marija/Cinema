using Cinema.Domain.Domain_models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Repository.Interface
{
   
    public interface IRepository<T> where T: BaseEntity
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
