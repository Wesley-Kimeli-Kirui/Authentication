using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenAuthor.Data;
using AuthenAuthor.Entities;
using AuthenAuthor.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace AuthenAuthor.Services
{
    public class UserService : IUserInterface
    {
        private readonly AuthDbContext _context;
        public UserService(AuthDbContext context)
        {
            _context = context;
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();

        }
        public async Task<string> RegisterUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return await Task.FromResult("User Registered Successfully");
        }
    }
}