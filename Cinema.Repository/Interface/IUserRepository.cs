using Cinema.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<ShopApplicationUser> GetAll();
        ShopApplicationUser Get(string id);
        void Insert(ShopApplicationUser entity);
        void Update(ShopApplicationUser entity);
        void Delete(ShopApplicationUser entity);
    }
}
