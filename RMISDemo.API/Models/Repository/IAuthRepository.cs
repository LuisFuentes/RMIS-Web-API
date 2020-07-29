using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMISDemo.API.Models.Repository
{
    public interface IAuthRepository
    {

        Task<ApiUser> Register(ApiUser user, string password);

        Task<ApiUser> Login(string username, string password);

        Task<bool> UserExists(string username);
    }
}
