using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Migratoions
{
    [Migration(20200517103400)]
    public class CreateSchemaUser : Migration
    {
        public override void Down()
        {
            Delete.Schema("user");
        }

        public override void Up()
        {
            Create.Schema("user");
        }
    }
}
