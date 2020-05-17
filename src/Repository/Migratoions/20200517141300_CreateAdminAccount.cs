using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Migratoions
{
    [Migration(20200517141300)]
    public class CreateAdminAccount : Migration
    {
        public override void Down()
        {
            Delete.FromTable("account")
                .Row(new { username = "admin" });
        }

        public override void Up()
        {
            Insert.IntoTable("account")
                .InSchema("user")
                .Row(new
                {
                    username = "admin",
                    email = "admin@admin.com",
                    // pass@word1 hashed with bcrypt algorithm
                    password = "$2y$12$wLpOwc.1KJVz/z.cSGdsaemvDUPlWcQPFD04l7wFDh0NXZDTTqTIm",
                    is_admin = true
                });
        }
    }
}
