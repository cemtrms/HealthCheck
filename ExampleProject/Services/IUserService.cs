using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleProject.Services
{
    public interface IUserService
    {
        IdentityUser GetUser();
        string GetEmailByUserId(string userId);
    }
}
