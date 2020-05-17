using Dapper;
using SqlKata;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public static class SqlKataExtensions
    {

        public static SqlResult GetSqlResult(this Query query)
        {
            var xQuery = query as XQuery;
            var compiler = xQuery.Compiler;
            var connection = xQuery.Connection;

            return compiler.Compile(query);
            
        }

    }
}
