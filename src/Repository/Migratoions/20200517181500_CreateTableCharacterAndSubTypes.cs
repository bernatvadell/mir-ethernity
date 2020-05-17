using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Migratoions
{
    [Migration(20200517181500)]
    public class CreateTableCharacterAndSubTypes : Migration
    {
        public override void Down()
        {
            Delete.Table("character")
              .InSchema("user");
        }

        public override void Up()
        {
            Create.Schema("content");

            Create.Table("class")
                .InSchema("content")
                .WithColumn("id").AsByte().PrimaryKey("pk_class")
                .WithColumn("name").AsString(50).Unique();

            Insert.IntoTable("class")
                .InSchema("content")
                .Row(new { id = 0, name = "Warrior" })
                .Row(new { id = 1, name = "Wizard" })
                .Row(new { id = 2, name = "Taoist" })
                .Row(new { id = 3, name = "Assassin" });

            Create.Table("attack_mode")
              .InSchema("content")
              .WithColumn("id").AsByte().PrimaryKey("pk_attack_mode")
              .WithColumn("name").AsString(50).Unique();

            Insert.IntoTable("attack_mode")
               .InSchema("content")
               .Row(new { id = 0, name = "Peace" })
               .Row(new { id = 1, name = "Group" })
               .Row(new { id = 2, name = "Guild" })
               .Row(new { id = 3, name = "War, Red and Brown" })
               .Row(new { id = 4, name = "All" });

            Create.Table("gender")
               .InSchema("content")
               .WithColumn("id").AsByte().PrimaryKey("pk_gender")
               .WithColumn("name").AsString().Unique();

            Insert.IntoTable("gender")
                .InSchema("content")
                .Row(new { id = 0, name = "Male" })
                .Row(new { id = 1, name = "Female" });

            Create.Table("character")
                .InSchema("user")
                .WithColumn("id").AsInt32().PrimaryKey("pk_character").Identity()
                .WithColumn("name").AsString(50).NotNullable().Unique("ux_character_name")
                .WithColumn("account_id").AsInt32().NotNullable()
                .WithColumn("creation_date").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentUTCDateTime)
                .WithColumn("class_id").AsByte().NotNullable()
                .WithColumn("gender_id").AsByte().NotNullable()
                .WithColumn("hair_color").AsInt32().NotNullable().WithDefaultValue(0xFFFFFFFF)
                .WithColumn("level").AsInt16().NotNullable().WithDefaultValue(0)
                .WithColumn("experience").AsDecimal(16, 4).NotNullable().WithDefaultValue(0)
                .WithColumn("current_hp").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("current_mp").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("attack_mode_id").AsInt32().NotNullable().WithDefaultValue(0);

            Create.ForeignKey()
                .FromTable("character").InSchema("user")
                .ForeignColumn("account_id")
                .ToTable("account").InSchema("user")
                .PrimaryColumn("id");

            Create.ForeignKey()
                .FromTable("character").InSchema("user")
                .ForeignColumn("class_id")
                .ToTable("class").InSchema("content")
                .PrimaryColumn("id");

            Create.ForeignKey()
                .FromTable("character").InSchema("user")
                .ForeignColumn("gender_id")
                .ToTable("gender").InSchema("content")
                .PrimaryColumn("id");

            Create.ForeignKey()
                .FromTable("character").InSchema("user")
                .ForeignColumn("attack_mode_id")
                .ToTable("attack_mode").InSchema("content")
                .PrimaryColumn("id");
        }
    }
}
