using Dapper;
using Mir.GameServer.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.PGSQL
{
    public class AccountRepository : IAccountRepository
    {
        private NpgsqlConnection _db;

        public AccountRepository(NpgsqlConnection db)
        {
            _db = db;
        }

        public async Task<Account> FindByUsername(string username)
        {
            return await _db.QueryFirstOrDefaultAsync<Account>("select id,username,email,password from \"user\".account WHERE username=@username", new { username });
        }
    }
}
