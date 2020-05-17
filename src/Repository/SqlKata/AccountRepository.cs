
using Dapper;
using Mir.GameServer.Models;
using Npgsql;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using SqlKata.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SqlKata
{
    public class AccountRepository : IAccountRepository
    {
        private QueryFactory _db;

        public AccountRepository(QueryFactory db)
        {
            _db = db;
        }

        public async Task<Account> FindByUsername(string username)
        {
           return await _db.Query("user.account")
                .Select("id", "username", "email", "password")
                .Where(new { username })
                .FirstOrDefaultAsync<Account>();
        }
    }
}
