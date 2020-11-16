using ExampleProject.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExampleProject.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _identityContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(ApplicationDbContext identityContext, IHttpContextAccessor httpContextAccessor)
        {
            _identityContext = identityContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public IdentityUser GetUser()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = _identityContext.Users.FirstOrDefault(u => u.Id == userId);
            return user;

        }
        public string GetEmailByUserId(string userId)
        {

            var user = _identityContext.Users.FirstOrDefault(u => u.Id == userId);
            return user.Email;

        }
    }
}
