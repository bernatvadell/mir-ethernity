using Mir.GameServer.Models;
using System;
using System.Threading.Tasks;

namespace Repository
{
    public interface IAccountRepository
    {
        Task<Account> FindByUsername(string username);
    }
}
