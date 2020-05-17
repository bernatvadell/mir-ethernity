
using Dapper;
using Mir.GameServer.Models;
using Npgsql;
using SqlKata;
using SqlKata.Compilers;
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
        private IDbConnection _db;
        private Compiler _compiler;

        public AccountRepository(IDbConnection db, Compiler compiler)
        {
            _db = db;
            _compiler = compiler;
        }

        public async Task<Account> FindByUsername(string username)
        {
            var query = new Query("user.account")
                .Select("id", "username", "email", "password")
                .Where(new { username });

            var result = _compiler.Compile(query);

            return await _db.QueryFirstOrDefaultAsync<Account>(result.Sql, result.NamedBindings);
        }
    }
}
