using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Migratoions
{
    [Migration(20200517103900)]
    public class CreateTableAccount : Migration
    {
        public override void Up()
        {
            Create.Table("account")
                .InSchema("user")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("username").AsString(50).NotNullable().Unique("ux_account_username")
                .WithColumn("email").AsString(500).NotNullable().Unique("ux_account_email")
                .WithColumn("password").AsBinary(72).NotNullable();

        }

        public override void Down()
        {
            Delete.Table("account")
                .InSchema("user");
        }
    }
}
