using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Contexts;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Persistence.Implementations
{
    public class UserPersist : GeneralPersist, IUserPersist
    {
        private readonly ProEventosContext _context;

        public UserPersist(ProEventosContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await this._context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await this._context.Users.FirstOrDefaultAsync(us =>
                us.Id.Equals(id));
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await this._context.Users.FirstOrDefaultAsync(us =>
                us.UserName.Equals(username));
        }
    }
}
