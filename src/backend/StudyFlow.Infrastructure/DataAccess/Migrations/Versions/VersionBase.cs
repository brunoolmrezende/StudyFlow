﻿using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace StudyFlow.Infrastructure.DataAccess.Migrations.Versions
{
    public abstract class VersionBase : ForwardOnlyMigration
    {
        protected ICreateTableColumnOptionOrWithColumnSyntax CreateTable(string tableName)
        {
            return Create.Table(tableName)
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Active").AsBoolean().NotNullable()
                .WithColumn("CreatedAt").AsDateTime().NotNullable();
        }
    }
}
