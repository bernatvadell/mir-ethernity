using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.PGSQL
{
    public class AccountRepository : IAccountRepository
    {
        private NpgsqlConnection _db;

        public AccountRepository(NpgsqlConnection db)
        {
            _db = db;
        }
    }
}
