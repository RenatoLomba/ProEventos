using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProEventos.Domain.Enum;

namespace ProEventos.Domain.Identity
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Title Title { get; set; }
        public string Description { get; set; }
        public Function Function { get; set; }
        public string AvatarUri { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }

        public virtual string FullName { get => $"{this.FirstName} {this.LastName}"; }
    }
}
