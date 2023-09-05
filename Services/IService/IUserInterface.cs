using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenAuthor.Entities;

namespace AuthenAuthor.Services.IService
{
    public interface IUserInterface
    {
        Task<string> RegisterUser(User user);
        Task<User> GetUserByEmail(string email);
    }
}